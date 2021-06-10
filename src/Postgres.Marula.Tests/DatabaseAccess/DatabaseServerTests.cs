using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Parsing;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// Database server access tests.
	/// </summary>
	internal class DatabaseServerTests : DatabaseAccessTestFixtureBase
	{
		[Test]
		public async Task GetRawParameterValueTest(
			[ValueSource(nameof(ParameterNameTestCaseSource))]
			(IParameterLink ParameterLink, bool WithRange) testCase)
		{
			var databaseServer = GetService<IDatabaseServer>();
			var rawParameterValue = await databaseServer.GetRawParameterValueAsync(testCase.ParameterLink);
			Assert.AreEqual(testCase.WithRange, rawParameterValue is RawRangeParameterValue);
		}

		/// <summary>
		/// Test case source for <see cref="GetRawParameterValueTest"/>. 
		/// </summary>
		private static IEnumerable<(IParameterLink ParameterLink, bool WithRange)> ParameterNameTestCaseSource()
			=> new (IParameterLink, bool)[]
			{
				(new ParameterLink("autovacuum"), false),
				(new ParameterLink("autovacuum_vacuum_cost_delay"), true),
				(new ParameterLink("cursor_tuple_fraction"), true),
				(new ParameterLink("deadlock_timeout"), true),

				(new ParameterLink("log_rotation_age"), true),
				(new ParameterLink("checkpoint_flush_after"), true),
				(new ParameterLink("track_counts"), false),
				(new ParameterLink("autovacuum_vacuum_scale_factor"), true)
			};

		/// <summary>
		/// Apply empty collection of parameter values.
		/// </summary>
		[Test]
		public async Task EmptyParametersCollectionTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(ImmutableArray<IParameterValue>.Empty);
			Assert.Pass();
		}

		/// <summary>
		/// Apply single parameter value.
		/// </summary>
		[Test]
		public async Task ApplySingleParameterValueTest(
			[ValueSource(nameof(ValuesToApplySource))]
			IParameterValue valueToApply)
		{
			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(new[] {valueToApply});

			// let postmaster reload configuration. 
			await Task.Delay(millisecondsDelay: 100);

			var rawParameterValue = await databaseServer.GetRawParameterValueAsync(valueToApply.ParameterLink);

			var parameterValueParser = GetService<IParameterValueParser>();
			var parsedValue = parameterValueParser.Parse(valueToApply.ParameterLink, rawParameterValue);
			Assert.AreEqual(valueToApply, parsedValue);
		}

		/// <summary>
		/// Parameter values source. 
		/// </summary>
		private static IEnumerable<IParameterValue> ValuesToApplySource()
		{
			yield return new TimeSpanParameterValue(
				new ParameterLink("autovacuum_vacuum_cost_delay"),
				TimeSpan.FromMilliseconds(value: 10));

			yield return new MemoryParameterValue(
				new ParameterLink("checkpoint_flush_after"),
				new Memory(512 * 1024));

			// has range [0..1] in pg_settings
			yield return new FractionParameterValue(
				new ParameterLink("cursor_tuple_fraction"),
				value: 0.128M);

			// has range [0..100] in pg_settings
			yield return new FractionParameterValue(
				new ParameterLink("autovacuum_analyze_scale_factor"),
				value: 0.004M);

			yield return new BooleanParameterValue(
				new ParameterLink("autovacuum"),
				value: true);
		}

		/// <summary>
		/// Apply multiple parameter values.
		/// </summary>
		[Test]
		public async Task ApplyMultipleParameterValuesTest()
		{
			var parameterValues = new IParameterValue[]
			{
				new TimeSpanParameterValue(
					new ParameterLink("autovacuum_naptime"),
					TimeSpan.FromSeconds(value: 12)),

				new MemoryParameterValue(
					new ParameterLink("effective_cache_size"),
					new Memory(4 * 1024 * 1024 * 1024UL)),

				new MemoryParameterValue(
					new ParameterLink("work_mem"),
					new Memory(8 * 1024 * 1024)),

				new MemoryParameterValue(
					new ParameterLink("shared_buffers"),
					new Memory(2 * 1024 * 1024 * 1024UL))
			};

			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(parameterValues);

			// let postmaster reload configuration. 
			await Task.Delay(millisecondsDelay: 100);

			var parameterValueParser = GetService<IParameterValueParser>();

			var valuesFromServer = await parameterValues
				.Select(parameterValue => parameterValue.ParameterLink)
				.SelectAsync(async parameterLink =>
				{
					var rawParameterValue = await databaseServer.GetRawParameterValueAsync(parameterLink);
					return parameterValueParser.Parse(parameterLink, rawParameterValue);
				});

			parameterValues
				.Zip(valuesFromServer)
				.ForEach(tuple => Assert.AreEqual(tuple.First, tuple.Second));
		}

		/// <summary>
		/// Get database server parameter context. 
		/// </summary>
		[Test]
		public async Task GetParameterContextTest(
			[ValueSource(nameof(ContextTestCaseSource))]
			(IParameterLink ParameterLink, ParameterContext ExpectedContext) testCase)
		{
			var databaseServer = GetService<IDatabaseServer>();
			var parameterContext = await databaseServer.GetParameterContextAsync(testCase.ParameterLink);
			Assert.AreEqual(testCase.ExpectedContext, parameterContext);
		}

		/// <summary>
		/// Test case source for <see cref="GetParameterContextTest"/>. 
		/// </summary>
		private static IEnumerable<(IParameterLink ParameterLink, ParameterContext ExpectedContext)> ContextTestCaseSource()
		{
			yield return (new ParameterLink("autovacuum"), ParameterContext.Sighup);
			yield return (new ParameterLink("shared_buffers"), ParameterContext.Postmaster);
			yield return (new ParameterLink("autovacuum_vacuum_scale_factor"), ParameterContext.Sighup);
			yield return (new ParameterLink("track_counts"), ParameterContext.Superuser);
		}

		/// <summary>
		/// Get current Write-Ahead Log insert location.
		/// </summary>
		[Test]
		public async Task GetLogSeqNumberTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			var currentLogSeqNumber = await databaseServer.GetCurrentLogSeqNumberAsync();
			Assert.AreNotEqual(default(LogSeqNumber), currentLogSeqNumber);
		}

		/// <summary>
		/// Get Postgres server version.
		/// </summary>
		[Test]
		public async Task GetPostgresVersionTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			var postgresVersion = await databaseServer.GetPostgresVersionAsync();
			Assert.IsNotNull(postgresVersion);
		}

		/// <summary>
		/// Get average table size.
		/// </summary>
		[Test]
		public async Task GetAverageTableSizeTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			var averageTableSize = await databaseServer.GetAverageTableSizeAsync();
			Assert.AreNotEqual(0, averageTableSize);
		}

		/// <summary>
		/// Get average bloat fraction.
		/// </summary>
		[Test]
		public async Task GetAverageBloatFractionTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			var averageBloatFraction = await databaseServer.GetAverageBloatFractionAsync();
			Assert.IsTrue(
				averageBloatFraction >= decimal.Zero && averageBloatFraction <= decimal.One,
				$"actual averageBloatFraction value: {averageBloatFraction}");
		}

		/// <summary>
		/// Get all parent to child links between tables in database.
		/// </summary>
		[Test]
		public async Task GetAllHierarchicalLinksTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			var parentToChildLinks = await databaseServer.GetAllHierarchicalLinks().ToArrayAsync();

			Assert.AreNotEqual(0, parentToChildLinks.Length);

			parentToChildLinks
				.All(link => !link.Equals(default(ParentToChild)))
				.To(Assert.IsTrue);
		}
	}
}
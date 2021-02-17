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
			(NonEmptyString ParameterName, bool WithRange) testCase)
		{
			var databaseServer = GetService<IDatabaseServer>();
			var rawParameterValue = await databaseServer.GetRawParameterValueAsync(testCase.ParameterName);
			Assert.AreEqual(testCase.WithRange, rawParameterValue.ValidRange is not null);
		}

		/// <summary>
		/// Test case source for <see cref="GetRawParameterValueTest"/>. 
		/// </summary>
		private static IEnumerable<(NonEmptyString ParameterName, bool WithRange)> ParameterNameTestCaseSource()
			=> new (NonEmptyString, bool)[]
			{
				("autovacuum", false),
				("autovacuum_vacuum_cost_delay", true),
				("cursor_tuple_fraction", true),
				("deadlock_timeout", true),

				("log_rotation_age", true),
				("checkpoint_flush_after", true),
				("track_counts", false),
				("autovacuum_vacuum_scale_factor", true)
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

			var valueFromDatabase = await databaseServer.GetRawParameterValueAsync(valueToApply.ParameterLink.Name);
			Assert.AreEqual(valueToApply, valueFromDatabase);
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
					new ParameterLink("deadlock_timeout"),
					TimeSpan.FromMilliseconds(value: 800)),

				new MemoryParameterValue(
					new ParameterLink("log_rotation_size"),
					new Memory(16 * 1024 * 1024)),

				new TimeSpanParameterValue(
					new ParameterLink("log_rotation_age"),
					TimeSpan.FromHours(value: 12)),

				new FractionParameterValue(
					new ParameterLink("autovacuum_vacuum_scale_factor"),
					value: 0.008M)
			};

			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(parameterValues);

			var valuesFromServer = await parameterValues
				.Select(parameterValue => parameterValue.ParameterLink.Name)
				.SelectAsync(async parameterName => await databaseServer.GetRawParameterValueAsync(parameterName));

			// let postmaster reload configuration. 
			await Task.Delay(millisecondsDelay: 100);

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
			(NonEmptyString ParameterName, ParameterContext ExpectedContext) testCase)
		{
			var databaseServer = GetService<IDatabaseServer>();
			var parameterContext = await databaseServer.GetParameterContextAsync(testCase.ParameterName);
			Assert.AreEqual(testCase.ExpectedContext, parameterContext);
		}

		/// <summary>
		/// Test case source for <see cref="GetParameterContextTest"/>. 
		/// </summary>
		private static IEnumerable<(NonEmptyString ParameterName, ParameterContext ExpectedContext)> ContextTestCaseSource()
		{
			yield return ("autovacuum", ParameterContext.Sighup);
			yield return ("shared_buffers", ParameterContext.Postmaster);
			yield return ("autovacuum_vacuum_scale_factor", ParameterContext.Sighup);
			yield return ("track_counts", ParameterContext.Superuser);
		}
	}
}
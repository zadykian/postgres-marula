using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.Calculations.Parameters.Values.Base;
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
		/// <summary>
		/// Get timespan parameter value from database server.
		/// </summary>
		[Test]
		public async Task GetTimeSpanParameterValueTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			const string parameterName = "autovacuum_naptime";
			var parameterValue = await databaseServer.GetParameterValueAsync(parameterName);

			Assert.IsInstanceOf<TimeSpanParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.Milliseconds, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
		}

		/// <summary>
		/// Get memory parameter value from database server.
		/// </summary>
		[Test]
		public async Task GetMemoryParameterValueTest()
		{
			var databaseServer = GetService<IDatabaseServer>();
			const string parameterName = "effective_cache_size";
			var parameterValue = await databaseServer.GetParameterValueAsync(parameterName);

			Assert.IsInstanceOf<MemoryParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.Bytes, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
		}

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
		/// Apply single timespan parameter value.
		/// </summary>
		[Test]
		public async Task ApplySingleTimeSpanParameterValueTest()
		{
			var valueToApply = new TimeSpanParameterValue(
				new ParameterLink("autovacuum_vacuum_cost_delay"),
				TimeSpan.FromMilliseconds(value: 10));

			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(new[] {valueToApply});

			var valueFromDatabase = await databaseServer.GetParameterValueAsync(valueToApply.ParameterLink.Name);
			Assert.AreEqual(valueToApply, valueFromDatabase);
		}

		/// <summary>
		/// Apply single memory parameter value.
		/// </summary>
		[Test]
		public async Task ApplySingleMemoryParameterValueTest()
		{
			var valueToApply = new MemoryParameterValue(
				new ParameterLink("checkpoint_flush_after"),
				new Memory(512 * 1024));

			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(new[] {valueToApply});

			var valueFromDatabase = await databaseServer.GetParameterValueAsync(valueToApply.ParameterLink.Name);
			Assert.AreEqual(valueToApply, valueFromDatabase);
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
					TimeSpan.FromHours(value: 12))
			};

			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(parameterValues);

			var valuesFromServer = await parameterValues
				.Select(parameterValue => parameterValue.ParameterLink.Name)
				.SelectAsync(async parameterName => await databaseServer.GetParameterValueAsync(parameterName));

			parameterValues
				.Zip(valuesFromServer)
				.ForEach(tuple => Assert.AreEqual(tuple.First, tuple.Second));
		}
	}
}
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// Database server access tests.
	/// </summary>
	internal class DatabaseServerTests : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
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
		/// Apply single parameter value. 
		/// </summary>
		[Test]
		public async Task ApplySingleParameterValueTest()
		{
			var parameterValues = new[]
			{
				new TimeSpanParameterValue(
					new ParameterLink("autovacuum_naptime"),
					TimeSpan.FromSeconds(value: 30))
			};

			var databaseServer = GetService<IDatabaseServer>();
			await databaseServer.ApplyToConfigurationAsync(parameterValues);
		}
	}
}
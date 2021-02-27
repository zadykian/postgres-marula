using System;
using NUnit.Framework;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.DatabaseAccess.Configuration;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.Infrastructure
{
	/// <summary>
	/// Application configuration tests.
	/// </summary>
	internal class ConfigurationTests : SingleComponentTestFixtureBase<InfrastructureSolutionComponent>
	{
		[Test]
		public void ConnectionStringTest()
			=> GetService<IDatabaseAccessConfiguration>()
				.GetConnectionString()
				.To(connectionString => Assert.IsNotEmpty(connectionString));

		[Test]
		public void RecalculationIntervalTest()
			=> GetService<ICalculationsConfiguration>()
				.GetRecalculationInterval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		[Test]
		public void AutoAdjustmentParameterTest()
			=> GetService<ICalculationsConfiguration>()
				.AutoAdjustIsEnabled()
				.To(_ => Assert.Pass());
	}
}
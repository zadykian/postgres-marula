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
				.ConnectionString()
				.To(connectionString => Assert.IsNotEmpty(connectionString));

		[Test]
		public void RecalculationIntervalTest()
			=> GetService<ICalculationsConfiguration>()
				.RecalculationInterval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		[Test]
		public void AutoAdjustmentParameterTest()
			=> GetService<ICalculationsConfiguration>()
				.AutoAdjustmentIsEnabled()
				.To(_ => Assert.Pass());

		[Test]
		public void TargetRelationsBloatFractionTest()
			=> GetService<ICalculationsConfiguration>()
				.TargetRelationsBloatFraction()
				.To(_ => Assert.Pass());
	}
}
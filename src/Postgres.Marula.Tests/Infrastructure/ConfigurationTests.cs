using System;
using NUnit.Framework;
using Postgres.Marula.Infrastructure;
using Postgres.Marula.Infrastructure.Configuration;
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
			=> GetService<IAppConfiguration>()
				.GetConnectionString()
				.To(connectionString => Assert.IsNotEmpty(connectionString));

		[Test]
		public void RecalculationIntervalTest()
			=> GetService<IAppConfiguration>()
				.GetRecalculationInterval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		[Test]
		public void AutoAdjustmentParameterTest()
			=> GetService<IAppConfiguration>()
				.AutoAdjustIsEnabled()
				.To(_ => Assert.Pass());
	}
}
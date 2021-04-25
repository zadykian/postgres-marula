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
	internal class ConfigurationTests : SingleComponentTestFixtureBase<InfrastructureAppComponent>
	{
		/// <summary>
		/// <see cref="IDatabaseAccessConfiguration.ConnectionString"/> test.
		/// </summary>
		[Test]
		public void ConnectionStringTest()
			=> GetService<IDatabaseAccessConfiguration>()
				.ConnectionString()
				.To(connectionString => Assert.IsNotEmpty(connectionString));

		/// <summary>
		/// <see cref="ICalculationsConfiguration.RecalculationInterval"/> test.
		/// </summary>
		[Test]
		public void RecalculationIntervalTest()
			=> GetService<ICalculationsConfiguration>()
				.RecalculationInterval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		/// <summary>
		/// <see cref="ICalculationsConfiguration.AutoAdjustmentIsEnabled"/> test.
		/// </summary>
		[Test]
		public void AutoAdjustmentParameterTest()
			=> GetService<ICalculationsConfiguration>()
				.AutoAdjustmentIsEnabled()
				.To(_ => Assert.Pass());
	}
}
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
		/// <see cref="IGeneralConfiguration.RecalculationInterval"/> test.
		/// </summary>
		[Test]
		public void RecalculationIntervalTest()
			=> GetService<ICalculationsConfiguration>()
				.General()
				.RecalculationInterval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		/// <summary>
		/// <see cref="IGeneralConfiguration.AutoAdjustmentIsEnabled"/> test.
		/// </summary>
		[Test]
		public void AutoAdjustmentParameterTest()
			=> GetService<ICalculationsConfiguration>()
				.General()
				.AutoAdjustmentIsEnabled()
				.To(_ => Assert.Pass());

		/// <summary>
		/// <see cref="IGeneralConfiguration.AgentApiUri"/> test.
		/// </summary>
		[Test]
		public void AgentApiUriTest()
			=> GetService<ICalculationsConfiguration>()
				.General()
				.AgentApiUri()
				.To(_ => Assert.Pass());

		/// <summary>
		/// <see cref="IPeriodicLoggingConfiguration.Interval"/> test.
		/// </summary>
		[Test]
		public void AutovacuumIntervalTest()
			=> GetService<ICalculationsConfiguration>()
				.Autovacuum()
				.Interval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		/// <summary>
		/// <see cref="IPeriodicLoggingConfiguration.RollingWindow"/> test.
		/// </summary>
		[Test]
		public void AutovacuumMovingAverageWindowTest()
			=> GetService<ICalculationsConfiguration>()
				.Autovacuum()
				.RollingWindow()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		/// <summary>
		/// <see cref="IPeriodicLoggingConfiguration.Interval"/> test.
		/// </summary>
		[Test]
		public void WalIntervalTest()
			=> GetService<ICalculationsConfiguration>()
				.Wal()
				.Interval()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));

		/// <summary>
		/// <see cref="IPeriodicLoggingConfiguration.RollingWindow"/> test.
		/// </summary>
		[Test]
		public void WalMovingAverageWindowTest()
			=> GetService<ICalculationsConfiguration>()
				.Wal()
				.RollingWindow()
				.To(recalculationInterval => Assert.IsTrue(
					((TimeSpan) recalculationInterval).TotalSeconds > 0
				));
	}
}
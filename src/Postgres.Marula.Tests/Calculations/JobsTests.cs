using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Tests.Calculations.Base;
using Postgres.Marula.Tests.Calculations.FakeServices;

namespace Postgres.Marula.Tests.Calculations
{
	/// <summary>
	/// <see cref="IJobs"/> tests.
	/// </summary>
	internal class JobsTests : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run all application jobs.
		/// </summary>
		[Test]
		public async Task RunAndStopAllJobsTest()
		{
			var jobs = GetService<IJobs>();

			jobs.StartAll();

			await Task.Delay(TimeSpan.FromSeconds(5));

			var databaseTracker = GetService<IDatabaseServerAccessTracker>();
			Assert.IsTrue(databaseTracker.ApplyMethodWasCalled);

			jobs.StopAll();
		}
	}
}
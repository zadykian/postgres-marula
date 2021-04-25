using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Tests.Calculations.Base;
using Postgres.Marula.Tests.Calculations.FakeServices;

namespace Postgres.Marula.Tests.Calculations
{
	/// <summary>
	/// <see cref="IJob"/> tests.
	/// </summary>
	internal class CalculationJobTest : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run calculations job.
		/// </summary>
		[Test]
		public async Task RunCalculationJob()
		{
			var calculationJob = GetService<IJob>();
			calculationJob.Run();

			var configuration = GetService<ICalculationsConfiguration>();
			await Task.Delay(configuration.RecalculationInterval() + TimeSpan.FromMilliseconds(value: 100));

			var databaseTracker = GetService<IDatabaseServerAccessTracker>();
			Assert.IsTrue(databaseTracker.ApplyMethodWasCalled);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Jobs;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Tests.Calculations.Base;
using Postgres.Marula.Tests.Calculations.FakeServices;

namespace Postgres.Marula.Tests.Calculations
{
	/// <summary>
	/// <see cref="IJob"/> tests.
	/// </summary>
	internal class JobTests : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run general calculations job.
		/// </summary>
		[Test]
		public async Task GeneralCalculationsJobTest()
		{
			var calculationJob = GetService<IEnumerable<IJob>>().Single(job => job is GeneralCalculationsJob);
			calculationJob.Run();

			var configuration = GetService<ICalculationsConfiguration>();
			await Task.Delay(configuration.RecalculationInterval() + TimeSpan.FromMilliseconds(value: 100));

			var databaseTracker = GetService<IDatabaseServerAccessTracker>();
			Assert.IsTrue(databaseTracker.ApplyMethodWasCalled);
		}
	}
}
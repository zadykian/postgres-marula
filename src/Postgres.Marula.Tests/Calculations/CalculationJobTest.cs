using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.Jobs;
using Postgres.Marula.Tests.Calculations.Base;
using Postgres.Marula.Tests.Calculations.FakeServices;

namespace Postgres.Marula.Tests.Calculations
{
	/// <summary>
	/// <see cref="ICalculationJob"/> tests.
	/// </summary>
	internal class CalculationJobTest : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run calculations job.
		/// </summary>
		[Test]
		public async Task RunCalculationJob()
		{
			var calculationJob = GetService<ICalculationJob>();
			calculationJob.Run();

			await Task.Delay(millisecondsDelay: 2000);

			var databaseTracker = GetService<IDatabaseServerAccessTracker>();
			Assert.IsTrue(databaseTracker.ApplyMethodWasCalled);
		}
	}
}
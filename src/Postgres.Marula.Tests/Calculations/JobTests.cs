using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Infrastructure.Extensions;
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
		/// Run all application jobs.
		/// </summary>
		[Test]
		public async Task RunAllJobsTest()
		{
			GetService<IEnumerable<IJob>>().ForEach(job => job.Run());

			var configuration = GetService<ICalculationsConfiguration>();
			await Task.Delay(configuration.General().RecalculationInterval() + TimeSpan.FromMilliseconds(value: 500));

			var databaseTracker = GetService<IDatabaseServerAccessTracker>();
			Assert.IsTrue(databaseTracker.ApplyMethodWasCalled);
		}
	}
}
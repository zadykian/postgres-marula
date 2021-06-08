using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using NUnit.Framework;
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
			var allJobs = GetService<IEnumerable<IJob>>().ToImmutableArray();

			allJobs.ForEach(job => job.Run());

			await Task.Delay(TimeSpan.FromSeconds(5));

			var databaseTracker = GetService<IDatabaseServerAccessTracker>();
			Assert.IsTrue(databaseTracker.ApplyMethodWasCalled);

			allJobs.ForEach(job => job.Stop());
		}
	}
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.HttpApi.Common;

namespace Postgres.Marula.App.Host.Api
{
	/// <summary>
	/// Access to long-running jobs.
	/// </summary>
	public class JobsController : ApiControllerBase, IJobs
	{
		private readonly IJobs jobs;

		/// <param name="jobs">
		/// All long-running jobs.
		/// </param>
		public JobsController(IJobs jobs) => this.jobs = jobs;

		/// <summary>
		/// Get info about all jobs.
		/// </summary>
		[HttpGet]
		public IAsyncEnumerable<IJobInfo> InfoAboutAllAsync() => jobs.InfoAboutAllAsync();

		/// <summary>
		/// Start all jobs.
		/// </summary>
		[HttpPatch]
		public async ValueTask StartAllAsync() => await jobs.StartAllAsync();

		/// <summary>
		/// Stop all executing jobs.
		/// </summary>
		[HttpPatch]
		public async ValueTask StopAllAsync() => await jobs.StopAllAsync();
	}
}
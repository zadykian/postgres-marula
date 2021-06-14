using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.WebApi.Common;

namespace Postgres.Marula.AppHost.Api
{
	/// <summary>
	/// Access to long-running jobs.
	/// </summary>
	public class JobsController : ApiControllerBase
	{
		private readonly IJobs jobs;

		/// <param name="jobs">
		/// All long-running jobs.
		/// </param>
		public JobsController(IJobs jobs) => this.jobs = jobs;

		/// <inheritdoc cref="IJobs.InfoAboutAllAsync"/>
		[HttpGet]
		public async Task<IActionResult> InfoAboutAllAsync() => Ok(await jobs.InfoAboutAllAsync().ToArrayAsync());

		/// <inheritdoc cref="IJobs.StartAllAsync"/>
		[HttpPatch]
		public async Task<IActionResult> StartAllAsync()
		{
			await jobs.StartAllAsync();
			return Ok();
		}

		/// <inheritdoc cref="IJobs.StopAllAsync"/>
		[HttpPatch]
		public async Task<IActionResult> StopAllAsync()
		{
			await jobs.StopAllAsync();
			return Ok();
		}
	}
}
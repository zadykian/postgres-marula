using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Postgres.Marula.AppControl.Configuration;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Infrastructure.Http;

namespace Postgres.Marula.AppControl.PeriodicJobs
{
	/// <inheritdoc cref="IJobs" />
	/// <remarks>
	/// This implementations accesses remote host via HTTP.
	/// </remarks>
	internal class RemoteJobs : HttpComponentBase, IJobs
	{
		public RemoteJobs(IControlAppConfiguration configuration) : base(configuration.HostApiUri())
		{
		}

		/// <inheritdoc />
		async IAsyncEnumerable<IJobInfo> IJobs.InfoAboutAllAsync()
		{
			var jobInfos = await PerformRequestAsync<JobInfo[]>(HttpMethod.Get, "Jobs/InfoAboutAll");
			foreach (var jobInfo in jobInfos) yield return jobInfo;
		}

		/// <inheritdoc />
		async ValueTask IJobs.StartAllAsync()
			=> await PerformRequestAsync<Unit>(HttpMethod.Patch, "Jobs/StartAll");

		/// <inheritdoc />
		async ValueTask IJobs.StopAllAsync()
			=> await PerformRequestAsync<Unit>(HttpMethod.Patch, "Jobs/StopAll");
	}
}
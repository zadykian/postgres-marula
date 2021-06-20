using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.Configuration;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.Http;
using Postgres.Marula.Infrastructure.JsonConverters;

namespace Postgres.Marula.App.Control.PeriodicJobs
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
			var jobInfos = await PerformRequestAsync<IEnumerable<JobInfo>>(HttpMethod.Get, "Jobs/InfoAboutAll").ConfigureAwait(false); // todo
			foreach (var jobInfo in jobInfos) yield return jobInfo;
		}

		/// <inheritdoc />
		async ValueTask IJobs.StartAllAsync()
			=> await PerformRequestAsync<Unit>(HttpMethod.Patch, "Jobs/StartAll");

		/// <inheritdoc />
		async ValueTask IJobs.StopAllAsync()
			=> await PerformRequestAsync<Unit>(HttpMethod.Patch, "Jobs/StopAll");

		protected override JsonSerializerOptions ConfigureSerializerOptions(JsonSerializerOptions serializerOptions)
		{
			serializerOptions.Converters.Add(new NonEmptyStringJsonConverter());
			return base.ConfigureSerializerOptions(serializerOptions);
		}
	}
}
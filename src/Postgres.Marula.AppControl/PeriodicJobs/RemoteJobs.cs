using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;

namespace Postgres.Marula.AppControl.PeriodicJobs
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementations accesses remote host via HTTP.
	/// </remarks>
	internal class RemoteJobs : IJobs
	{
		/// <inheritdoc />
		IAsyncEnumerable<IJobInfo> IJobs.InfoAboutAllAsync() => throw new System.NotImplementedException();

		/// <inheritdoc />
		ValueTask IJobs.StartAllAsync() => throw new System.NotImplementedException();

		/// <inheritdoc />
		ValueTask IJobs.StopAllAsync() => throw new System.NotImplementedException();
	}
}
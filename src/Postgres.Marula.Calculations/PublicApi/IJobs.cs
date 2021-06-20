using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;

namespace Postgres.Marula.Calculations.PublicApi
{
	/// <summary>
	/// All long-running jobs.
	/// </summary>
	public interface IJobs
	{
		/// <summary>
		/// Get info about all jobs.
		/// </summary>
		IAsyncEnumerable<IJobInfo> InfoAboutAllAsync();

		/// <summary>
		/// Start all jobs.
		/// </summary>
		ValueTask StartAllAsync();

		/// <summary>
		/// Stop all executing jobs.
		/// </summary>
		ValueTask StopAllAsync();
	}
}
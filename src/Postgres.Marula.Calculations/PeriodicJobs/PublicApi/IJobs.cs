using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postgres.Marula.Calculations.PeriodicJobs.PublicApi
{
	/// <summary>
	/// All long-running jobs.
	/// </summary>
	public interface IJobs
	{
		/// <summary>
		/// Get info about all jobs.
		/// </summary>
		IAsyncEnumerable<IJobInfo> InfoAboutAll();

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
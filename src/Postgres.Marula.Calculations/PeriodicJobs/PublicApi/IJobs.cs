using System.Collections.Generic;

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
		IReadOnlyCollection<IJobInfo> InfoAboutAll();

		/// <summary>
		/// Start all jobs.
		/// </summary>
		void StartAll();

		/// <summary>
		/// Stop all executing jobs.
		/// </summary>
		void StopAll();
	}
}
namespace Postgres.Marula.Calculations.PeriodicJobs.PublicApi
{
	/// <summary>
	/// All long-running jobs.
	/// </summary>
	public interface IJobs
	{
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
namespace Postgres.Marula.Calculations.PeriodicJobs.Base
{
	/// <summary>
	/// Component responsible for long-time periodic process.
	/// </summary>
	internal interface IJob
	{
		/// <summary>
		/// Start job.
		/// </summary>
		void Start();

		/// <summary>
		/// Stop job.
		/// </summary>
		void Stop();
	}
}
namespace Postgres.Marula.Calculations.PeriodicJobs.PublicApi
{
	/// <summary>
	/// Job's state.
	/// </summary>
	public enum JobState : byte
	{
		/// <summary>
		/// Job is stopped.
		/// </summary>
		Stopped,

		/// <summary>
		/// Job is running.
		/// </summary>
		Running
	}
}
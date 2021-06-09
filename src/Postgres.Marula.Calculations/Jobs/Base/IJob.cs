namespace Postgres.Marula.Calculations.Jobs.Base
{
	/// <summary>
	/// Component responsible for long-time periodic process.
	/// </summary>
	public interface IJob
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
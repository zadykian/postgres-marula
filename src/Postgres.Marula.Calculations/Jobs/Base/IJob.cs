namespace Postgres.Marula.Calculations.Jobs.Base
{
	/// <summary>
	/// Component responsible for long-time periodic process.
	/// </summary>
	public interface IJob
	{
		/// <summary>
		/// Run job.
		/// </summary>
		void Run();
	}
}
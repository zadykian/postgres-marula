namespace Postgres.Marula.Calculations.Jobs
{
	/// <summary>
	/// Process responsible for long-time periodic parameters calculation.
	/// </summary>
	public interface ICalculationJob
	{
		/// <summary>
		/// Run parameters calculation job.
		/// </summary>
		void Run();
	}
}
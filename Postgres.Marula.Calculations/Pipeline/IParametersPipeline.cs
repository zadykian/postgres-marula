using System.Threading.Tasks;

namespace Postgres.Marula.Calculations.Pipeline
{
	/// <summary>
	/// Database parameters management pipeline.
	/// </summary>
	internal interface IParametersPipeline
	{
		/// <summary>
		/// Run parameters management pipeline.
		/// </summary>
		Task RunAsync();
	}
}
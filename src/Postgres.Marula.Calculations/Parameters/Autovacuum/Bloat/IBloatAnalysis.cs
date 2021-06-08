using System.Threading.Tasks;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat
{
	/// <summary>
	/// Database tables bloat analysis.
	/// </summary>
	internal interface IBloatAnalysis
	{
		/// <summary>
		/// Perform average database bloat analysis. 
		/// </summary>
		Task<BloatCoefficients> ExecuteAsync();
	}
}
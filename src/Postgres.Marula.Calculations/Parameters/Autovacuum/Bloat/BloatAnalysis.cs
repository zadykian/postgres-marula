using System.Threading.Tasks;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat
{
	/// <inheritdoc />
	internal class BloatAnalysis : IBloatAnalysis
	{
		private readonly ISystemStorage systemStorage;
		private readonly ICalculationsConfiguration configuration;

		public BloatAnalysis(
			ISystemStorage systemStorage,
			ICalculationsConfiguration configuration)
		{
			this.systemStorage = systemStorage;
			this.configuration = configuration;
		}

		/// <inheritdoc />
		async Task<BloatCoefficients> IBloatAnalysis.ExecuteAsync()
		{
			
		}
	}
}
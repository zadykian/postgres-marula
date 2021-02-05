using System.Collections.Generic;

namespace Postgres.Marula.Infrastructure.SolutionComponents.Factory
{
	/// <summary>
	/// Factory of solution components.
	/// </summary>
	public interface ISolutionComponentsFactory
	{
		/// <summary>
		/// Create all components declared in solution.
		/// </summary>
		IEnumerable<ISolutionComponent> CreateAll();
	}
}
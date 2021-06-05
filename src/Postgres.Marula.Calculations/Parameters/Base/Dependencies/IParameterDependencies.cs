using System.Collections.Generic;

// Type of parameter which implements IParameter interface.
using ParameterType = System.Type;

namespace Postgres.Marula.Calculations.Parameters.Base.Dependencies
{
	/// <summary>
	/// Dependencies of parameter.
	/// </summary>
	internal interface IParameterDependencies
	{
		/// <summary>
		/// Set that current parameter depends on value of parameter <typeparamref name="TParameter"/>.
		/// This means that <typeparamref name="TParameter"/> must be calculated before current parameter. 
		/// </summary>
		IParameterDependencies DependsOn<TParameter>() where TParameter : IParameter;

		/// <summary>
		/// Get all configured dependencies. 
		/// </summary>
		IReadOnlyCollection<ParameterType> All();
	}
}
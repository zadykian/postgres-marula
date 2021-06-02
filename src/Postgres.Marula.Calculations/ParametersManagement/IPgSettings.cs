using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.ParametersManagement
{
	/// <summary>
	/// Database server configuration.
	/// </summary>
	internal interface IPgSettings
	{
		/// <summary>
		/// Apply parameter values <paramref name="parameterValues"/>
		/// to database server configuration.
		/// </summary>
		Task ApplyAsync(IEnumerable<IParameterValue> parameterValues);

		/// <summary>
		/// Read value of type <typeparamref name="TValue"/> of parameter <typeparamref name="TParameter"/>. 
		/// </summary>
		Task<TValue> ReadAsync<TParameter, TValue>()
			where TParameter : IParameter
			where TValue : IEquatable<TValue>;
	}
}
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
		/// Apply parameter value <paramref name="parameterValue"/> to settings.
		/// </summary>
		void Apply(IParameterValue parameterValue);

		/// <summary>
		/// Get all applied parameter values.
		/// </summary>
		IAsyncEnumerable<ParameterValueWithStatus> AllAppliedAsync();

		/// <summary>
		/// Read value of type <typeparamref name="TValue"/> of parameter <typeparamref name="TParameter"/>. 
		/// </summary>
		Task<TValue> ReadAsync<TParameter, TValue>()
			where TParameter : IParameter
			where TValue : IEquatable<TValue>;

		/// <summary>
		/// Send all applied parameter values to database server.
		/// </summary>
		Task FlushAsync();
	}
}
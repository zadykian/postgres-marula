using System;
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
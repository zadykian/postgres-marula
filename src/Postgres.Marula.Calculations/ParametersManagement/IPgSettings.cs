using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

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
		/// Read value of type <typeparamref name="TValue"/>
		/// of parameter named <paramref name="parameterName"/>. 
		/// </summary>
		Task<TValue> ReadAsync<TValue>(NonEmptyString parameterName) where TValue : IEquatable<TValue>;
	}
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.HttpApi.Common;

namespace Postgres.Marula.App.Host.Api
{
	/// <summary>
	/// Access to calculated parameter values.
	/// </summary>
	public class ParameterValuesController : ApiControllerBase, IParameterValues
	{
		private readonly IParameterValues parameterValues;

		/// <param name="parameterValues">
		/// Calculated parameter values.
		/// </param>
		public ParameterValuesController(IParameterValues parameterValues) => this.parameterValues = parameterValues;

		/// <summary>
		/// Get parameter values calculated during most recent job iteration. 
		/// </summary>
		[HttpGet]
		public IAsyncEnumerable<IValueView> MostRecent() => parameterValues.MostRecent();
	}
}
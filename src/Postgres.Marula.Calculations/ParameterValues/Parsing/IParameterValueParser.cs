using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;

namespace Postgres.Marula.Calculations.ParameterValues.Parsing
{
	/// <summary>
	/// Database parameter value parser.
	/// </summary>
	internal interface IParameterValueParser
	{
		/// <summary>
		/// Parse raw parameter representation into strongly typed value instance. 
		/// </summary>
		IParameterValue Parse(IParameterLink parameterLink, RawParameterValue rawParameterValue);
	}
}
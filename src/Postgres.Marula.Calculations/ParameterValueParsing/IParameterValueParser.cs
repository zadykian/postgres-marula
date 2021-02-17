using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValueParsing
{
	/// <summary>
	/// Database parameter value parser.
	/// </summary>
	internal interface IParameterValueParser
	{
		/// <summary>
		/// Parse raw parameter representation into strongly typed value instance. 
		/// </summary>
		IParameterValue Parse(NonEmptyString parameterName, RawParameterValue rawParameterValue);
	}
}
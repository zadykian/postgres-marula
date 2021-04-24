using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Memory database server parameter.
	/// </summary>
	internal abstract class MemoryParameterBase : ParameterBase<MemoryParameterValue, Memory>
	{
	}
}
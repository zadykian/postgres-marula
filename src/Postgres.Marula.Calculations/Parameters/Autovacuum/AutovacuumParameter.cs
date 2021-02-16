// ReSharper disable UnusedType.Global

using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// Starts the autovacuum subprocess.
	/// </summary>
	internal class AutovacuumParameter : ParameterBase
	{
		/// <inheritdoc />
		public override NonEmptyString Name => "autovacuum";

		/// <inheritdoc />
		public override IParameterValue Calculate() => new BooleanParameterValue(this.GetLink(), value: true);
	}
}
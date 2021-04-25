using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// Controls whether the server should run the autovacuum launcher daemon.
	/// </summary>
	internal class Autovacuum : BooleanParameterBase
	{
		/// <inheritdoc />
		public override NonEmptyString Name => "autovacuum";

		/// <inheritdoc />
		protected override bool CalculateValue() => true;
	}
}
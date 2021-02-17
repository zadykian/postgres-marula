using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// Controls whether the server should run the autovacuum launcher daemon.
	/// </summary>
	internal class AutovacuumParameter : ParameterBase
	{
		public AutovacuumParameter(IDatabaseServer databaseServer) : base(databaseServer)
		{
		}

		/// <inheritdoc />
		public override NonEmptyString Name => "autovacuum";

		/// <inheritdoc />
		public override IParameterValue Calculate() => new BooleanParameterValue(this.GetLink(), value: true);
	}
}
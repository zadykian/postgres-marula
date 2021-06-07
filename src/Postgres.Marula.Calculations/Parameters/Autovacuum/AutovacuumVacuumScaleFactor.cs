using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_vacuum_scale_factor]
	/// Specifies a fraction of the table size to add to
	/// <see cref="AutovacuumVacuumThreshold"/> when deciding whether to trigger a vacuum.
	/// </summary>
	internal class AutovacuumVacuumScaleFactor : FractionParameterBase
	{
		public AutovacuumVacuumScaleFactor(
			ILogger<AutovacuumVacuumScaleFactor> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override async ValueTask<Fraction> CalculateValueAsync() => throw new System.NotImplementedException();
	}
}
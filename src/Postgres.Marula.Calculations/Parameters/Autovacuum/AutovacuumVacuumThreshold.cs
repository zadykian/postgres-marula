using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using TuplesCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_vacuum_threshold]
	/// Specifies the minimum number of updated or deleted tuples
	/// needed to trigger a vacuum in any one table.
	/// </summary>
	internal class AutovacuumVacuumThreshold : IntegerParameterBase
	{
		public AutovacuumVacuumThreshold(
			ILogger<AutovacuumVacuumThreshold> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override async ValueTask<TuplesCount> CalculateValueAsync() => throw new System.NotImplementedException();
	}
}
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	/// <summary>
	/// [max_wal_size]
	/// Maximum size to let the WAL grow during automatic checkpoints.
	/// This is a soft limit - WAL size can exceed max_wal_size under special circumstances.
	/// </summary>
	internal class MaxWalSize : MemoryParameterBase
	{
		/// <inheritdoc />
		protected override Memory CalculateValue() => throw new System.NotImplementedException();
	}
}
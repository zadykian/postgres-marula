using System;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	/// <summary>
	/// Maximum time between automatic WAL checkpoints.
	/// </summary>
	internal class CheckpointTimeout : TimeSpanParameterBase
	{
		/// <inheritdoc />
		public override NonEmptyString Name => "checkpoint_timeout";

		/// <inheritdoc />
		protected override PositiveTimeSpan CalculateValue() => TimeSpan.FromMinutes(30);
	}
}
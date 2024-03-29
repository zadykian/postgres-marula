using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	/// <summary>
	/// [checkpoint_timeout]
	/// Maximum time between automatic WAL checkpoints.
	/// </summary>
	internal class CheckpointTimeout : TimeSpanParameterBase
	{
		public CheckpointTimeout(ILogger<CheckpointTimeout> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override ValueTask<PositiveTimeSpan> CalculateValueAsync()
			=> TimeSpan
				.FromMinutes(30)
				.To(timespan => ValueTask.FromResult((PositiveTimeSpan) timespan));
	}
}
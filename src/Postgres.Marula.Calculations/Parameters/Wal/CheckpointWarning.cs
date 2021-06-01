using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Wal
{
	/// <summary>
	/// [checkpoint_warning]
	/// Write a message to the server log if checkpoints caused by the filling
	/// of WAL segment files happen closer together than this amount of time.
	/// </summary>
	internal class CheckpointWarning : TimeSpanParameterBase
	{
		private readonly IPgSettings pgSettings;

		public CheckpointWarning(
			IPgSettings pgSettings,
			ILogger<TimeSpanParameterBase> logger) : base(logger)
			=> this.pgSettings = pgSettings;

		/// <inheritdoc />
		protected override async ValueTask<PositiveTimeSpan> CalculateValueAsync()
		{
			var checkpointTimeout = await pgSettings.ReadAsync<PositiveTimeSpan>("checkpoint_timeout");
			return checkpointTimeout * 0.8;
		}
	}
}
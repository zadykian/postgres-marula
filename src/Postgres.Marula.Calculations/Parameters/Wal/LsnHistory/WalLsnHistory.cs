using System.Collections.Generic;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Parameters.Wal.LsnHistory
{
	/// <inheritdoc />
	internal class WalLsnHistory : IWalLsnHistory
	{
		private readonly ISystemStorage systemStorage;
		private readonly ICalculationsConfiguration configuration;

		public WalLsnHistory(ISystemStorage systemStorage, ICalculationsConfiguration configuration)
		{
			this.systemStorage = systemStorage;
			this.configuration = configuration;
		}

		/// <inheritdoc />
		IAsyncEnumerable<LsnHistoryEntry> IWalLsnHistory.ReadAsync()
			=> configuration
				.Wal()
				.MovingAverageWindow()
				.To(systemStorage.GetLsnHistoryAsync);
	}
}
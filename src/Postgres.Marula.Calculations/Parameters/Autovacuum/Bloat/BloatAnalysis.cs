using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat
{
	/// <inheritdoc />
	internal class BloatAnalysis : IBloatAnalysis
	{
		private readonly ISystemStorage systemStorage;
		private readonly ICalculationsConfiguration configuration;

		public BloatAnalysis(
			ISystemStorage systemStorage,
			ICalculationsConfiguration configuration)
		{
			this.systemStorage = systemStorage;
			this.configuration = configuration;
		}

		/// <inheritdoc />
		async Task<BloatCoefficients> IBloatAnalysis.ExecuteAsync()
		{
			var averageBloatHistory = await LoadHistory().ToArrayAsync();

			var leftTimeBound = averageBloatHistory
				.First()
				.LogTimestamp;

			// abscissa values are shift to left, so selection values starts from zero.
			var abscissaValues = averageBloatHistory
				.Select(entry => (double) entry.LogTimestamp.Ticks)
				.Select(ticks => ticks - leftTimeBound.Ticks)
				.ToArray();

			var ordinateValues = averageBloatHistory
				.Select(entry => (double) (decimal) entry.AverageBloatFraction)
				.ToArray();

			// approximate bloat fraction selection to linear function.
			var (linearMember, freeMember) = Fit.Line(abscissaValues, ordinateValues);
			return new BloatCoefficients(linearMember, freeMember);
		}

		/// <summary>
		/// Load history entries from system storage. 
		/// </summary>
		private IAsyncEnumerable<BloatFractionHistoryEntry> LoadHistory()
			=> configuration
				.Autovacuum()
				.MovingAverageWindow()
				.To(systemStorage.GetBloatFractionHistory);
	}
}
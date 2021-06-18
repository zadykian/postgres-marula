using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Exceptions;
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

			// At least two values are required to build approximated linear function.
			if (averageBloatHistory.Length < 2) throw Error.NoBloatHistory();

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
			var (intercept, slope) = Fit.Line(abscissaValues, ordinateValues);
			return new BloatCoefficients(intercept, slope);
		}

		/// <summary>
		/// Load history entries from system storage. 
		/// </summary>
		private IAsyncEnumerable<BloatFractionHistoryEntry> LoadHistory()
			=> configuration
				.Autovacuum()
				.RollingWindow()
				.To(systemStorage.GetBloatFractionHistory);
	}
}
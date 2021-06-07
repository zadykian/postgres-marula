using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class FakeSystemStorage : ISystemStorage
	{
		/// <inheritdoc />
		Task ISystemStorage.SaveParameterValuesAsync(IReadOnlyCollection<ParameterValueWithStatus> parameterValues)
			=> Task.CompletedTask;

		/// <inheritdoc />
		Task ISystemStorage.SaveLogSeqNumberAsync(LogSeqNumber logSeqNumber)
			=> Task.CompletedTask;

		/// <inheritdoc />
		async IAsyncEnumerable<LsnHistoryEntry> ISystemStorage.GetLsnHistoryAsync(PositiveTimeSpan window)
		{
			await Task.CompletedTask;
			yield return new(new(2021, 05, 1, 9, 0, 0), new("32/A0000100"));
			yield return new(new(2021, 05, 1, 9, 1, 0), new("32/A0001000"));
			yield return new(new(2021, 05, 1, 9, 2, 0), new("32/A0010000"));
			yield return new(new(2021, 05, 1, 9, 3, 0), new("32/A0100000"));
		}

		/// <inheritdoc />
		Task ISystemStorage.SaveBloatFractionAsync(Fraction averageBloatFraction)
			=> Task.CompletedTask;

		/// <inheritdoc />
		async IAsyncEnumerable<BloatFractionHistoryEntry> ISystemStorage.GetBloatFractionHistory(PositiveTimeSpan window)
		{
			await Task.CompletedTask;
			yield return new(new(2021, 05, 1, 09, 0, 0), 0.4M);
			yield return new(new(2021, 05, 1, 12, 0, 0), 0.5M);
			yield return new(new(2021, 05, 1, 15, 0, 0), 0.6M);
			yield return new(new(2021, 05, 1, 18, 0, 0), 0.5M);
		}
	}
}
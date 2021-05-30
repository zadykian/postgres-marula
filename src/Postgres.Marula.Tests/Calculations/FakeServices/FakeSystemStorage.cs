using System.Collections.Generic;
using System.Linq;
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
		IAsyncEnumerable<LsnHistoryEntry> ISystemStorage.GetLsnHistory(PositiveTimeSpan window)
			=> AsyncEnumerable.Empty<LsnHistoryEntry>();
	}
}
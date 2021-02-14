using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Values.Base;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class FakeSystemStorage : ISystemStorage
	{
		/// <inheritdoc />
		public Task SaveParameterValuesAsync(IReadOnlyCollection<ParameterValueWithStatus> parameterValues) => Task.CompletedTask;
	}
}
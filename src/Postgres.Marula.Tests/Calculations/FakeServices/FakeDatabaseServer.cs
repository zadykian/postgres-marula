using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class FakeDatabaseServer : IDatabaseServer
	{
		Task IDatabaseServer.ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues)
			=> Task.CompletedTask;

		/// <inheritdoc />
		Task<IParameterValue> IDatabaseServer.GetParameterValueAsync(NonEmptyString parameterName)
			=> throw new System.NotSupportedException();
	}
}
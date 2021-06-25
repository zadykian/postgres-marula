using System.Collections.Generic;
using System.Linq;
using Postgres.Marula.Calculations.Parameters.Autovacuum;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.LockManagement;
using Postgres.Marula.Calculations.Parameters.ResourceUsage.Memory;
using Postgres.Marula.Calculations.Parameters.Wal;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class FakeParameterValues : IParameterValues
	{
		/// <inheritdoc />
		IAsyncEnumerable<IValueView> IParameterValues.MostRecentAsync()
			=> new ValueView[]
				{
					new(new Link<Autovacuum>(),                 "true"),
					new(new Link<CheckpointCompletionTarget>(), "0.8" ),
					new(new Link<MaxLocksPerTransaction>(),     "96"  ),
					new(new Link<SharedBuffers>(),              "1GB" ),
					new(new Link<AutovacuumVacuumCostDelay>(),  "2ms" )
				}
				.ToAsyncEnumerable();	
	}
}
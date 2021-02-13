using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="ISystemStorage" />
	internal class DefaultSystemStorage : DatabaseInteractionComponent, ISystemStorage
	{
		public DefaultSystemStorage(IPreparedDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{
		}

		/// <inheritdoc />
		Task ISystemStorage.SaveParameterValuesAsync(IEnumerable<ParameterValueWithStatus> parameterValues)
			=> throw new System.NotImplementedException();
	}
}
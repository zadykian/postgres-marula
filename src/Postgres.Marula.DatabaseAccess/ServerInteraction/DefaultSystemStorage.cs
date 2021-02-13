using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction
{
	/// <inheritdoc cref="ISystemStorage" />
	internal class DefaultSystemStorage : DatabaseInteractionComponent, ISystemStorage
	{
		private readonly INamingConventions namingConventions;

		public DefaultSystemStorage(
			IPreparedDbConnectionFactory dbConnectionFactory,
			INamingConventions namingConventions) : base(dbConnectionFactory)
			=> this.namingConventions = namingConventions;

		/// <inheritdoc />
		Task ISystemStorage.SaveParameterValuesAsync(IEnumerable<ParameterValueWithStatus> parameterValues)
		{
			var commandText = $@"
				insert into {namingConventions.SystemSchemaName}.{}";
		}

		private static NonEmptyString ToValuesString(ParameterValueWithStatus parameterValue)
		{
			
		}
	}
}
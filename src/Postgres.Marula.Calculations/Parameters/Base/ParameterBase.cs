using System.Collections.Concurrent;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <inheritdoc />
	internal abstract class ParameterBase : IParameter
	{
		private static readonly ConcurrentDictionary<NonEmptyString, ParameterContext> contextCache = new();
		private readonly IDatabaseServer databaseServer;

		protected ParameterBase(IDatabaseServer databaseServer) => this.databaseServer = databaseServer;

		/// <inheritdoc />
		public abstract NonEmptyString Name { get; }

		/// <inheritdoc />
		public abstract IParameterValue Calculate();

		/// <inheritdoc />
		async Task<ParameterContext> IParameter.GetContextAsync()
		{
			if (!contextCache.TryGetValue(Name, out var parameterContext))
			{
				parameterContext = await databaseServer.GetParameterContextAsync(Name);
				contextCache[Name] = parameterContext;
			}

			return parameterContext;
		}
	}
}
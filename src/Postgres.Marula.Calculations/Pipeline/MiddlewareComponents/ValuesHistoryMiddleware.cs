using System;
using System.Threading.Tasks;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.ExternalDependencies;

namespace Postgres.Marula.Calculations.Pipeline.MiddlewareComponents
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for values history maintenance.
	/// </summary>
	internal class ValuesHistoryMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		private readonly ISystemStorage systemStorage;

		public ValuesHistoryMiddleware(ISystemStorage systemStorage) => this.systemStorage = systemStorage;

		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next) => throw new NotImplementedException();
	}
}
using System;
using System.Threading.Tasks;
using PipelineNet.Middleware;

namespace Postgres.Marula.Calculations.Pipeline.Components
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for values history maintenance.
	/// </summary>
	internal class ValuesHistoryMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next) => throw new NotImplementedException();
	}
}
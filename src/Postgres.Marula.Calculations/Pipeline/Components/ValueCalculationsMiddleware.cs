using System;
using System.Threading.Tasks;
using PipelineNet.Middleware;

namespace Postgres.Marula.Calculations.Pipeline.Components
{
	/// <summary>
	/// Pipeline component which is responsible
	/// for parameter values retrieving.
	/// </summary>
	internal class ValueCalculationsMiddleware : IAsyncMiddleware<ParametersManagementContext>
	{
		/// <inheritdoc />
		Task IAsyncMiddleware<ParametersManagementContext>.Run(
			ParametersManagementContext context,
			Func<ParametersManagementContext, Task> next) => throw new NotImplementedException();
	}
}
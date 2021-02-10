using Microsoft.Extensions.DependencyInjection;
using PipelineNet.Pipelines;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.Factory
{
	/// <inheritdoc />
	internal class DefaultPipelineFactory : IPipelineFactory
	{
		/// <inheritdoc />
		IParametersPipeline IPipelineFactory.CreateWithScope(IServiceScope pipelineScope)
		{
			// todo: add middleware components
			var asyncPipeline = pipelineScope
			 	.To(scope => new ServiceScopeMiddlewareResolver(scope))
				.To(resolver => new AsyncPipeline<ParametersManagementContext>(resolver));

			var pipelineContext = pipelineScope.ServiceProvider.GetRequiredService<ParametersManagementContext>();
			return new DefaultParametersPipeline(asyncPipeline, pipelineContext);
		}
	}
}
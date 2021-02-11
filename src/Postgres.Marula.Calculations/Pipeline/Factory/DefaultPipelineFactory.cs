using Microsoft.Extensions.DependencyInjection;
using PipelineNet.Pipelines;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Calculations.Pipeline.Factory
{
	/// <inheritdoc />
	internal class DefaultPipelineFactory : IPipelineFactory
	{
		/// <inheritdoc />
		IParametersPipeline IPipelineFactory.CreateWithScope(IServiceScope pipelineScope)
		{
			var asyncPipeline = pipelineScope
			 	.To(scope => new ServiceScopeMiddlewareResolver(scope))
				.To(resolver => new AsyncPipeline<ParametersManagementContext>(resolver))
			    .Add<ValueCalculationsMiddleware>()
			    .Add<ParametersAdjustmentMiddleware>()
			    .Add<ValuesHistoryMiddleware>();

			var pipelineContext = pipelineScope.ServiceProvider.GetRequiredService<ParametersManagementContext>();
			return new DefaultParametersPipeline(asyncPipeline, pipelineContext);
		}
	}
}
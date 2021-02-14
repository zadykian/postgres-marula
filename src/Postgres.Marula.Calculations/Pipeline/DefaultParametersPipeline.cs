using System.Threading.Tasks;
using PipelineNet.Pipelines;

namespace Postgres.Marula.Calculations.Pipeline
{
	/// <inheritdoc />
	internal class DefaultParametersPipeline : IParametersPipeline
	{
		private readonly IAsyncPipeline<ParametersManagementContext> pipeline;
		private readonly ParametersManagementContext context;

		public DefaultParametersPipeline(
			IAsyncPipeline<ParametersManagementContext> pipeline,
			ParametersManagementContext context)
		{
			this.pipeline = pipeline;
			this.context = context;
		}

		/// <inheritdoc />
		Task IParametersPipeline.RunAsync() => pipeline.Execute(context);
	}
}
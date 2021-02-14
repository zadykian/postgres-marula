using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Calculations.Pipeline.Factory
{
	/// <summary>
	/// Parameters pipeline factory.
	/// </summary>
	internal interface IPipelineFactory
	{
		/// <summary>
		/// Create parameters management pipeline.
		/// </summary>
		/// <param name="pipelineScope">
		/// Scope of pipeline's single iteration.
		/// </param>
		IParametersPipeline CreateWithScope(IServiceScope pipelineScope);
	}
}
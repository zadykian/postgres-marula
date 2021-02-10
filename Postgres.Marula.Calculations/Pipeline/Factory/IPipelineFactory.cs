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
		IParametersPipeline Create();
	}
}
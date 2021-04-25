using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Jobs.Base;
using Postgres.Marula.Calculations.Pipeline.Factory;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Jobs
{
	/// <inheritdoc cref="IJob"/>
	/// <remarks>
	/// This implementation is responsible for general calculations process.
	/// </remarks>
	internal class GeneralCalculationsJob : JobBase
	{
		private readonly IPipelineFactory pipelineFactory;

		public GeneralCalculationsJob(
			ICalculationsConfiguration configuration,
			IPipelineFactory pipelineFactory,
			IServiceScopeFactory serviceScopeFactory,
			ILogger<GeneralCalculationsJob> logger) : base(configuration.RecalculationInterval(), serviceScopeFactory, logger)
			=> this.pipelineFactory = pipelineFactory;

		/// <inheritdoc />
		protected override NonEmptyString Description => "parameters calculation";

		/// <inheritdoc />
		protected override async ValueTask ExecuteAsync(IServiceScope serviceScope)
			=> await pipelineFactory
				.CreateWithScope(serviceScope)
				.RunAsync();
	}
}
using System.Threading.Tasks;
using NUnit.Framework;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.Tests.Calculations.Base;

namespace Postgres.Marula.Tests.Calculations.MiddlewareComponents
{
	/// <summary>
	/// <see cref="ValueCalculationsMiddleware"/> middleware tests.
	/// </summary>
	[Ignore("")]
	internal class ValueCalculationsMiddlewareTests : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run calculations middleware. 
		/// </summary>
		[Test]
		public async Task RunMiddlewareTest()
		{
			var middleware = (IAsyncMiddleware<ParametersManagementContext>) GetService<ValueCalculationsMiddleware>();
			var parametersManagementContext = GetService<ParametersManagementContext>();

			await middleware.Run(parametersManagementContext, async _ =>
			{
				await Task.CompletedTask;
				Assert.Pass();
			});
		}
	}
}
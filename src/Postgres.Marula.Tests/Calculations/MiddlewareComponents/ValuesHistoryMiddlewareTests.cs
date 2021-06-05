using System.Threading.Tasks;
using NUnit.Framework;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.Tests.Calculations.Base;

namespace Postgres.Marula.Tests.Calculations.MiddlewareComponents
{
	/// <summary>
	/// <see cref="ValuesHistoryMiddleware"/> middleware tests.
	/// </summary>
	internal class ValuesHistoryMiddlewareTests : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run values history middleware. 
		/// </summary>
		[Test]
		public async Task RunMiddlewareTest()
		{
			var parametersManagementContext = GetService<ParametersManagementContext>();
			var middleware = (IAsyncMiddleware<ParametersManagementContext>) GetService<ValuesHistoryMiddleware>();

			await middleware.Run(parametersManagementContext, async context =>
			{
				await Task.CompletedTask;
				Assert.AreSame(parametersManagementContext, context);
			});
		}
	}
}
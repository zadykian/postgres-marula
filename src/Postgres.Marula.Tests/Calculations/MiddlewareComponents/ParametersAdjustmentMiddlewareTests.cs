using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.Tests.Calculations.Base;

namespace Postgres.Marula.Tests.Calculations.MiddlewareComponents
{
	/// <summary>
	/// <see cref="ParametersAdjustmentMiddleware"/> middleware tests.
	/// </summary>
	internal class ParametersAdjustmentMiddlewareTests : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Run calculations middleware. 
		/// </summary>
		[Test]
		public async Task RunMiddlewareTest()
		{
			var parametersManagementContext = GetService<ParametersManagementContext>() with
			{
				CalculatedValues = GetService<IEnumerable<IParameter>>()
					.Select(parameter => parameter.Calculate())
					.ToImmutableArray()
			};

			var middleware = (IAsyncMiddleware<ParametersManagementContext>) GetService<ParametersAdjustmentMiddleware>();

			await middleware.Run(parametersManagementContext, async context =>
			{
				await Task.CompletedTask;
				Assert.AreSame(parametersManagementContext, context);
			});
		}
	}
}
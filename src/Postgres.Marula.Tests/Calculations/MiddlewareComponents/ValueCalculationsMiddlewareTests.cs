using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PipelineNet.Middleware;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Pipeline;
using Postgres.Marula.Calculations.Pipeline.MiddlewareComponents;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Tests.Calculations.Base;

namespace Postgres.Marula.Tests.Calculations.MiddlewareComponents
{
	/// <summary>
	/// <see cref="ValueCalculationsMiddleware"/> middleware tests.
	/// </summary>
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

			await middleware.Run(parametersManagementContext, async context =>
			{
				await Task.CompletedTask;
				Assert.IsTrue(context.Parameters.Any());
				Assert.IsTrue(context.CalculatedValues.Any());

				var parameterLinks = context
					.Parameters
					.Select(parameter => parameter.GetLink())
					.ToImmutableArray();

				context
					.CalculatedValues
					.Count(value => !parameterLinks.Contains(value.ParameterLink))
					.To(valuesWithoutParameterCount => Assert.AreEqual(expected: 0, valuesWithoutParameterCount));
			});
		}
	}
}
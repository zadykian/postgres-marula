using System;
using Microsoft.Extensions.DependencyInjection;
using PipelineNet.MiddlewareResolver;

namespace Postgres.Marula.Calculations.Pipeline.Factory
{
	/// <inheritdoc />
	internal class ServiceScopeMiddlewareResolver : IMiddlewareResolver
	{
		private readonly IServiceScope serviceScope;

		public ServiceScopeMiddlewareResolver(IServiceScope serviceScope)
			=> this.serviceScope = serviceScope;

		/// <inheritdoc />
		object IMiddlewareResolver.Resolve(Type type)
			=> serviceScope.ServiceProvider.GetRequiredService(type);
	}
}
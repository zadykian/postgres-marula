using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Agent
{
	/// <inheritdoc />
	internal class Startup : IStartup
	{
		/// <inheritdoc />
		IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
			=> services
				.AddControllers()
				.Services
				.AddSwaggerGen()
				.BuildServiceProvider();

		/// <inheritdoc />
		void IStartup.Configure(IApplicationBuilder builder)
			=> builder
				.UseRouting()
				.UseEndpoints(endpointBuilder => endpointBuilder.MapControllers())
				.UseSwagger()
				.UseSwaggerUI();
	}
}
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Agent
{
	internal class Startup
	{
		public void ConfigureServices(IServiceCollection services)
			=> services
				.AddControllers()
				.Services
				.AddSwaggerGen(options =>
				{
					var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					options.IncludeXmlComments(xmlPath);
				});

		public void Configure(IApplicationBuilder builder)
			=> builder
				.UseRouting()
				.UseEndpoints(endpointBuilder => endpointBuilder.MapControllers())
				.UseSwagger()
				.UseSwaggerUI();
	}
}
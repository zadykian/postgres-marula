using System;
using System.IO;
using System.Reflection;
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
				.AddSwaggerGen(options =>
				{
					var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					options.IncludeXmlComments(xmlPath);
				})
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
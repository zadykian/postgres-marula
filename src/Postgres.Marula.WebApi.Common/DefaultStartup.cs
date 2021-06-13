using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.WebApi.Common
{
	/// <summary>
	/// Default web application startup.
	/// </summary>
	internal class DefaultStartup
	{
		/// <summary>
		/// Configure application services. 
		/// </summary>
		public void ConfigureServices(IServiceCollection services)
			=> services
				.AddMvc()
				.To(AddForeignAssemblies)
				.AddControllersAsServices()
				.Services
				.AddSwaggerGen(options =>
				{
					var xmlFile = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					options.IncludeXmlComments(xmlPath);
				});

		/// <summary>
		/// Add to <paramref name="mvcBuilder"/> all assemblies containing
		/// controllers derived from <see cref="ControllerBase"/>.
		/// </summary>
		private static IMvcBuilder AddForeignAssemblies(IMvcBuilder mvcBuilder)
			=> AppDomain
				.CurrentDomain
				.GetAssemblies()
				.Where(assembly => assembly.GetTypes().Any(type => type.IsAssignableTo(typeof(ControllerBase))))
				.Aggregate(mvcBuilder, (builder, assembly) => builder.AddApplicationPart(assembly));

		/// <summary>
		/// Configure request processing pipeline. 
		/// </summary>
		public void Configure(IApplicationBuilder builder)
			=> builder
				.UseRouting()
				.UseEndpoints(endpointBuilder => endpointBuilder.MapControllers())
				.UseSwagger()
				.UseSwaggerUI();
	}
}
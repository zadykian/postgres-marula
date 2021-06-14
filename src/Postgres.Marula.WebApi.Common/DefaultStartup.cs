using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.WebApi.Common.JsonConverters;

namespace Postgres.Marula.WebApi.Common
{
	/// <summary>
	/// Default web application startup.
	/// </summary>
	internal class DefaultStartup
	{
		/// <summary>
		/// Name of current entry assembly.
		/// </summary>
		private static NonEmptyString EntryAssemblyName => Assembly.GetEntryAssembly()!.GetName().Name!;

		/// <summary>
		/// Configure application services. 
		/// </summary>
		public void ConfigureServices(IServiceCollection services)
			=> services
				.AddMvc()
				.To(AddForeignAssemblies)
				.AddControllersAsServices()
				.AddJsonOptions(options =>
				{
					var converters = options.JsonSerializerOptions.Converters;
					converters.Add(new JsonStringEnumConverter());
					converters.Add(new NonEmptyStringJsonConverter());
				})
				.Services
				.AddSwaggerGen(options =>
				{
					var xmlFile = $"{EntryAssemblyName}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					options.IncludeXmlComments(xmlPath);
					options.SwaggerDoc("v1", new OpenApiInfo
					{
						Version = "v1",
						Title = EntryAssemblyName,
						Description = $"{EntryAssemblyName}'s public API.",
					});
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
				.UseSwaggerUI(options =>
				{
					options.DocumentTitle = EntryAssemblyName;
					options.RoutePrefix = string.Empty;
					options.SwaggerEndpoint("/swagger/v1/swagger.json", EntryAssemblyName);
				});
	}
}
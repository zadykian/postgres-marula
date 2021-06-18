using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.JsonConverters;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Postgres.Marula.HttpApi.Common
{
	/// <summary>
	/// Default HTTP API application startup.
	/// </summary>
	internal class DefaultStartup
	{
		/// <summary>
		/// Name of current entry assembly.
		/// </summary>
		private static NonEmptyString EntryAssemblyName => Assembly.GetEntryAssembly()!.GetName().Name!;

		/// <summary>
		/// API version.
		/// </summary>
		private static NonEmptyString ApiVersion => "v1";

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
					options.JsonSerializerOptions.PropertyNamingPolicy = new LiteralNamingPolicy();
				})
				.Services
				.AddSwaggerGen(ConfigureSwagger);

		/// <summary>
		/// Perform Swagger configuration. 
		/// </summary>
		private static void ConfigureSwagger(SwaggerGenOptions options)
		{
			AppContext
				.BaseDirectory
				.To(dir => Path.Combine(dir, $"{EntryAssemblyName}.xml"))
				.To(path => options.IncludeXmlComments(path));

			options.SwaggerDoc(ApiVersion, new OpenApiInfo
			{
				Version = ApiVersion,
				Title = EntryAssemblyName,
				Description = $"{EntryAssemblyName}'s public API.",
			});
		}

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
					options.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", EntryAssemblyName);
				});

		/// <inheritdoc />
		private sealed class LiteralNamingPolicy : JsonNamingPolicy
		{
			/// <inheritdoc />
			public override string ConvertName(string name) => name;
		}
	}
}
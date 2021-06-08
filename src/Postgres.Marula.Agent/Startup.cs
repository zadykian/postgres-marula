using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Agent
{
	/// <inheritdoc />
	internal class Startup : IStartup
	{
		/// <inheritdoc />
		IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
			=> services
				.AddComponent<HwInfoAppComponent>()
				.BuildServiceProvider();

		/// <inheritdoc />
		void IStartup.Configure(IApplicationBuilder app)
		{
			// todo
		}
	}
}
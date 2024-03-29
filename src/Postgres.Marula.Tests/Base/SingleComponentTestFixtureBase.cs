using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.DatabaseAccess.Configuration;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.Tests.Base
{
	/// <summary>
	/// Base class for testing services from component <typeparamref name="TAppComponent"/>.
	/// </summary>
	[TestFixture]
	internal abstract class SingleComponentTestFixtureBase<TAppComponent>
		where TAppComponent : IAppComponent, new()
	{
		private readonly IServiceProvider serviceProvider;

		protected SingleComponentTestFixtureBase()
			=> serviceProvider = CreateServiceCollection().BuildServiceProvider();

		/// <summary>
		/// Create collection of services.
		/// </summary>
		private IServiceCollection CreateServiceCollection()
		{
			var serviceCollection = new ServiceCollection();
			new TAppComponent().RegisterServices(serviceCollection);
			ConfigureServices(serviceCollection);
			return serviceCollection;
		}

		/// <summary>
		/// Get registered implementation of service <typeparamref name="TService"/>.
		/// </summary>
		protected TService GetService<TService>()
			where TService : notnull
			=> serviceProvider.GetRequiredService<TService>();

		/// <summary>
		/// Perform additional services configuration. 
		/// </summary>
		protected virtual void ConfigureServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddLogging()
				.AddSingleton<IConfiguration>(_
					=> new ConfigurationBuilder()
						.AddJsonFile("marula-host-config.json")
						.AddJsonFile("marula-host-config.local.json", optional: true)
						.Build())
				.AddSingleton<ICalculationsConfiguration, CalculationsConfiguration>()
				.AddSingleton<IDatabaseAccessConfiguration, DatabaseAccessConfiguration>();
	}
}
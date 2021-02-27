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
	/// Base class for testing services from component <typeparamref name="TComponent"/>. 
	/// </summary>
	[TestFixture]
	internal abstract class SingleComponentTestFixtureBase<TComponent>
		where TComponent : IAppComponent, new()
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
			new TComponent().RegisterServices(serviceCollection);
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
						.AddJsonFile("appsettings.json")
						.AddJsonFile("appsettings.local.json", optional: true)
						.Build())
				.AddSingleton<ICalculationsConfiguration, DefaultCalculationsConfiguration>()
				.AddSingleton<IDatabaseAccessConfiguration, DefaultDatabaseAccessConfiguration>();
	}
}
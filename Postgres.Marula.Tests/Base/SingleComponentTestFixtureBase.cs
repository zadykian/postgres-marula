using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.Infrastructure.SolutionComponents;
using Microsoft.Extensions.Configuration;

namespace Postgres.Marula.Tests.Base
{
	/// <summary>
	/// Base class for testing services from component <typeparamref name="TSolutionComponent"/>. 
	/// </summary>
	[TestFixture]
	internal abstract class SingleComponentTestFixtureBase<TSolutionComponent>
		where TSolutionComponent : ISolutionComponent, new()
	{
		private IServiceProvider serviceProvider;

		[OneTimeSetUp]
		public virtual void OneTimeSetUp()
		{
			var serviceCollection = new ServiceCollection();
			new TSolutionComponent().RegisterServices(serviceCollection);
			ConfigureServices(serviceCollection);
			serviceProvider = serviceCollection.BuildServiceProvider();
		}

		/// <summary>
		/// Get registered implementation of service <typeparamref name="TService"/>. 
		/// </summary>
		protected TService GetService<TService>() => serviceProvider.GetRequiredService<TService>();

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
						.Build());
	}
}
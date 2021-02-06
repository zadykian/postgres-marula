using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.Infrastructure.SolutionComponents;

namespace Postgres.Marula.Tests.Base
{
	/// <summary>
	/// Base class for testing of single <typeparamref name="TService"/>
	/// from component <typeparamref name="TSolutionComponent"/>. 
	/// </summary>
	[TestFixture]
	internal abstract class SingleServiceTestBase<TService, TSolutionComponent>
		where TSolutionComponent : ISolutionComponent, new()
	{
		/// <summary>
		/// Service under test.
		/// </summary>
		protected TService ServiceToTest { get; private set; }

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var serviceCollection = new ServiceCollection();
			new TSolutionComponent().RegisterServices(serviceCollection);
			ConfigureServices(serviceCollection);

			ServiceToTest = serviceCollection
				.BuildServiceProvider()
				.GetRequiredService<TService>();
		}

		/// <summary>
		/// Perform additional services configuration. 
		/// </summary>
		protected virtual void ConfigureServices(IServiceCollection serviceCollection)
		{
		}

		[OneTimeTearDown]
		public void OneTimeTearDown() => (ServiceToTest as IDisposable)?.Dispose();
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IServiceCollection"/> type.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Add all implementations of interface <typeparamref name="TInterface"/>
		/// to collection <paramref name="serviceCollection"/>
		/// with lifetime <paramref name="servicesLifetime"/>. 
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="TInterface"/> is not interface.
		/// </exception>
		public static IServiceCollection AddBasedOn<TInterface>(
			this IServiceCollection serviceCollection,
			ServiceLifetime servicesLifetime = ServiceLifetime.Singleton)
		{
			if (!typeof(TInterface).IsInterface)
			{
				throw new ArgumentException($"Type '{nameof(TInterface)}' must be interface.", nameof(TInterface));
			}

			static bool TypePredicate(Type type)
				=> type
					.GetInterfaces()
					.Contains(typeof(TInterface)) && !type.IsAbstract;

			AppDomain
				.CurrentDomain
				.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(TypePredicate)
				.Select(type => new ServiceDescriptor(typeof(TInterface), type, servicesLifetime))
				.ForEach(serviceCollection.Add);

			return serviceCollection;
		}

		/// <summary>
		/// Add all registered services with service type <typeparamref name="TExisting"/>
		/// as services of type <typeparamref name="TNew"/>.
		/// </summary>
		public static IServiceCollection Forward<TExisting, TNew>(this IServiceCollection serviceCollection)
			where TExisting : class, TNew
			where TNew : notnull
			=> serviceCollection
				.AddTransient(provider
					=> provider
						.GetRequiredService<IEnumerable<TExisting>>()
						.Cast<TNew>())
				.AddTransient(provider
					=> provider
						.GetRequiredService<TExisting>());

		/// <summary>
		/// Add all services from component <typeparamref name="TAppComponent"/>
		/// to collection <paramref name="serviceCollection"/>. 
		/// </summary>
		public static IServiceCollection AddComponent<TAppComponent>(this IServiceCollection serviceCollection)
			where TAppComponent : IAppComponent, new()
		{
			new TAppComponent().RegisterServices(serviceCollection);
			return serviceCollection;
		}
	}
}
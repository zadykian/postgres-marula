using System;
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
			where TInterface : class
		{
			if (!typeof(TInterface).IsInterface)
			{
				throw new ArgumentException($"Type '{typeof(TInterface).Name}' must be interface.", typeof(TInterface).Name);
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
		/// Add service <typeparamref name="TForwarding"/> as implementation of <typeparamref name="TForwardTo"/>. 
		/// </summary>
		public static IServiceCollection Forward<TForwarding, TForwardTo>(this IServiceCollection serviceCollection)
			where TForwarding : class, TForwardTo
			where TForwardTo : class
			=> serviceCollection
				.AddTransient<TForwardTo>(provider => provider.GetRequiredService<TForwarding>());

		/// <summary>
		/// Add all services from component <typeparamref name="TAppComponent"/>
		/// to collection <paramref name="serviceCollection"/>. 
		/// </summary>
		public static IServiceCollection AddComponent<TAppComponent>(this IServiceCollection serviceCollection)
			where TAppComponent : IAppComponent, new()
			=> new TAppComponent().RegisterServices(serviceCollection);
	}
}
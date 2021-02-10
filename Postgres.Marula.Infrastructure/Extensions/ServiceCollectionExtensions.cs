using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IServiceCollection"/> type.
	/// </summary>
	internal static class ServiceCollectionExtensions
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
	}
}
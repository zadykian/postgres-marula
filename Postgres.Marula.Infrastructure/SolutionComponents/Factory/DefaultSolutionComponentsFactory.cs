using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.SolutionComponents.Factory
{
	/// <inheritdoc />
	public class DefaultSolutionComponentsFactory : ISolutionComponentsFactory
	{
		/// <inheritdoc />
		IEnumerable<ISolutionComponent> ISolutionComponentsFactory.CreateAll()
			=> GetAssembliesToGetComponentsFrom()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(ComponentTypePredicate)
				.Select(Activator.CreateInstance)
				.Cast<ISolutionComponent>()
				.ToImmutableArray();

		/// <summary>
		/// Get assemblies to access solution components.
		/// </summary>
		private static IEnumerable<Assembly> GetAssembliesToGetComponentsFrom()
			=> AppDomain
				.CurrentDomain
				.BaseDirectory
				.To(directoryPath => Directory.GetFiles(directoryPath, "Postgres.Marula.*.dll"))
				.Select(AssemblyName.GetAssemblyName)
				.Select(Assembly.Load);

		/// <summary>
		/// Predicate function for solution component types filtering.
		/// </summary>
		private static bool ComponentTypePredicate(Type type)
			=> type
				.GetInterfaces()
				.Contains(typeof(ISolutionComponent)) && !type.IsAbstract;
	}
}
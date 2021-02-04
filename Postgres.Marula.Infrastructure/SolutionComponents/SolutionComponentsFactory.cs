using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Postgres.Marula.Infrastructure.SolutionComponents
{
	/// <summary>
	/// Factory of solution components.
	/// </summary>
	public static class SolutionComponentsFactory
	{
		/// <summary>
		/// Get all components in solution.
		/// </summary>
		public static IEnumerable<ISolutionComponent> CreateAll()
			=> AppDomain
				.CurrentDomain
				.GetAssemblies()
				.Select(assembly => assembly.GetTypes())
				.SelectMany(types => types)
				.Where(type =>
					!type.IsAbstract
					&& type
						.GetInterfaces()
						.Contains(typeof(ISolutionComponent)))
				.Select(Activator.CreateInstance)
				.Cast<ISolutionComponent>()
				.ToImmutableArray();
	}
}
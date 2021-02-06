using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Postgres.Marula.Infrastructure.SolutionComponents.Factory
{
	/// <inheritdoc />
	public class SolutionComponentsFactory : ISolutionComponentsFactory
	{
		/// <inheritdoc />
		IEnumerable<ISolutionComponent> ISolutionComponentsFactory.CreateAll()
			=> AppDomain
				.CurrentDomain
				.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(ComponentTypePredicate)
				.Select(Activator.CreateInstance)
				.Cast<ISolutionComponent>()
				.ToImmutableArray();

		/// <summary>
		/// Predicate function for solution component types filtering.
		/// </summary>
		private static bool ComponentTypePredicate(Type type)
			=> !type.IsAbstract
			   && type
				   .GetInterfaces()
				   .Contains(typeof(ISolutionComponent));
	}
}
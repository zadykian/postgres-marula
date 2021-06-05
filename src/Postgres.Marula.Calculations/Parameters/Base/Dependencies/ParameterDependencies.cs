using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Postgres.Marula.Calculations.Parameters.Base.Dependencies
{
	/// <inheritdoc />
	internal class ParameterDependencies : IParameterDependencies
	{
		private readonly ICollection<Type> dependencyTypes = new List<Type>();

		private ParameterDependencies()
		{
		}

		/// <summary>
		/// Empty parameter dependencies.
		/// </summary>
		public static IParameterDependencies Empty => new ParameterDependencies();

		/// <inheritdoc />
		IParameterDependencies IParameterDependencies.DependsOn<TParameter>()
		{
			if (dependencyTypes.Contains(typeof(TParameter)))
			{
				throw new ArgumentException($"Parameter '{typeof(TParameter).Name}' already set as dependency.", typeof(TParameter).Name);
			}

			dependencyTypes.Add(typeof(TParameter));
			return this;
		}

		/// <inheritdoc />
		IReadOnlyCollection<Type> IParameterDependencies.All() => dependencyTypes.ToImmutableArray();
	}
}
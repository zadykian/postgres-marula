using System;
using System.Linq;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Link to database server parameter.
	/// </summary>
	public interface IParameterLink
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		NonEmptyString Name { get; }
	}

	/// <inheritdoc cref="IParameterLink"/>
	public sealed record ParameterLink(NonEmptyString Name) : IParameterLink
	{
		public ParameterLink(Type parameterType) : this(NameByType(parameterType))
		{
		}

		/// <summary>
		/// Get parameter name by its' type. 
		/// </summary>
		private static NonEmptyString NameByType(Type parameterType)
		{
			if (parameterType.IsAbstract)
			{
				throw new ArgumentException(
					$"Type '{parameterType}' must be non-abstract.", nameof(parameterType));
			}

			if (!parameterType
				.GetInterfaces()
				.Contains(typeof(IParameter)))
			{
				throw new ArgumentException(
					$"Type '{parameterType}' must implement {nameof(IParameter)} interface.", nameof(parameterType));
			}

			return parameterType.Name.ToSnakeCase();
		}
	}
}
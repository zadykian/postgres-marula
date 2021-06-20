using System;
using System.Linq;
using System.Text.Json;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.JsonSerialization;
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
	public record ParameterLink(NonEmptyString Name) : IParameterLink
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

	/// <inheritdoc cref="IParameterLink"/>	
	public record Link<TParameter> : ParameterLink
		where TParameter : IParameter
	{
		public Link() : base(typeof(TParameter))
		{
		}
	}

	// ReSharper disable once UnusedType.Global
	/// <inheritdoc />
	internal class ParameterLinkConverter : CustomJsonConverter<IParameterLink>
	{
		/// <inheritdoc />
		public override IParameterLink Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options) => new ParameterLink(reader.GetString()!);

		/// <inheritdoc />
		public override void Write(
			Utf8JsonWriter writer,
			IParameterLink link,
			JsonSerializerOptions options) => writer.WriteStringValue(link.Name);
	}
}
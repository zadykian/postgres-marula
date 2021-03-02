using System;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.Conventions
{
	/// <summary>
	/// Database objects' naming conventions.
	/// </summary>
	internal interface INamingConventions
	{
		/// <summary>
		/// Name of database schema that contains all system tables, types and so on.
		/// </summary>
		[ScriptPlaceholder("SYSTEM_SCHEMA_NAME_TO_REPLACE")]
		DatabaseObjectName SystemSchemaName { get; }

		/// <summary>
		/// Name of calculated parameter values table.
		/// </summary>
		[ScriptPlaceholder("VALUES_HISTORY_TABLE_NAME_TO_REPLACE")]
		DatabaseObjectName ValuesHistoryTableName { get; }

		/// <summary>
		/// Name of calculated parameters dictionary table.
		/// </summary>
		[ScriptPlaceholder("PARAMETERS_TABLE_NAME_TO_REPLACE")]
		DatabaseObjectName ParametersTableName { get; }

		/// <summary>
		/// Name of calculation status enumeration type.
		/// </summary>
		[ScriptPlaceholder("STATUS_ENUM_NAME_TO_REPLACE")]
		DatabaseObjectName CalculationStatusEnumName { get; }

		/// <summary>
		/// Name of parameter unit enumeration type.
		/// </summary>
		[ScriptPlaceholder("UNIT_ENUM_NAME_TO_REPLACE")]
		DatabaseObjectName ParameterUnitEnumName { get; }
	}

	/// <summary>
	/// Attribute to configure placeholder text for <see cref="INamingConventions"/> property.
	/// It must be applied to all <see cref="INamingConventions"/> properties.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	internal class ScriptPlaceholderAttribute : Attribute
	{
		public ScriptPlaceholderAttribute(string placeholder) => Placeholder = placeholder;

		/// <summary>
		/// Placeholder in SQL script.
		/// </summary>
		public NonEmptyString Placeholder { get; }
	}
}
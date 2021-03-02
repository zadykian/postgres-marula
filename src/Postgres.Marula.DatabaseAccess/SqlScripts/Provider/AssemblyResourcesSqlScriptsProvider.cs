using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.SqlScripts.Provider
{
	/// <inheritdoc />
	internal class AssemblyResourcesSqlScriptsProvider : ISqlScriptsProvider
	{
		private readonly INamingConventions namingConventions;

		public AssemblyResourcesSqlScriptsProvider(INamingConventions namingConventions)
			=> this.namingConventions = namingConventions;

		/// <inheritdoc />
		IEnumerable<SqlScript> ISqlScriptsProvider.GetAllOrderedByExecution()
			=> Assembly
				.GetExecutingAssembly()
				.GetManifestResourceNames()
				.Where(resourceName => Regex.IsMatch(resourceName, @".+\.sql$"))
				.Select(resourceName =>
					GetSqlResourceFullContentByName(resourceName)
						.To(resourceContent => (
							Name:    resourceName,
							Content: resourceContent,
							Order:   GetScriptExecutionOrder(resourceContent)
						)))
				.OrderBy(tuple => tuple.Order)
				.Select(tuple => new SqlScript(tuple.Name, tuple.Content))
				.ToImmutableArray();

		/// <summary>
		/// Load resource with name <paramref name="resourceName"/> from current assembly. 
		/// </summary>
		private NonEmptyString GetSqlResourceFullContentByName(NonEmptyString resourceName)
			=> typeof(INamingConventions)
				.GetProperties()
				.Select(propertyInfo => (
					ScriptPlaceholder: propertyInfo.GetCustomAttribute<ScriptPlaceholderAttribute>()!.Placeholder,
					PropertyValue:     (DatabaseObjectName) propertyInfo.GetValue(namingConventions)!))
				.Aggregate(
					GetResourceContent(resourceName),
					(content, tuple) => content.Replace(tuple.ScriptPlaceholder, tuple.PropertyValue));

		/// <summary>
		/// Get full content of assembly resource named <paramref name="resourceName"/>. 
		/// </summary>
		private static NonEmptyString GetResourceContent(NonEmptyString resourceName)
		{
			using var streamReader = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(resourceName)
				.ThrowIfNull($"Failed to load resource '{resourceName}' from assembly.")
				.To(resourceStream => new StreamReader(resourceStream));

			return streamReader.ReadToEnd();
		}

		/// <summary>
		/// Parse resource content and extract SQL script execution order. 
		/// </summary>
		private static ushort GetScriptExecutionOrder(NonEmptyString resourceContent)
		{
			var executionOrderLine = resourceContent
				.ToString()
				.Split(Environment.NewLine)
				.First();

			const string orderValuePrefix = "-- execution-order: ";
			var firstLinePattern = $"^{orderValuePrefix}[0-9]+$";

			return Regex.IsMatch(executionOrderLine, firstLinePattern)
				? executionOrderLine
					.Replace(orderValuePrefix, string.Empty)
					.To(ushort.Parse)
				: throw new ApplicationException($"SQL script must contain '{firstLinePattern}' as first line.");
		}
	}
}
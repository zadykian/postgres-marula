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
							Name: resourceName,
							Content: resourceContent,
							ExecutionOrder: GetScriptExecutionOrder(resourceContent)
						)))
				.OrderBy(tuple => tuple.ExecutionOrder)
				.Select(tuple => new SqlScript(tuple.Name, tuple.Content))
				.ToImmutableArray();

		/// <summary>
		/// Load resource with name <paramref name="resourceName"/> from current assembly. 
		/// </summary>
		private NonEmptyString GetSqlResourceFullContentByName(NonEmptyString resourceName)
		{
			using var streamReader = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(resourceName)
				.ThrowIfNull($"Failed to load resource '{resourceName}' from assembly.")
				.To(resourceStream => new StreamReader(resourceStream));

			return streamReader
				.ReadToEnd()
				.Replace("SYSTEM_SCHEMA_NAME_TO_REPLACE", namingConventions.SystemSchemaName)
				.Replace("VALUES_HISTORY_TABLE_NAME_TO_REPLACE", namingConventions.ValuesHistoryTableName);
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
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.Types;

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
				.Where(resourceName => Regex.IsMatch(resourceName, @".*\.sql"))
				.Select(resourceName => GetSqlResourceFullContentByName(resourceName))
				.Select(GetScriptWithExecutionOrder)
				.OrderBy(tuple => tuple.ExecutionOrder)
				.Select(tuple => new SqlScript(tuple.Content))
				.ToImmutableArray();

		/// <summary>
		/// Load resource with name <paramref name="resourceName"/> from current assembly. 
		/// </summary>
		private NonEmptyString GetSqlResourceFullContentByName(NonEmptyString resourceName)
		{
			using var resourceStream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream(resourceName)
				.ThrowIfNull($"Failed to load resource '{resourceName}' from assembly.");

			using var streamReader = new StreamReader(resourceStream);

			return streamReader
				.ReadToEnd()
				.Replace("SYSTEM_SCHEMA_NAME_TO_REPLACE", namingConventions.SystemSchemaName);
		}

		/// <summary>
		/// Parse resource content and extract SQL script with execution order. 
		/// </summary>
		private static (NonEmptyString Content, ushort ExecutionOrder) GetScriptWithExecutionOrder(NonEmptyString resourceContent)
		{
			var resourceContentLines = ((string) resourceContent).Split(Environment.NewLine);
			var executionOrderLine = resourceContentLines.First();

			const string orderValuePrefix = "-- execution-order: ";
			var firstLinePattern = $"{orderValuePrefix}[0-9]+";

			if (!Regex.IsMatch(executionOrderLine, firstLinePattern))
			{
				throw new ApplicationException($"SQL script must contain '{firstLinePattern}' as first line");
			}

			return
			(
				Content: resourceContentLines
					.Skip(count: 2)
					.JoinBy(Environment.NewLine),

				ExecutionOrder: executionOrderLine
					.Replace(orderValuePrefix, string.Empty)
					.To(ushort.Parse)
			);
		}
	}
}
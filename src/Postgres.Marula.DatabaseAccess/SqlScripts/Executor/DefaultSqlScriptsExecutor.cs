using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;

namespace Postgres.Marula.DatabaseAccess.SqlScripts.Executor
{
	/// <inheritdoc />
	internal class DefaultSqlScriptsExecutor : ISqlScriptsExecutor
	{
		private readonly ISqlScriptsProvider sqlScriptsProvider;
		private readonly ILogger<DefaultSqlScriptsExecutor> logger;
		private readonly INamingConventions namingConventions;

		public DefaultSqlScriptsExecutor(
			ISqlScriptsProvider sqlScriptsProvider,
			ILogger<DefaultSqlScriptsExecutor> logger,
			INamingConventions namingConventions)
		{
			this.sqlScriptsProvider = sqlScriptsProvider;
			this.logger = logger;
			this.namingConventions = namingConventions;
		}

		/// <inheritdoc />
		async Task ISqlScriptsExecutor.ExecuteScriptsAsync(IDbConnection dbConnection)
		{
			if (!await ScriptsMustBeExecuted(dbConnection))
			{
				return;
			}

			using var dbTransaction = dbConnection.BeginTransaction();

			foreach (var sqlScript in sqlScriptsProvider.GetAllOrderedByExecution())
			{
				try
				{
					await dbConnection.ExecuteAsync(sqlScript.Content, transaction: dbTransaction);
				}
				catch (Exception exception)
				{
					logger.LogError(exception, $"Failed to execute SQL script '{sqlScript.Name}'.");
					throw;
				}
			}

			dbTransaction.Commit();
		}

		/// <summary>
		/// Figure out is scripts execution required.
		/// </summary>
		private async Task<bool> ScriptsMustBeExecuted(IDbConnection dbConnection)
		{
			var commandText = string.Intern($@"
				select not exists (
					select 1
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});");

			return await dbConnection.QuerySingleAsync<bool>(commandText, new {namingConventions.SystemSchemaName});
		}
	}
}
using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;

namespace Postgres.Marula.DatabaseAccess.SqlScripts.Executor
{
	/// <inheritdoc />
	internal class DefaultSqlScriptsExecutor : ISqlScriptsExecutor
	{
		private readonly ISqlScriptsProvider sqlScriptsProvider;
		private readonly ILogger<DefaultSqlScriptsExecutor> logger;

		public DefaultSqlScriptsExecutor(
			ISqlScriptsProvider sqlScriptsProvider,
			ILogger<DefaultSqlScriptsExecutor> logger)
		{
			this.sqlScriptsProvider = sqlScriptsProvider;
			this.logger = logger;
		}

		/// <inheritdoc />
		async Task ISqlScriptsExecutor.ExecuteScriptsAsync(IDbConnection dbConnection)
		{
			logger.LogInformation("Scripts execution is started.");
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
			logger.LogInformation("Scripts execution completed successfully.");
		}
	}
}
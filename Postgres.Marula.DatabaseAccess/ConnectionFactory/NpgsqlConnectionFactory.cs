using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Provider;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <inheritdoc />
	internal class NpgsqlConnectionFactory : IDbConnectionFactory
	{
		private readonly ISqlScriptsProvider sqlScriptsProvider;
		private readonly IConfiguration configuration;
		private readonly ILogger<NpgsqlConnectionFactory> logger;
		private readonly INamingConventions namingConventions;

		public NpgsqlConnectionFactory(
			ISqlScriptsProvider sqlScriptsProvider,
			IConfiguration configuration,
			ILogger<NpgsqlConnectionFactory> logger,
			INamingConventions namingConventions)
		{
			this.sqlScriptsProvider = sqlScriptsProvider;
			this.configuration = configuration;
			this.logger = logger;
			this.namingConventions = namingConventions;
		}

		/// <inheritdoc />
		async Task<IDbConnection> IDbConnectionFactory.GetPreparedConnectionAsync()
		{
			var dbConnection = configuration
				.GetConnectionString("Default")
				.To(connectionString => new NpgsqlConnection(connectionString));

			await dbConnection.OpenAsync();
			await ExecuteRequiredScriptsAsync(dbConnection);
			return dbConnection;
		}

		/// <summary>
		/// If necessary, execute scripts received from <see cref="sqlScriptsProvider"/>
		/// within connection <paramref name="dbConnection"/>.
		/// </summary>
		private async Task ExecuteRequiredScriptsAsync(IDbConnection dbConnection)
		{
			if (!await ScriptsMustBeExecuted(dbConnection))
			{
				return;
			}

			using var dbTransaction = dbConnection.BeginTransaction();

			await sqlScriptsProvider
				.GetAllOrderedByExecution()
				.ForEachAsync(async sqlScript =>
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
				});

			dbTransaction.Commit();
		}

		/// <summary>
		/// Figure out is scripts execution required.
		/// </summary>
		private async Task<bool> ScriptsMustBeExecuted(IDbConnection dbConnection)
		{
			var commandText = string.Intern($@"
				select exists (
					select 1
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});");

			return await dbConnection.QuerySingleAsync<bool>(commandText, namingConventions);
		}

		/// <inheritdoc />
		async void IDbConnectionFactory.ReleaseConnection(IDbConnection dbConnection)
		{
			if (dbConnection is IAsyncDisposable asyncDisposable)
			{
				await asyncDisposable.DisposeAsync();
			}
			else
			{
				dbConnection.Dispose();
			}
		}
	}
}
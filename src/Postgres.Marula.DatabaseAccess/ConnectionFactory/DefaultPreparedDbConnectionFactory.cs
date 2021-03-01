using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <inheritdoc />
	internal class DefaultPreparedDbConnectionFactory : IPreparedDbConnectionFactory
	{
		private readonly Lazy<Task<IDbConnection>> lazyPreparedConnection;
		private readonly ISqlScriptsExecutor sqlScriptsExecutor;
		private readonly INamingConventions namingConventions;

		public DefaultPreparedDbConnectionFactory(
			IDbConnection dbConnection,
			ISqlScriptsExecutor sqlScriptsExecutor,
			INamingConventions namingConventions)
		{
			lazyPreparedConnection = new Lazy<Task<IDbConnection>>(
				() => PrepareConnectionAsync(dbConnection),
				LazyThreadSafetyMode.PublicationOnly);

			this.sqlScriptsExecutor = sqlScriptsExecutor;
			this.namingConventions = namingConventions;
		}

		/// <inheritdoc />
		Task<IDbConnection> IPreparedDbConnectionFactory.GetPreparedConnectionAsync() => lazyPreparedConnection.Value;

		/// <summary>
		/// Prepare database connection for future communications with server.
		/// </summary>
		private async Task<IDbConnection> PrepareConnectionAsync(IDbConnection dbConnection)
		{
			if (!dbConnection.State.HasFlag(ConnectionState.Open))
			{
				if (dbConnection is DbConnection awaitableConnection) await awaitableConnection.OpenAsync();
				else dbConnection.Open();
			}

			if (await DatabaseIsPrepared(dbConnection))
			{
				return dbConnection;
			}

			await sqlScriptsExecutor.ExecuteScriptsAsync(dbConnection);
			return dbConnection;
		}
		
		/// <summary>
		/// Figure out is database is prepared already.
		/// </summary>
		private async Task<bool> DatabaseIsPrepared(IDbConnection dbConnection)
		{
			var commandText = string.Intern($@"
				select not exists (
					select null
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});");

			return await dbConnection.QuerySingleAsync<bool>(commandText, new {namingConventions.SystemSchemaName});
		}
	}
}
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <inheritdoc />
	internal class DefaultPreparedDbConnectionFactory : IPreparedDbConnectionFactory
	{
		private readonly Lazy<Task<IDbConnection>> lazyPreparedConnection;
		private readonly ISqlScriptsExecutor sqlScriptsExecutor;

		public DefaultPreparedDbConnectionFactory(
			IDbConnection dbConnection,
			ISqlScriptsExecutor sqlScriptsExecutor)
		{
			lazyPreparedConnection = new Lazy<Task<IDbConnection>>(
				() => PrepareConnectionAsync(dbConnection),
				LazyThreadSafetyMode.PublicationOnly);

			this.sqlScriptsExecutor = sqlScriptsExecutor;
		}

		/// <inheritdoc />
		Task<IDbConnection> IPreparedDbConnectionFactory.GetPreparedConnectionAsync() => lazyPreparedConnection.Value;

		/// <summary>
		/// Prepare database connection for future communications with server.
		/// </summary>
		private async Task<IDbConnection> PrepareConnectionAsync(IDbConnection dbConnection)
		{
			await ((DbConnection) dbConnection).OpenAsync();
			await sqlScriptsExecutor.ExecuteScriptsAsync(dbConnection);
			return dbConnection;
		}
	}
}
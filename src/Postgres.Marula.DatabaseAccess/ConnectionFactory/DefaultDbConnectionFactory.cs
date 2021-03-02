using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <inheritdoc />
	internal class DefaultDbConnectionFactory : IDbConnectionFactory
	{
		private readonly Lazy<Task<IDbConnection>> lazyPreparedConnection;

		private readonly ISqlScriptsExecutor sqlScriptsExecutor;
		private readonly INamingConventions namingConventions;
		private readonly IReadOnlyCollection<IParameterLink> allParameterLinks;

		public DefaultDbConnectionFactory(
			IDbConnection dbConnection,
			ISqlScriptsExecutor sqlScriptsExecutor,
			INamingConventions namingConventions,
			IEnumerable<IParameterLink> allParameterLinks)
		{
			lazyPreparedConnection = new Lazy<Task<IDbConnection>>(
				() => PrepareConnectionAsync(dbConnection),
				LazyThreadSafetyMode.PublicationOnly);

			this.sqlScriptsExecutor = sqlScriptsExecutor;
			this.namingConventions = namingConventions;
			this.allParameterLinks = allParameterLinks.ToImmutableArray();
		}

		/// <inheritdoc />
		Task<IDbConnection> IDbConnectionFactory.GetConnectionAsync() => lazyPreparedConnection.Value;

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
			await FillParameterDictionaryTable(dbConnection);
			return dbConnection;
		}
		
		/// <summary>
		/// Figure out is database is prepared already.
		/// </summary>
		private async Task<bool> DatabaseIsPrepared(IDbConnection dbConnection)
		{
			var commandText = string.Intern($@"
				select exists (
					select null
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});");

			return await dbConnection.QuerySingleAsync<bool>(commandText, new {namingConventions.SystemSchemaName});
		}

		/// <summary>
		/// Fill table <see cref="INamingConventions.ParametersTableName"/>
		/// with all names of all parameters existing in project.
		/// </summary>
		private async Task FillParameterDictionaryTable(IDbConnection dbConnection)
		{
			var parameterNames = allParameterLinks
				.Select(parameterLink => (string) parameterLink.Name)
				.ToImmutableArray();

			var commandText = string.Intern($@"
				insert into {namingConventions.SystemSchemaName}.{namingConventions.ParametersTableName}
					(name)
				select unnest(@{nameof(parameterNames)}) as parameter_name;");

			await dbConnection.ExecuteAsync(commandText, new {parameterNames});
		}
	}
}
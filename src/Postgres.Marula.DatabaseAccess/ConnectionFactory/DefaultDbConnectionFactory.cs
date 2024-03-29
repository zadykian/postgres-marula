using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.DatabaseAccess.SqlScripts.Executor;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ConnectionFactory
{
	/// <inheritdoc />
	internal class DefaultDbConnectionFactory : IDbConnectionFactory
	{
		private readonly AsyncLazy<IDbConnection> lazyPreparedConnection;
		private readonly ISqlScriptsExecutor sqlScriptsExecutor;
		private readonly INamingConventions namingConventions;

		public DefaultDbConnectionFactory(
			IDbConnection dbConnection,
			ISqlScriptsExecutor sqlScriptsExecutor,
			INamingConventions namingConventions)
		{
			lazyPreparedConnection = new(() => PrepareConnectionAsync(dbConnection));
			this.sqlScriptsExecutor = sqlScriptsExecutor;
			this.namingConventions = namingConventions;
		}

		/// <inheritdoc />
		async Task<IDbConnection> IDbConnectionFactory.GetConnectionAsync() => await lazyPreparedConnection;

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

			// Acquire lock to prevent multiple threads from parallel scripts execution.
			using var _ = await Lock.AcquireAsync(lockTimeout: TimeSpan.FromMinutes(1));

			if (!await DatabaseStructureIsPrepared(dbConnection))
			{
				await sqlScriptsExecutor.ExecuteScriptsAsync(dbConnection);
				await FillParameterDictionaryTable(dbConnection);
			}

			return dbConnection;
		}

		/// <summary>
		/// Figure out if database structure is prepared already.
		/// </summary>
		private async Task<bool> DatabaseStructureIsPrepared(IDbConnection connection)
		{
			var queryText = string.Intern($@"
				select exists (
					select null
					from pg_catalog.pg_namespace
					where nspname = @{nameof(INamingConventions.SystemSchemaName)});");

			return await connection.QuerySingleAsync<bool>(queryText, new {namingConventions.SystemSchemaName});
		}

		/// <summary>
		/// Fill table <see cref="INamingConventions.ParametersTableName"/>
		/// with all names of all parameters existing in project.
		/// </summary>
		private async Task FillParameterDictionaryTable(IDbConnection dbConnection)
		{
			var parameterNames = AllParameterLinks()
				.Select(name => (string) name.Name)
				.ToImmutableArray();

			var commandText = string.Intern($@"
				insert into {namingConventions.SystemSchemaName}.{namingConventions.ParametersTableName}
					(name)
				select unnest(@{nameof(parameterNames)}) as parameter_name
				on conflict (name) do nothing;");

			await dbConnection.ExecuteAsync(commandText, new {parameterNames});
		}

		/// <summary>
		/// Get links to all parameters defined in application. 
		/// </summary>
		private static IEnumerable<IParameterLink> AllParameterLinks()
			=> AppDomain
				.CurrentDomain
				.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(IParameter)))
				.Select(parameterType => new ParameterLink(parameterType))
				.OrderBy(link => link.Name);
	}
}
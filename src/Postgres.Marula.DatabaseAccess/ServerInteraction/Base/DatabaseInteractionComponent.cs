using System.Data;
using System.Threading.Tasks;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction.Base
{
	/// <summary>
	/// Component which interacts with database server.
	/// </summary>
	internal abstract class DatabaseInteractionComponent
	{
		private readonly IDbConnectionFactory dbConnectionFactory;

		protected DatabaseInteractionComponent(IDbConnectionFactory dbConnectionFactory)
			=> this.dbConnectionFactory = dbConnectionFactory;

		/// <summary>
		/// Get connection to database to interact with. 
		/// </summary>
		protected Task<IDbConnection> GetConnectionAsync() => dbConnectionFactory.GetConnectionAsync();
	}
}
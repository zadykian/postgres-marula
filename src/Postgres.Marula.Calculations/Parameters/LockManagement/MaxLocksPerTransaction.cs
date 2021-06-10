using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using LocksCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.LockManagement
{
	/// <summary>
	/// [max_locks_per_transaction]
	/// The shared lock table tracks locks on
	/// max_locks_per_transaction * (max_connections + max_prepared_transactions) objects.
	/// This parameter controls the average number of object locks allocated for each transaction.
	/// </summary>
	internal class MaxLocksPerTransaction : IntegerParameterBase
	{
		private readonly IDatabaseServer databaseServer;

		public MaxLocksPerTransaction(
			IDatabaseServer databaseServer,
			ILogger<MaxLocksPerTransaction> logger) : base(logger)
			=> this.databaseServer = databaseServer;

		/// <inheritdoc />
		protected override async ValueTask<LocksCount> CalculateValueAsync()
		{
			var maxPartitionsCount = await MaxPartitionsCount();
			return (LocksCount) 1.2 * maxPartitionsCount;
		}

		private async Task<uint> MaxPartitionsCount()
		{
			await Task.CompletedTask;
			
			var res = databaseServer
				.GetAllHierarchicalLinks()
				.GroupBy(parentToChild => parentToChild.Parent)
				.MaxAsync()
		}
	}
}
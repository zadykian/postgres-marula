using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Infrastructure.Extensions;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using LocksCount = System.UInt32;
using Table = Postgres.Marula.Infrastructure.TypeDecorators.SchemaQualifiedObjectName;

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

		/// <summary>
		/// Get maximum count of child partitions among all partitioned and inherited tables.
		/// Sub-partitions are also taken into account.
		/// </summary>
		private async Task<uint> MaxPartitionsCount()
		{
			var allLinks = await databaseServer.GetAllHierarchicalLinks().ToArrayAsync();
			if (!allLinks.Any()) return default;

			IEnumerable<Table> AllInheritorsOf(Table parent)
				=> allLinks!
					.Where(link => link.Parent.Equals(parent))
					.Select(link => link.Child)
					.SelectMany(child => AllInheritorsOf(child).Add(child));

			return (uint) allLinks
				.Select(link => link.Parent)
				.Select(parent => AllInheritorsOf(parent).Count())
				.Max();
		}
	}
}
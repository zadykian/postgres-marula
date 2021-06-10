using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;
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
		private readonly IPgSettings pgSettings;

		public MaxLocksPerTransaction(
			IDatabaseServer databaseServer,
			IPgSettings pgSettings,
			ILogger<MaxLocksPerTransaction> logger) : base(logger)
		{
			this.databaseServer = databaseServer;
			this.pgSettings = pgSettings;
		}

		/// <inheritdoc />
		/// <remarks>
		/// If current value is more then calculated one, it stays untouched, because it could be adjusted manually
		/// on the assumption of some other aspects which didn't relate to tables partitioning.
		/// </remarks>
		protected override async ValueTask<LocksCount> CalculateValueAsync()
		{
			var maxPartitionsCount = await MaxPartitionsCount();
			var calculatedValue = (LocksCount) 1.2 * maxPartitionsCount;
			var currentValue = await pgSettings.ReadAsync<MaxLocksPerTransaction, LocksCount>();
			return Math.Max(calculatedValue, currentValue);
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
				=> allLinks
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
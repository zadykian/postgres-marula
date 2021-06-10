using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
		public MaxLocksPerTransaction(ILogger<MaxLocksPerTransaction> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override ValueTask<LocksCount> CalculateValueAsync() => ValueTask.FromResult<uint>(128);
	}
}
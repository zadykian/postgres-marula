using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using WorkersCount = System.UInt32;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum_max_workers]
	/// Specifies the maximum number of autovacuum
	/// processes (other than the autovacuum launcher) that may be running at any one time.
	/// </summary>
	internal class AutovacuumMaxWorkers : IntegerParameterBase
	{
		public AutovacuumMaxWorkers(ILogger<IntegerParameterBase> logger) : base(logger)
		{
		}

		protected override ValueTask<WorkersCount> CalculateValueAsync()
		{
			// todo
			return ValueTask.FromResult<uint>(4);
		}
	}
}
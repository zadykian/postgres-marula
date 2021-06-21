using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Autovacuum;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Base.Dependencies;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.HwInfo;

// ReSharper disable UnusedType.Global
// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.UInt32;
using Mem = Postgres.Marula.Infrastructure.TypeDecorators.Memory;

namespace Postgres.Marula.Calculations.Parameters.ResourceUsage.Memory
{
	/// <summary>
	/// [autovacuum_work_mem]
	/// Specifies the maximum amount of memory
	/// to be used by each autovacuum worker process.
	/// </summary>
	internal class AutovacuumWorkMem : MemoryParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;
		private readonly IPgSettings pgSettings;

		public AutovacuumWorkMem(
			IHardwareInfo hardwareInfo,
			IPgSettings pgSettings,
			ILogger<AutovacuumWorkMem> logger) : base(logger)
		{
			this.hardwareInfo = hardwareInfo;
			this.pgSettings = pgSettings;
		}

		/// <inheritdoc />
		public override IParameterDependencies Dependencies()
			=> ParameterDependencies
				.Empty
				.DependsOn<AutovacuumMaxWorkers>();

		/// <inheritdoc />
		protected override async ValueTask<Mem> CalculateValueAsync()
		{
			var totalRamSize = await hardwareInfo.TotalRam();
			var autovacuumMaxWorkers = await pgSettings.ReadAsync<AutovacuumMaxWorkers, CoresCount>();
			return 0.1 * totalRamSize / autovacuumMaxWorkers;
		}
	}
}
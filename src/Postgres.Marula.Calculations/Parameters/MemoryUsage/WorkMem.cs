using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParametersManagement;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.MemoryUsage
{
	/// <summary>
	/// [work_mem]
	/// Sets the base maximum amount of memory to be used by a query operation 
	/// (such as a sort or hash table) before writing to temporary disk files.
	/// </summary>
	internal class WorkMem : MemoryParameterBase
	{
		private readonly IHardwareInfo hardwareInfo;
		private readonly IPgSettings pgSettings;

		public WorkMem(
			IHardwareInfo hardwareInfo,
			IPgSettings pgSettings,
			ILogger<MemoryParameterBase> logger) : base(logger)
		{
			this.hardwareInfo = hardwareInfo;
			this.pgSettings = pgSettings;
		}

		/// <inheritdoc />
		protected override async ValueTask<Memory> CalculateValueAsync()
		{
		}
	}
}
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.HwInfo
{
	/// <inheritdoc cref="IHardwareInfo"/>
	/// <remarks>
	/// This implementation uses bash to access hardware info.
	/// </remarks>
	internal class BashHardwareInfo : HardwareInfoBase
	{
		public BashHardwareInfo(ILogger<BashHardwareInfo> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override NonEmptyString PathToExecutable => "/bin/bash";

		/// <inheritdoc />
		protected override async Task<Memory> GetTotalRamAsyncImpl()
		{
			const string fieldName = "MemTotal:";
			var memTotalString = await ExecuteCommandAsync($"grep {fieldName} /proc/meminfo");
			var refined = memTotalString.Replace(fieldName, string.Empty).Trim();
			return Memory.Parse(refined);
		}

		/// <inheritdoc />
		protected override async Task<CoresCount> GetCpuCoresCountAsyncImpl()
		{
			var coresCountString = await ExecuteCommandAsync("nproc");
			return CoresCount.Parse(coresCountString);
		}
	}
}
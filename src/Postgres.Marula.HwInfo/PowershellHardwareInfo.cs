using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.HwInfo
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation uses PowerShell to access hardware info.
	/// </remarks>
	internal class PowershellHardwareInfo : HardwareInfoBase
	{
		public PowershellHardwareInfo(ILogger<HardwareInfoBase> logger) : base(logger)
		{
		}

		/// <inheritdoc />
		protected override NonEmptyString PathToExecutable => "powershell.exe";

		/// <inheritdoc />
		protected override async Task<Memory> GetTotalRamAsync()
		{
			const string command = "(Get-CimInstance Win32_PhysicalMemory | Measure-Object -Property capacity -Sum).Sum";
			var totalRamInBytesString = await ExecuteCommandAsync(command);
			return ulong.Parse(totalRamInBytesString);
		}

		/// <inheritdoc />
		protected override async Task<CoresCount> GetCpuCoresCountAsync()
		{
			await Task.CompletedTask;
			var coresCountString = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")!;
			return CoresCount.Parse(coresCountString);
		}
	}
}
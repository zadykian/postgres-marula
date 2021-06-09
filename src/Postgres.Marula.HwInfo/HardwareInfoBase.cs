using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.HwInfo
{
	/// <summary>
	/// Base class for hardware info access implementations.
	/// </summary>
	internal abstract class HardwareInfoBase : IHardwareInfo
	{
		private readonly AsyncLazy<Memory> ramSizeAsyncCache;
		private readonly AsyncLazy<CoresCount> coresCountAsyncCache;
		private readonly ILogger<HardwareInfoBase> logger;

		protected HardwareInfoBase(ILogger<HardwareInfoBase> logger)
		{
			ramSizeAsyncCache = new(GetTotalRamAsync);
			coresCountAsyncCache = new(GetCpuCoresCountAsync);
			this.logger = logger;
		}

		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.TotalRam() => await ramSizeAsyncCache;

		/// <inheritdoc />
		async Task<CoresCount> IHardwareInfo.CpuCoresCount() => await coresCountAsyncCache;

		/// <summary>
		/// Path to executable.
		/// </summary>
		protected abstract NonEmptyString PathToExecutable { get; }

		/// <inheritdoc cref="IHardwareInfo.TotalRam"/>
		protected abstract Task<Memory> GetTotalRamAsync();

		/// <inheritdoc cref="IHardwareInfo.CpuCoresCount"/>
		protected abstract Task<CoresCount> GetCpuCoresCountAsync();

		/// <summary>
		/// Execute command and return received output. 
		/// </summary>
		protected async Task<NonEmptyString> ExecuteCommandAsync(NonEmptyString command)
		{
			using var process = new Process
			{
				StartInfo = new()
				{
					FileName = PathToExecutable,
					Arguments = $"-c \"{command}\"",
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				}
			};

			try
			{
				process.Start();
				var output = await process.StandardOutput.ReadToEndAsync();
				await process.WaitForExitAsync();
				return output;
			}
			catch (Exception exception)
			{
				logger.LogError(exception, $"Failed to execute command '{command}' via '{PathToExecutable}'.");
				throw;
			}
		}
	}
}
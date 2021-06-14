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
			ramSizeAsyncCache = new(GetTotalRamAsyncImpl);
			coresCountAsyncCache = new(GetCpuCoresCountAsyncImpl);
			this.logger = logger;
		}

		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.GetTotalRamAsync() => await ramSizeAsyncCache;

		/// <inheritdoc />
		async Task<CoresCount> IHardwareInfo.GetCpuCoresCountAsync() => await coresCountAsyncCache;

		/// <summary>
		/// Path to executable.
		/// </summary>
		protected abstract NonEmptyString PathToExecutable { get; }

		/// <inheritdoc cref="IHardwareInfo.GetTotalRamAsync"/>
		protected abstract Task<Memory> GetTotalRamAsyncImpl();

		/// <inheritdoc cref="IHardwareInfo.GetCpuCoresCountAsync"/>
		protected abstract Task<CoresCount> GetCpuCoresCountAsyncImpl();

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
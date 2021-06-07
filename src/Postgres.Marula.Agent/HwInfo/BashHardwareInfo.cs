using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Agent.HwInfo
{
	/// <inheritdoc />
	/// <remarks>
	/// This implementation uses bash to access hardware info.
	/// </remarks>
	internal class BashHardwareInfo : IHardwareInfo
	{
		private readonly ILogger<BashHardwareInfo> logger;

		public BashHardwareInfo(ILogger<BashHardwareInfo> logger) => this.logger = logger;

		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.TotalRam()
		{
			var memTotalString = await ExecuteBashCommandAsync("grep MemTotal /proc/meminfo");
			var refined = Regex.Replace()
		}

		/// <inheritdoc />
		byte IHardwareInfo.CpuCoresCount() => throw new System.NotImplementedException();

		/// <summary>
		/// Execute command <paramref name="command"/> via bash and return received output. 
		/// </summary>
		private async Task<NonEmptyString> ExecuteBashCommandAsync(NonEmptyString command)
		{
			using var process = new Process
			{
				StartInfo = new()
				{
					FileName = "/bin/bash",
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
				logger.LogError(exception, $"Failed to execute bash command '{command}'.");
				throw;
			}
		}
	}
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.HwInfo;
using Postgres.Marula.WebApi.Common;

namespace Postgres.Marula.Agent.Api
{
	/// <summary>
	/// Hardware info access.
	/// </summary>
	public class HardwareInfoController : ApiControllerBase
	{
		private readonly IHardwareInfo hardwareInfo;

		/// <param name="hardwareInfo">
		/// Hosting machine hardware info.
		/// </param>
		public HardwareInfoController(IHardwareInfo hardwareInfo) => this.hardwareInfo = hardwareInfo;

		/// <summary>
		/// Get total size of available RAM.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetTotalRamAsync() => Ok(await hardwareInfo.TotalRam());

		/// <summary>
		/// Get number of CPU cores.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetCpuCoresCountAsync() => Ok(await hardwareInfo.CpuCoresCount());
	}
}
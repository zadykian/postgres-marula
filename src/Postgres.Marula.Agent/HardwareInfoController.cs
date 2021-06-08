using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.HwInfo;
using static System.Net.Mime.MediaTypeNames;

namespace Postgres.Marula.Agent
{
	/// <summary>
	/// Hardware info access.
	/// </summary>
	[Produces(Application.Json)]
	public class HardwareInfoController : Controller
	{
		private readonly IHardwareInfo hardwareInfo;

		public HardwareInfoController(IHardwareInfo hardwareInfo) => this.hardwareInfo = hardwareInfo;

		/// <summary>
		/// Get total size of available RAM.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetTotalRam() => Ok(await hardwareInfo.TotalRam());

		/// <summary>
		/// Get number of CPU cores.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetCpuCoresCount() => Ok(await hardwareInfo.CpuCoresCount());
	}
}
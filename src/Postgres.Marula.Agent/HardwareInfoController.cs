using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.HwInfo;
using static System.Net.Mime.MediaTypeNames;

namespace Postgres.Marula.Agent
{
	/// <summary>
	/// Hardware info access.
	/// </summary>
	[ApiController]
	[Route("[controller]/[action]")]
	[Produces(Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public class HardwareInfoController : Controller
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.WebApi.Common;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.Agent.Api
{
	/// <summary>
	/// Hardware info access.
	/// </summary>
	public class HardwareInfoController : ApiControllerBase, IHardwareInfo
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
		public async Task<Memory> GetTotalRamAsync() => await hardwareInfo.GetTotalRamAsync();

		/// <summary>
		/// Get number of CPU cores.
		/// </summary>
		[HttpGet]
		public async Task<CoresCount> GetCpuCoresCountAsync() => await hardwareInfo.GetCpuCoresCountAsync();
	}
}
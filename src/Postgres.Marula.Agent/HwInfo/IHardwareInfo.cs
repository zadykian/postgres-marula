using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.Agent.HwInfo
{
	/// <summary>
	/// Hosting machine hardware info.
	/// </summary>
	public interface IHardwareInfo
	{
		/// <summary>
		/// Total size of RAM.
		/// </summary>
		Task<Memory> TotalRam();

		/// <summary>
		/// Number of CPU cores. 
		/// </summary>
		Task<CoresCount> CpuCoresCount();
	}
}
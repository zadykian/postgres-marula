using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Postgres.Marula.ControlApp
{
	/// <summary>
	/// Application entry point.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// Entry point method.
		/// </summary>
		private static Task Main(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				// todo
				.Build()
				.RunAsync();
	}
}
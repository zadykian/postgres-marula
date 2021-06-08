using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Postgres.Marula.Agent
{
	/// <summary>
	/// Application entry point. 
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// Entry point method. 
		/// </summary>
		private static void Main(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
				.Build()
				.Run();
	}
}
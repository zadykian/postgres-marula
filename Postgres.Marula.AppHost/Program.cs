using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.SolutionComponents.Factory;

namespace Postgres.Marula.AppHost
{
	internal static class Program
	{
		/// <summary>
		/// Application entry point. 
		/// </summary>
		private static async Task Main(string[] args)
			=> await Host
				.CreateDefaultBuilder(args)
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				})
				.To(hostBuilder => new Application(hostBuilder, new DefaultSolutionComponentsFactory()))
				.RunAsync();
	}
}
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Terminal UI service.
	/// </summary>
	internal class TerminalUiService : IHostedService
	{
		/// <inheritdoc />
		Task IHostedService.StartAsync(CancellationToken cancellationToken) => throw new System.NotImplementedException();

		/// <inheritdoc />
		Task IHostedService.StopAsync(CancellationToken cancellationToken) => throw new System.NotImplementedException();
	}
}
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Lifetime
{
	/// <inheritdoc />
	internal class UiShutdown : IUIShutdown
	{
		private readonly IHostApplicationLifetime applicationLifetime;

		public UiShutdown(IHostApplicationLifetime applicationLifetime)
			=> this.applicationLifetime = applicationLifetime;

		/// <inheritdoc />
		async Task IUIShutdown.StopAsync()
		{
			if (applicationLifetime.ApplicationStopping.IsCancellationRequested)
			{
				return;
			}

			await Task.CompletedTask;
			Application.RequestStop();
			Application.Top.Clear();
			applicationLifetime.StopApplication();
		}
	}
}
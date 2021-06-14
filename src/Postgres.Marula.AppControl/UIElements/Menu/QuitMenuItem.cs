using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Lifetime;
using Postgres.Marula.AppControl.UIElements.Messages;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <summary>
	/// Quit menu item.
	/// </summary>
	internal class QuitMenuItem : MenuItem
	{
		private readonly IUIShutdown uiShutdown;
		private readonly IMessageBox messageBox;

		public QuitMenuItem(
			IUIShutdown uiShutdown,
			IMessageBox messageBox) : base("quit", byte.MaxValue)
		{
			this.uiShutdown = uiShutdown;
			this.messageBox = messageBox;
		}

		/// <inheritdoc />
		public override async Task HandleClickAsync()
			=> await messageBox
				.QueryAsync(Name, "are you sure are you want to quit from ctl app?")
				.OnConfirmed(uiShutdown.StopAsync);
	}
}
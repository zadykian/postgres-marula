using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Lifetime;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;
using Postgres.Marula.App.Control.UIElements.Messages;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <summary>
	/// Quit menu item.
	/// </summary>
	internal class QuitMenuItem : MenuItemBase
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
				.Query(Name, "are you sure are you want to quit from ctl app?")
				.OnConfirmedAsync(uiShutdown.StopAsync);
	}
}
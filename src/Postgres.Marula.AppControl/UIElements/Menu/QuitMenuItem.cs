using System.Threading.Tasks;
using Postgres.Marula.AppControl.UIElements.Messages;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <summary>
	/// Quit menu item.
	/// </summary>
	internal class QuitMenuItem : MenuItem
	{
		private readonly IUserInterface userInterface;
		private readonly IMessageBox messageBox;

		public QuitMenuItem(
			IUserInterface userInterface,
			IMessageBox messageBox) : base("quit", byte.MaxValue)
		{
			this.userInterface = userInterface;
			this.messageBox = messageBox;
		}

		/// <inheritdoc />
		public override async Task HandleClickAsync()
		{
			await base.HandleClickAsync();
			var confirmed = await messageBox.QueryAsync(Name, "are you sure are you want to quit from ctl app?");
			if (confirmed) await userInterface.StopAsync();
		}
	}
}
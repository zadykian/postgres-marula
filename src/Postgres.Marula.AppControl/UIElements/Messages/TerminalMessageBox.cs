using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.Messages
{
	/// <inheritdoc />
	internal class TerminalMessageBox : IMessageBox
	{
		/// <inheritdoc />
		async Task<bool> IMessageBox.QueryAsync(NonEmptyString title, NonEmptyString message)
		{
			await Task.CompletedTask;
			var answerButtonNumber = MessageBox.Query(
				(string) title,
				(string) message, "yes", "no");
			return answerButtonNumber == 0;
		}
	}
}
using System;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Messages
{
	/// <inheritdoc />
	internal class TerminalMessageBox : IMessageBox
	{
		/// <inheritdoc />
		ConfirmationResult IMessageBox.Query(NonEmptyString title, NonEmptyString message)
		{
			var answerButtonNumber = MessageBox.Query(
				(string) title,
				(string) message, "yes", "no");

			return answerButtonNumber == 0
				? ConfirmationResult.Confirmed
				: ConfirmationResult.Rejected;
		}

		/// <inheritdoc />
		void IMessageBox.Info(NonEmptyString title, NonEmptyString message)
			=> MessageBox.Query((string) title, (string) message, "ok");

		/// <inheritdoc />
		void IMessageBox.Error(NonEmptyString title, Exception exception)
			=> MessageBox.ErrorQuery((string) title, $"error occured: '{exception.Message}'", "ok");
	}
}
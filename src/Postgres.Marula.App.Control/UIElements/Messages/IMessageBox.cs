using System;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.App.Control.UIElements.Messages
{
	/// <summary>
	/// Message box.
	/// </summary>
	internal interface IMessageBox
	{
		/// <summary>
		/// Show message box with <paramref name="title"/>, <paramref name="message"/>
		/// and confirm/reject buttons
		/// </summary>
		ConfirmationResult Query(NonEmptyString title, NonEmptyString message);

		/// <summary>
		/// Show info message box with <paramref name="title"/> and <paramref name="message"/>. 
		/// </summary>
		void Info(NonEmptyString title, NonEmptyString message);

		/// <summary>
		/// Show error message box with <paramref name="title"/> and <see cref="Exception.Message"/>.
		/// </summary>
		void Error(NonEmptyString title, Exception exception);
	}
}
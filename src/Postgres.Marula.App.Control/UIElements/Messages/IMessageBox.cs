using System.Threading.Tasks;
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
		Task<ConfirmationResult> QueryAsync(NonEmptyString title, NonEmptyString message);

		/// <summary>
		/// Show message box with <paramref name="title"/> and <paramref name="message"/>. 
		/// </summary>
		void Show( NonEmptyString title, NonEmptyString message);
	}
}
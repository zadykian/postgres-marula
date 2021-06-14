using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.AppControl.UIElements.Messages
{
	/// <summary>
	/// Message box.
	/// </summary>
	internal interface IMessageBox
	{
		/// <summary>
		/// Show message box with <paramref name="title"/> and <paramref name="message"/>. 
		/// </summary>
		/// <returns>
		/// <c>true</c> if user confirmed action, otherwise - <c>false</c>.
		/// </returns>
		Task<bool> QueryAsync(NonEmptyString title, NonEmptyString message);
	}
}
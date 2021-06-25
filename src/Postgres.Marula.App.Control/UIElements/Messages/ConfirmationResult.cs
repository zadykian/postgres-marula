using System;
using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Messages
{
	/// <summary>
	/// Message box operation confirmation result.
	/// </summary>
	internal enum ConfirmationResult : byte
	{
		/// <summary>
		/// Operation is confirmed by user and can be executed.
		/// </summary>
		Confirmed = 1,

		/// <summary>
		/// Operation is rejected by user.
		/// </summary>
		Rejected = 2
	}

	/// <summary>
	/// Extension methods for <see cref="ConfirmationResultExtensions"/> type.
	/// </summary>
	internal static class ConfirmationResultExtensions
	{
		/// <summary>
		/// Execute function <paramref name="onConfirmed"/>
		/// if <paramref name="confirmationResult"/> is equal to <see cref="ConfirmationResult.Confirmed"/>. 
		/// </summary>
		public static async Task OnConfirmedAsync(this ConfirmationResult confirmationResult, Func<Task> onConfirmed)
		{
			if (confirmationResult != ConfirmationResult.Confirmed) return;
			await onConfirmed();
		}
	}
}
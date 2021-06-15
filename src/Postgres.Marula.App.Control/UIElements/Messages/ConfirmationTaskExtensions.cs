using System;
using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Messages
{
	/// <summary>
	/// Extension methods for <see cref="IMessageBox"/> fluent syntax.
	/// </summary>
	internal static class ConfirmationTaskExtensions
	{
		/// <summary>
		/// Execute function <paramref name="onConfirmed"/>
		/// if <paramref name="confirmationTask"/> contains <c>true</c>. 
		/// </summary>
		public static async Task OnConfirmed(this Task<ConfirmationResult> confirmationTask, Func<Task> onConfirmed)
		{
			var confirmationResult = await confirmationTask;
			if (confirmationResult == ConfirmationResult.Confirmed) await onConfirmed();
		}

		/// <summary>
		/// Execute function <paramref name="onConfirmed"/>
		/// if <paramref name="confirmationTask"/> contains <c>true</c>. 
		/// </summary>
		public static async Task OnConfirmed(this Task<ConfirmationResult> confirmationTask, Func<ValueTask> onConfirmed)
		{
			var confirmationResult = await confirmationTask;
			if (confirmationResult == ConfirmationResult.Confirmed) await onConfirmed();
		}
	}
}
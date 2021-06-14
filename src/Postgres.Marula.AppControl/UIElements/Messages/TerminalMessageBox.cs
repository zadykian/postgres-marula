using System;
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

	/// <summary>
	/// Extension methods for <see cref="IMessageBox"/> fluent syntax.
	/// </summary>
	internal static class MessageBoxExtensions
	{
		/// <summary>
		/// Execute function <paramref name="onConfirmed"/>
		/// if <paramref name="confirmationTask"/> contains <c>true</c>. 
		/// </summary>
		public static async Task OnConfirmed(this Task<bool> confirmationTask, Func<Task> onConfirmed)
		{
			var confirmed = await confirmationTask;
			if (confirmed) await onConfirmed();
		}
		
		/// <summary>
		/// Execute function <paramref name="onConfirmed"/>
		/// if <paramref name="confirmationTask"/> contains <c>true</c>. 
		/// </summary>
		public static async Task OnConfirmed(this Task<bool> confirmationTask, Func<ValueTask> onConfirmed)
		{
			var confirmed = await confirmationTask;
			if (confirmed) await onConfirmed();
		}
	}
}
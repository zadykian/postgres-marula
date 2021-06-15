namespace Postgres.Marula.AppControl.UIElements.Messages
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
}
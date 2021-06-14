using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.Controls
{
	/// <summary>
	/// Application UI buttons.
	/// </summary>
	internal interface IButtons
	{
		/// <summary>
		/// Button to start all main host's jobs. 
		/// </summary>
		Button StartAllJobs();

		/// <summary>
		/// Button to stop all main host's jobs. 
		/// </summary>
		Button StopAllJobs();
	}
}
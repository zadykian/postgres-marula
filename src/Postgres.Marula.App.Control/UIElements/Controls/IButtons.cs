using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Controls
{
	/// <summary>
	/// Application UI buttons.
	/// </summary>
	internal interface IButtons
	{
		/// <summary>
		/// Button to calculate parameter values immediately. 
		/// </summary>
		Button CalculateImmediately();

		/// <summary>
		/// Button to export calculated values to .sql file.
		/// </summary>
		Button ExportValues();

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
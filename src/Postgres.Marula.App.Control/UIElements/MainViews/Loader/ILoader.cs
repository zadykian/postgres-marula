namespace Postgres.Marula.App.Control.UIElements.MainViews.Loader
{
	/// <summary>
	/// Loader UI element.
	/// </summary>
	internal interface ILoader
	{
		/// <summary>
		/// Being invoked when some long-running process is started.
		/// </summary>
		void OnLoadingStarted();

		/// <summary>
		/// Being invoked when some long-running process is finished.
		/// </summary>
		void OnLoadingFinished();
	}
}
using System.Timers;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
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

	/// <summary>
	/// Loading bar.
	/// </summary>
	internal class LoadingBar : ProgressBar, ILoader
	{
		private readonly Timer timer;

		public LoadingBar()
		{
			Height = 1;
			ColorScheme = Colors.Error;
			Width = Dim.Fill(1);
			timer = new(interval: 20);
			timer.Elapsed += (_, _) => Application.MainLoop?.Invoke(Pulse);
		}

		/// <inheritdoc />
		void ILoader.OnLoadingStarted() => timer.Start();

		/// <inheritdoc />
		void ILoader.OnLoadingFinished()
		{
			timer.Stop();
			Fraction = 0;
		}
	}
}
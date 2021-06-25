using System.Timers;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews.Loader
{
	/// <summary>
	/// Loading bar.
	/// </summary>
	internal class LoadingBar : ProgressBar, ILoader
	{
		private readonly Timer timer;

		public LoadingBar()
		{
			Height = 1;
			Width = Dim.Fill(1);
			timer = new(interval: 20);
			timer.Elapsed += (_, _) => Application.MainLoop?.Invoke(Pulse);
		}

		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public LoadingBar Initialize()
		{
			ColorScheme = BarColorScheme();
			return this;
		}

		/// <inheritdoc />
		void ILoader.OnLoadingStarted() => timer.Start();

		/// <inheritdoc />
		void ILoader.OnLoadingFinished()
		{
			timer.Stop();
			Fraction = 0;
		}
		
		/// <summary>
		/// Create UI color scheme for loading bar. 
		/// </summary>
		protected static ColorScheme BarColorScheme()
		{
			var normal = Application.Driver.MakeAttribute(fore: Color.Black, back: Color.Gray);
			var focused = Application.Driver.MakeAttribute(fore: Color.White, back: Color.Cyan);

			return new ColorScheme
			{
				Normal = normal,
				Focus = focused,
				HotNormal = normal,
				HotFocus = normal
			};
		}
	}
}
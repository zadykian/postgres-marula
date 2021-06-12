using Terminal.Gui;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Main UI window.
	/// </summary>
	internal class ControlWindow : Window
	{
		public ControlWindow() => Initialize();

		/// <summary>
		/// Perform main window initialization.
		/// </summary>
		private void Initialize()
		{
			Title = "postgres-marula-ctl";

			Width = Dim.Fill();
			Height = Dim.Fill();
			ColorScheme = new ColorScheme
			{
				Normal = Application.Driver.MakeAttribute(fore: Color.White, back: Color.DarkGray)
			};
		}
	}
}
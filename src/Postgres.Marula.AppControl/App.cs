using Terminal.Gui;

namespace Postgres.Marula.AppControl
{
	internal class App : Window
	{
		public App() => Initialize();

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
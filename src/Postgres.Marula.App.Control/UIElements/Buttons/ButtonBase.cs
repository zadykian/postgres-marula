using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Base class for all UI buttons.
	/// </summary>
	internal abstract class ButtonBase : Button
	{
		/// <summary>
		/// Create UI color scheme for buttons. 
		/// </summary>
		protected static ColorScheme ButtonColorScheme()
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
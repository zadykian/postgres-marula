using Terminal.Gui;

namespace Postgres.Marula.AppControl.UIElements.Controls
{
	/// <summary>
	/// Extension methods for <see cref="Button"/> type.
	/// </summary>
	internal static class ButtonExtensions
	{
		/// <summary>
		/// Set horizontal offset <paramref name="offset"/> for <paramref name="button"/>.
		/// </summary>
		public static Button WithHorizontalOffset(this Button button, Pos offset)
		{
			button.X = offset;
			return button;
		}
	}
}
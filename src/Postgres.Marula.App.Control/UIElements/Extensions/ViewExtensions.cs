using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="View"/> type.
	/// </summary>
	internal static class ViewExtensions
	{
		/// <summary>
		/// Set horizontal offset <paramref name="offset"/> for <paramref name="view"/>.
		/// </summary>
		public static View WithHorizontalOffset(this View view, Pos offset)
		{
			view.X = offset;
			return view;
		}

		/// <summary>
		/// Set vertical offset <paramref name="offset"/> for <paramref name="view"/>.
		/// </summary>
		public static View WithVerticalOffset(this View view, Pos offset)
		{
			view.Y = offset;
			return view;
		}
	}
}
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
		public static TView WithHorizontalOffset<TView>(this TView view, Pos offset)
			where TView : View
		{
			view.X = offset;
			return view;
		}

		/// <summary>
		/// Set vertical offset <paramref name="offset"/> for <paramref name="view"/>.
		/// </summary>
		public static TView WithVerticalOffset<TView>(this TView view, Pos offset)
			where TView : View
		{
			view.Y = offset;
			return view;
		}

		/// <summary>
		/// Fill all available space in width and height. 
		/// </summary>
		public static TView FillDimensions<TView>(this TView view)
			where TView : View
		{
			view.Width = Dim.Fill();
			view.Height = Dim.Fill();
			return view;
		}
	}
}
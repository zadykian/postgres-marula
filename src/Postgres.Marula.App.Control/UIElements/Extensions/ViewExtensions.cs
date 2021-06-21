using System;
using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="View"/> type and its' inheritors.
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
		
		/// <summary>
		/// Clear view. 
		/// </summary>
		public static TView Cleared<TView>(this TView view)
			where TView : View
		{
			view.Clear();
			return view;
		}

		/// <summary>
		/// Add <paramref name="childView"/> to <paramref name="view"/>. 
		/// </summary>
		public static TView With<TView>(this TView view, View childView)
			where TView : View
		{
			view.Add(childView);
			return view;
		}
		
		/// <summary>
		/// Set new title for <paramref name="frameView"/>. 
		/// </summary>
		public static TFrameView Titled<TFrameView>(this TFrameView frameView, NonEmptyString newTitle)
			where TFrameView : FrameView
		{
			frameView.Title = (string) newTitle;
			return frameView;
		}

		/// <summary>
		/// Add async function <paramref name="onSelection"/> as handler of <see cref="ListView.OpenSelectedItem"/>
		/// event if <see cref="ListViewItemEventArgs.Value"/> is assignable to <typeparamref name="TListItem"/>.
		/// </summary>
		public static ListView OnSelectionOf<TListItem>(this ListView listView, Func<TListItem, Task> onSelection)
		{
			listView.OpenSelectedItem += async eventArgs =>
			{
				if (eventArgs.Value is not TListItem value) return;
				await onSelection(value);
			};

			return listView;
		}
	}
}
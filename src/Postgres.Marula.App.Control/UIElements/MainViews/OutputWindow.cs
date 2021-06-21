using System.Collections.Generic;
using Postgres.Marula.App.Control.UIElements.Extensions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
{
	/// <summary>
	/// Current output UI window.
	/// </summary>
	internal interface IOutputWindow
	{
		/// <summary>
		/// Set new title for window. 
		/// </summary>
		IOutputWindow Titled(NonEmptyString newValue);

		/// <summary>
		/// Show <paramref name="output"/> in the window. 
		/// </summary>
		IOutputWindow Show<T>(IEnumerable<T> output);
	}

	/// <inheritdoc cref="IOutputWindow" />
	internal class OutputWindow : FrameView, IOutputWindow
	{
		/// <summary>
		/// Perform initialization. 
		/// </summary>
		public OutputWindow Initialize()
		{
			Width = Dim.Fill();
			Height = Dim.Fill();
			CanFocus = false;
			return this;
		}

		/// <inheritdoc />
		IOutputWindow IOutputWindow.Titled(NonEmptyString newValue) => this.Titled(newValue);

		/// <inheritdoc />
		IOutputWindow IOutputWindow.Show<T>(IEnumerable<T> output)
			=> output
				.AsListView()
				.FillDimensions()
				.To(listView => this.Cleared().With(listView));
	}
}
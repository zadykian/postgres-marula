using System.Collections.Generic;
using System.Linq;
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
		IOutputWindow Title(NonEmptyString newValue);

		/// <summary>
		/// Show <paramref name="outputLines"/> in the window. 
		/// </summary>
		IOutputWindow Show(IEnumerable<NonEmptyString> outputLines);
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
		IOutputWindow IOutputWindow.Title(NonEmptyString newValue)
		{
			Title = (string) newValue;
			return this;
		}

		/// <inheritdoc />
		IOutputWindow IOutputWindow.Show(IEnumerable<NonEmptyString> outputLines)
		{
			Clear();
			Add(new ListView(outputLines.ToArray())
			{
				Width = Dim.Fill(),
				Height = Dim.Fill()
			});

			return this;
		}
	}
}
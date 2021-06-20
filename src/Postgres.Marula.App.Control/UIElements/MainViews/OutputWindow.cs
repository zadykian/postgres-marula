using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.MainViews
{
	/// <summary>
	/// Current output UI window.
	/// </summary>
	public class OutputWindow : FrameView
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
	}
}
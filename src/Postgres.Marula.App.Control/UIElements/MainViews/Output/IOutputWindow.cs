using System.Collections.Generic;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.App.Control.UIElements.MainViews.Output
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
}
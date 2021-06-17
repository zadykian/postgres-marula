using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Lifetime
{
	/// <summary>
	/// Component responsible of UI initialization.
	/// </summary>
	interface IUIStartup
	{
		/// <summary>
		/// Start interaction with user. 
		/// </summary>
		Task StartAsync();
	}
}
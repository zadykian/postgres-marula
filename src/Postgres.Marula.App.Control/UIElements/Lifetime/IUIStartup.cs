using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Lifetime
{
	interface IUIStartup
	{
		/// <summary>
		/// Start interaction with user. 
		/// </summary>
		Task StartAsync();
	}
}
using System.Threading.Tasks;

namespace Postgres.Marula.AppControl.UIElements.Lifetime
{
	interface IUIStartup
	{
		/// <summary>
		/// Start interaction with user. 
		/// </summary>
		Task StartAsync();
	}
}
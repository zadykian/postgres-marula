using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Lifetime
{
	internal interface IUIShutdown
	{
		/// <summary>
		/// Stop interaction with user.
		/// </summary>
		Task StopAsync();
	}
}
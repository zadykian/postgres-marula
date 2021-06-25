using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Lifetime
{
	/// <summary>
	/// Component responsible UI application termination.
	/// </summary>
	internal interface IUIShutdown
	{
		/// <summary>
		/// Stop interaction with user.
		/// </summary>
		Task StopAsync();
	}
}
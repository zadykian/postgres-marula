using System.Threading.Tasks;

namespace Postgres.Marula.AppControl.UIElements.Lifetime
{
	internal interface IUIShutdown
	{
		/// <summary>
		/// Stop interaction with user.
		/// </summary>
		Task StopAsync();
	}
}
using System.Threading.Tasks;

namespace Postgres.Marula.AppControl.UIElements
{
	/// <summary>
	/// User interface.
	/// </summary>
	internal interface IUserInterface
	{
		/// <summary>
		/// Start interaction with user. 
		/// </summary>
		Task StartAsync();

		/// <summary>
		/// Stop interaction with user.
		/// </summary>
		Task StopAsync();
	}
}
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction.ViewFactory
{
	/// <summary>
	/// Component for value view creation.
	/// </summary>
	internal interface IValueViewFactory
	{
		/// <summary>
		/// Create <see cref="IValueView"/> based on link to parameter and its' value string representation.
		/// </summary>
		Task<IValueView> CreateAsync(IParameterLink link, NonEmptyString stringValue);
	}
}
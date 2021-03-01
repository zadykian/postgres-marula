using System.Reflection;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="PropertyInfo"/> type.
	/// </summary>
	public static class PropertyInfoExtensions
	{
		/// <summary>
		/// Get value of type <typeparamref name="TValue"/> from property <paramref name="propertyInfo"/>
		/// of object <paramref name="propertyOwner"/>.
		/// </summary>
		public static TValue GetValue<TValue>(
			this PropertyInfo propertyInfo,
			object propertyOwner) => (TValue) propertyInfo.GetValue(propertyOwner)!;
	}
}
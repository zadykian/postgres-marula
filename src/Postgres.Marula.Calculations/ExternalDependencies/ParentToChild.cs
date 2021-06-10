using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// Link between two tables in tables in database,
	/// in case when <see cref="Child"/> is a partition or an inheritor of <see cref="Parent"/>.
	/// </summary>
	public readonly struct ParentToChild
	{
		public ParentToChild(
			SchemaQualifiedObjectName parent,
			SchemaQualifiedObjectName child)
		{
			Parent = parent;
			Child = child;
		}

		/// <summary>
		/// Parent table fill name.
		/// </summary>
		public SchemaQualifiedObjectName Parent { get; }

		/// <summary>
		/// Child table fill name.
		/// </summary>
		public SchemaQualifiedObjectName Child { get; }
	}
}
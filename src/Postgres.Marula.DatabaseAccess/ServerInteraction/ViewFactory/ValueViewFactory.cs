using System;
using System.Globalization;
using System.Threading.Tasks;
using Dapper;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.ServerInteraction.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.DatabaseAccess.ServerInteraction.ViewFactory
{
	/// <inheritdoc cref="IValueViewFactory" />
	internal class ValueViewFactory : DatabaseInteractionComponent, IValueViewFactory
	{
		public ValueViewFactory(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{
		}

		/// <inheritdoc />
		async Task<IValueView> IValueViewFactory.CreateAsync(IParameterLink link, NonEmptyString stringValue)
		{
			if (!Fraction.TryParse(stringValue, out var fraction))
			{
				return new ValueView(link, stringValue);
			}

			var commandText = string.Intern($@"
				select min_val, max_val
				from pg_catalog.pg_settings
				where name = @{nameof(IParameterLink.Name)};");

			var connection = await Connection();
			var (minValue, maxValue) = await connection.QuerySingleAsync<(decimal, decimal)>(
				commandText,
				new {link.Name});

			var multiplier = (minValue, maxValue) switch
			{
				(decimal.Zero, decimal.One) => decimal.One,
				(decimal.Zero, 100)         => 100,
				_ => throw new NotSupportedException(
					$"Fraction parameter range [{minValue} .. {maxValue}] is not " +
					$"supported (parameter '{link.Name}').")
			};

			var normalizedValue = (fraction.Value * multiplier).ToString(CultureInfo.InvariantCulture);
			return new ValueView(link, normalizedValue);
		}
	}
}
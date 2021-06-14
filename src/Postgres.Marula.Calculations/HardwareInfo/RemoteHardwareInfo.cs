using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Configuration;
using Postgres.Marula.Calculations.Exceptions;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Http;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using CoresCount = System.Byte;

namespace Postgres.Marula.Calculations.HardwareInfo
{
	/// <inheritdoc cref="IHardwareInfo" />
	/// <remarks>
	/// This implementations accesses remote agent via HTTP.
	/// </remarks>
	internal class RemoteHardwareInfo : HttpComponentBase, IHardwareInfo
	{
		public RemoteHardwareInfo(ICalculationsConfiguration configuration)
			: base(configuration.General().AgentApiUri())
		{
		}

		/// <inheritdoc />
		async Task<Memory> IHardwareInfo.GetTotalRamAsync()
			=> await PerformRequestAsync<Memory>(HttpMethod.Get, "HardwareInfo/GetTotalRam");

		/// <inheritdoc />
		async Task<CoresCount> IHardwareInfo.GetCpuCoresCountAsync()
			=> await PerformRequestAsync<CoresCount>(HttpMethod.Get, "HardwareInfo/GetCpuCoresCount");

		/// <inheritdoc />
		protected override Exception WrapException(Exception occuredError) => Error.FailedToAccessAgent(occuredError);

		/// <inheritdoc />
		protected override JsonSerializerOptions ConfigureSerializerOptions(JsonSerializerOptions serializerOptions)
		{
			serializerOptions.Converters.Add(MemoryConverter.Instance);
			return serializerOptions;
		}

		/// <summary>
		/// JSON read-only converter for <see cref="Memory"/> type.
		/// </summary>
		private sealed class MemoryConverter : JsonConverter<Memory>
		{
			private MemoryConverter()
			{
			}

			/// <summary>
			/// Converter instance.
			/// </summary>
			public static MemoryConverter Instance { get; } = new();

			/// <inheritdoc />
			public override Memory Read(
				ref Utf8JsonReader reader,
				Type typeToConvert,
				JsonSerializerOptions options)
			{
				reader.Read(); // skip start object: '{'
				reader.Read(); // skip property name: 'totalBytes'
				var totalBytesValue = reader.GetUInt64();
				while (reader.TokenType != JsonTokenType.EndObject) reader.Read();
				return new(totalBytesValue);
			}

			/// <inheritdoc />
			public override void Write(
				Utf8JsonWriter writer,
				Memory value,
				JsonSerializerOptions options) => throw new NotSupportedException();
		}
	}
}
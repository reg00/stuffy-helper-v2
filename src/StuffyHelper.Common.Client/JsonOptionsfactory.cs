using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StuffyHelper.Common.Client;

public static class JsonOptionsFactory
    {
        private static readonly Lazy<JsonSerializerOptions> Options;
        private static readonly Lazy<JsonSerializerOptions> OptionsIndent;
        private static readonly List<JsonConverter> CustomConverters = new();

        static JsonOptionsFactory()
        {
            Options = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions().ConfigureDefaultOptions());
            OptionsIndent = new Lazy<JsonSerializerOptions>(() => new JsonSerializerOptions().ConfigureDefaultOptionsIndent());
        }

        public static JsonSerializerOptions DefaultOptionsIndent => OptionsIndent.Value;
        public static JsonSerializerOptions DefaultOptions => Options.Value;

        public static JsonSerializerOptions ConfigureDefaultOptions(this JsonSerializerOptions options)
        {
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.AllowTrailingCommas = true;
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));


            foreach (var converter in CustomConverters)
            {
                options.Converters.Add(converter);
            }

            return options;
        }

        public static JsonSerializerOptions ConfigureDefaultOptionsIndent(this JsonSerializerOptions options)
        {
            ConfigureDefaultOptions(options);
            options.WriteIndented = true;

            return options;
        }

        public static void AddUniqueCustomCoverter(JsonConverter jsonConverter)
        {
            var type = jsonConverter.GetType();

            if (!CustomConverters.Any(x => x.GetType() == type))
                CustomConverters.Add(jsonConverter);
        }
    }
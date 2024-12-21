using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable IDE0130
namespace iFind.Net.Utils;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options) => writer.WriteStringValue($"{date:yyyy-MM-dd HH:mm:ss}");
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateTime.Parse(reader.GetString()!);
}

public static class JsonHelper
{
    public static JsonSerializerOptions JsonSerializerOptions { get; }

    static JsonHelper()
    {
        JsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    }
}

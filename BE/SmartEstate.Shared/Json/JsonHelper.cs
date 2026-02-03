using System.Text.Json;

namespace SmartEstate.Shared.Json;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public static string ToJson<T>(T value) => JsonSerializer.Serialize(value, Options);

    public static T? FromJson<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);
}

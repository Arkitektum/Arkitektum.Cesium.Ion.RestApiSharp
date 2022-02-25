using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Arkitektum.Cesium.Ion.RestApiSharp.Util;

internal class AssetTypeEnumJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null) return;

        var str = value.ToString();
        writer.WriteValue(str != null && Regex.IsMatch(str, @"^_") ? str[1..] : str);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null) return null;

        var value = reader.Value.ToString();
        if (value != null && Regex.IsMatch(value, @"^\d+$"))
            return Enum.Parse(objectType, value);

        if (value != null && Regex.IsMatch(value, @"^\d+"))
            value = "_" + value;

        return value != null ? Enum.Parse(objectType, value) : null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(AssetType);
    }
}
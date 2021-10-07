using System;
using Newtonsoft.Json;

///<summary>
/// Helper to ensure ItemData integers are deserialized to Int32 type instead of Int64 by default.
/// Adapted from: https://stackoverflow.com/a/9444519/8585687
///</summary>
public class ItemDataConverter : JsonConverter
{
    // This converter is only used for deserialization; no need to write
    public override bool CanWrite { get => false; }

    // This converter is only used for the ItemData type
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ItemData);
    }

    // Override for JSON deserialization: adds a check for integer types specifically, otherwise default
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        ItemData result = new ItemData();
        reader.Read();

        while (reader.TokenType == JsonToken.PropertyName)
        {
            string propertyName = reader.Value as string;
            reader.Read();

            object value;
            if (reader.TokenType == JsonToken.Integer)
                value = Convert.ToInt32(reader.Value);
            else
                value = serializer.Deserialize(reader);
            result.Add(propertyName, value);
            reader.Read();
        }

        return result;
    }

    // This converter is only used for deserialization; no need to write
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
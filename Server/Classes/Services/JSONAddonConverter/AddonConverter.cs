using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLibs;

public class AddonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => typeof(Addon).IsAssignableFrom(objectType);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var typeName = jsonObject["Type"]?.ToString();

        Addon addon = typeName switch
        {
            "MapWall" => new MapWall(),
            "MapPelletColor" => new MapPelletColor(),
            "MapPelletShape" => new MapPelletShape(),
            _ => throw new NotSupportedException($"Unknown addon type: {typeName}")
        };

        serializer.Populate(jsonObject.CreateReader(), addon);
        return addon;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName("Type");
        writer.WriteValue(value.GetType().Name);

        foreach (var property in value.GetType().GetProperties())
        {
            writer.WritePropertyName(property.Name);
            var propertyValue = property.GetValue(value);
            serializer.Serialize(writer, propertyValue);
        }

        writer.WriteEndObject();
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ObjectDetector;

public static class Serialization
{
    public static string Serialize<T>(T obj)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { },
            // 2025-03-24T18:00:04.000000
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK", 
        };
        

        return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
    }

    public static T? Deserialize<T>(string json)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = { new StringEnumConverter() },
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK", 
        };
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}
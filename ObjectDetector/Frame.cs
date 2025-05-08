using Newtonsoft.Json;

namespace ObjectDetector;

public record Frame
{
    [JsonProperty("frame_id")] public int FrameId { get; init; }
    [JsonProperty("timestamp")] public DateTime Timestamp { get; init; }
    [JsonProperty("detections")] public List<Detection> Detections { get; init; } = new();
}

public record Detection
{
    [JsonProperty("id")] public int? Id { get; set; } // optional id for the detection
    [JsonProperty("x")] public float X { get; set; } // center x-coordinate (normalized 0-1)
    [JsonProperty("y")] public float Y { get; set; } // center y-coordinate (normalized 0-1)
    [JsonProperty("width")] public float Width { get; set; } // width (normalized 0-1)
    [JsonProperty("height")] public float Height { get; set; } // height (normalized 0-1)
}
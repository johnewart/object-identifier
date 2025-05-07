using System.Text.Json.Serialization;

public record Frame
{
    [JsonPropertyName("frame_id")]
    public int FrameId { get; init; }
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }
    [JsonPropertyName("detections")]
    public List<Detection> Detections { get; init; } = new();
}

public record Detection
{
    [JsonPropertyName("id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Id { get; set; } // optional id for the detection
    [JsonPropertyName("x")]
    public float X { get; set; } // center x-coordinate (normalized 0-1)
    [JsonPropertyName("y")]
    public float Y { get; set; } // center y-coordinate (normalized 0-1)
    [JsonPropertyName("width")]
    public float Width { get; set; } // width (normalized 0-1)
    [JsonPropertyName("height")]
    public float Height { get; set; } // height (normalized 0-1)
}


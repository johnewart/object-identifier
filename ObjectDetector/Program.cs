using System.Text.Json;

namespace ObjectDetector;

public class Program
{
    public List<Frame> ReadFrames(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<List<Frame>>(json);
    }
    
    public static void Main(string[] args)
    {
        var dataDir = "/Users/johnewart/Projects/ObjectDetector/data";
        
        var processor = new FrameProcessor(
            Path.Combine(dataDir, "input"),
            Path.Combine(dataDir, "output"),
            Path.Combine(dataDir, "visualization")
        );
        
        var identifier = new ObjectIdentifier(0.05f, 5, distanceTolerance: 0.1f);
        var runId = "run04";
        var inputFileName = $"{runId}.json";
        var result = processor.Process(inputFileName, identifier);
        processor.GenerateVisualization(runId, result);
    }
}
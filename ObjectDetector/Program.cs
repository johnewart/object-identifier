using System.Text.Json;

namespace ObjectDetector;

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = null;
        string outputFilePath = null;
        string visualizationDir = null;

        // Parse command-line arguments
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--input":
                    inputFilePath = args[++i];
                    break;
                case "--output":
                    outputFilePath = args[++i];
                    break;
                case "--vis-dir":
                    visualizationDir = args[++i];
                    break;
                default:
                    Console.WriteLine($"Unknown argument: {args[i]}");
                    return;
            }
        }
        
        // Validate arguments
        if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath) || string.IsNullOrEmpty(visualizationDir))
        {
            Console.WriteLine("Usage: --input <inputFilePath> --output <outputFilePath> --vis-dir <visualizationDir>");
            return;
        }

        // Process frames
        var processor = new FrameProcessor(
            inputFilePath,
            outputFilePath,
            visualizationDir
        );

        var identifier = new ObjectIdentifier(0.05f, 5, distanceTolerance: 0.1f);
        var result = processor.Process(identifier);
        processor.GenerateVisualization(result);
    }
}
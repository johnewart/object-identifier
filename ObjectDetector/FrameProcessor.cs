using System.Drawing;
using System.Text.Json;
using ScottPlot;
using Color = ScottPlot.Color;

namespace ObjectDetector;

public class FrameProcessor(string inputFileDir, string outputFileDir, string visualizationFileDir)
{
    public List<Frame> ReadFrames(string filePath)
    {
        var fullPath = Path.Join(inputFileDir, filePath);
        using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<List<Frame>>(json) ?? [];
    }

    public List<Frame> Process(string filename, ObjectIdentifier identifier)
    {
        var frames = ReadFrames(filename);
        var outputFileName = filename.Replace(".json", "_output.json");
        var outputFilePath = Path.Join(outputFileDir, outputFileName);
        var output = new List<Frame>();

        foreach (var frame in frames)
        {
            var outputFrame = new Frame
            {
                FrameId = frame.FrameId,
                Detections = new List<Detection>()
            };


            foreach (var detection in frame.Detections)
            {
                var objectId = identifier.IdentifyObject(frame.FrameId, detection);

                outputFrame.Detections.Add(detection with { Id = objectId });
            }

            output.Add(outputFrame);
        }

        // Write the output to a file
        var outputJson = JsonSerializer.Serialize(output);
        using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(outputStream);
        writer.Write(outputJson);
        writer.Flush();
        writer.Close();


        return output;
    }

    public void GenerateVisualization(string directoryName, List<Frame> frames)
    {
        string[] vizcolors =
        [
            "#00429d", "#1d4da2", "#2c58a7", "#3963ac", "#446eb1", "#4f7ab6", "#5985bb", "#6391c0", "#6e9dc4",
            "#78a8c9", "#83b4cd", "#8fc0d1", "#9bccd5", "#a8d8d9", "#b7e3dc", "#c8eedf", "#ddf8e1", "#ffffe0"
        ];

        //var plotColors = vizcolors.Select(s => ScottPlot.Color.FromHex(s)).ToArray();
        var plotColors = new ScottPlot.Colormaps.Turbo()
            .GetColors(10)
            .ToArray();;


        var visualizationOutputDir = Path.Join(visualizationFileDir, directoryName);
        Directory.CreateDirectory(visualizationOutputDir);
        var count = 0;
        List<string> filenames = new List<string>();


        foreach (var frame in frames)
        {
            count += 1;
            var visualizationFileName = frame.FrameId.ToString().PadLeft(5, '0') + ".png";
            var visualizationFilePath = Path.Join(visualizationOutputDir, visualizationFileName);
            filenames.Add(visualizationFileName);

            Plot framePlot = new();
            framePlot.Axes.SetLimits(0, 1.0, 0, 1.0);

            foreach (var detection in frame.Detections)
            {
                var rect = framePlot.Add.Rectangle(
                    detection.X,
                    detection.X + detection.Width,
                    detection.Y,
                    detection.Y + detection.Height);
                var color = plotColors[detection.Id ?? 0 % plotColors.Length];
                Console.WriteLine($"{detection.Id ?? -1} -> {color.ToHex()}");
                rect.FillColor = color.WithOpacity(0.7);
                rect.LineColor = Colors.Black;
                rect.LineWidth = 1;


                var txt = framePlot.Add.Text(detection.Id?.ToString() ?? "UFO", detection.X + .01, detection.Y);
                txt.LabelAlignment = Alignment.LowerLeft;
                txt.LabelFontSize = 12;
            }

            framePlot.SavePng(visualizationFilePath, 640, 480);
        }

        var visualizationHtml = VisualizationTemplate.GenerateVisualization(filenames);
        var visualizationHtmlFilePath = Path.Join(visualizationOutputDir, "index.html");
        using var visualizationHtmlStream =
            new FileStream(visualizationHtmlFilePath, FileMode.Create, FileAccess.Write);
        using var visualizationHtmlWriter = new StreamWriter(visualizationHtmlStream);
        visualizationHtmlWriter.Write(visualizationHtml);
        visualizationHtmlWriter.Flush();
        visualizationHtmlWriter.Close();
    }
}
using System.Text.Json;
using ScottPlot;

namespace ObjectDetector;

public class FrameProcessor(string inputFilePath, string outputFilePath, string visualizationFileDir)
{
    public List<Frame> ReadFrames(string filePath)
    {
        using var stream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var frames = Serialization.Deserialize<List<Frame>>(json) 
               ?? throw new InvalidOperationException("Failed to deserialize frames");
        return frames;
    }

    public List<Frame> Process(ObjectIdentifier identifier)
    {
        // Make sure the directory for output files exists
        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath) ?? string.Empty);
        
        var frames = ReadFrames(inputFilePath);
        var output = new List<Frame>();

        foreach (var frame in frames)
        {
            var outputFrame = new Frame
            {
                FrameId = frame.FrameId,
                Timestamp = frame.Timestamp,
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
        var outputJson = Serialization.Serialize(output);
        using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(outputStream);
        writer.Write(outputJson);
        writer.Flush();
        writer.Close();


        return output;
    }

    public void GenerateVisualization(List<Frame> frames)
    {
        //var plotColors = vizcolors.Select(s => ScottPlot.Color.FromHex(s)).ToArray();
        var plotColors = new ScottPlot.Colormaps.Turbo()
            .GetColors(10)
            .ToArray();;


        Directory.CreateDirectory(visualizationFileDir);
        List<string> filenames = new List<string>();


        foreach (var frame in frames)
        {
            var visualizationFileName = frame.FrameId.ToString().PadLeft(5, '0') + ".png";
            var visualizationFilePath = Path.Join(visualizationFileDir, visualizationFileName);
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
                rect.FillColor = color.WithOpacity(0.9);
                rect.LineColor = Colors.Black;
                rect.LineWidth = 1;

                var txt = framePlot.Add.Text(detection.Id?.ToString() ?? "UFO", detection.X + .01, detection.Y);
                txt.LabelAlignment = Alignment.LowerLeft;
                txt.LabelFontSize = 12;
                txt.LabelFontColor = Colors.White;
                txt.LabelBold = true;
            }

            framePlot.SavePng(visualizationFilePath, 640, 480);
        }

        var visualizationHtml = VisualizationTemplate.GenerateVisualization(filenames);
        var visualizationHtmlFilePath = Path.Join(visualizationFileDir, "index.html");
        using var visualizationHtmlStream =
            new FileStream(visualizationHtmlFilePath, FileMode.Create, FileAccess.Write);
        using var visualizationHtmlWriter = new StreamWriter(visualizationHtmlStream);
        visualizationHtmlWriter.Write(visualizationHtml);
        visualizationHtmlWriter.Flush();
        visualizationHtmlWriter.Close();
    }
}
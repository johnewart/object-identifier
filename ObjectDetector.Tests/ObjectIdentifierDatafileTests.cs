namespace ObjectDetector.Tests;

public class ObjectIdentifierDatafileTests
{
    private string dataRoot = Path.Combine(Directory.GetCurrentDirectory(), "data");

    private FrameProcessor GetProcessor(string datafilename)
    {
        var inputDataDir = Path.Combine(dataRoot, "input");
        if (!Directory.Exists(inputDataDir))
        {
            Directory.CreateDirectory(inputDataDir);
        }
        var outputDataDir = Path.Combine(dataRoot, "output");
        if (!Directory.Exists(outputDataDir))
        {
            Directory.CreateDirectory(outputDataDir);
        }
        var visualizationDataDir = Path.Combine(dataRoot, "visualization");
        if (!Directory.Exists(visualizationDataDir))
        {
            Directory.CreateDirectory(visualizationDataDir);
        }
        var inputFilePath = Path.Combine(inputDataDir, datafilename);
        var outputFilePath = Path.Combine(outputDataDir, datafilename);
        
        return new FrameProcessor(
            inputFilePath,
            outputFilePath,
            visualizationDataDir
        );
    }
            
    [Fact]
    public void Run01()
    {

        var processor = GetProcessor("run01.json");
        var identifier = new ObjectIdentifier(0.001f, 5, distanceTolerance: 0.001f);
        var output = processor.Process(identifier );
        
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            Assert.Equal(2, frame.Detections.Count);
            
            foreach(var detection in frame.Detections)
            {
                uniqueIds.Add(detection.Id ?? -1);
            }        
        }
        
        Assert.Equal(2, uniqueIds.Count); // Expecting two unique object IDs
        
        processor.GenerateVisualization(output);
    }
    
    [Fact]
    // use the test run02.json file and verify the output
    public void Run02()
    {
        var processor = GetProcessor("run02.json");
        var identifier = new ObjectIdentifier(0.05f, 5);
        var output = processor.Process( identifier );
        
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            var frameIds = new HashSet<int>();
            // Assert all detections have a unique ID
            Assert.All(frame.Detections, x => frameIds.Add(x?.Id ?? -1));
            foreach(var detection in frame.Detections)
            {
             uniqueIds.Add(detection.Id ?? -1);
            }
        }
        Assert.Equal(2, uniqueIds.Count); 
        Assert.DoesNotContain(-1, uniqueIds);
    }

    [Fact]
    // use the test run02.json file and verify the output
    public void Run03_SmallThreshold()
    {
        var processor = GetProcessor("run03.json");
        var identifier = new ObjectIdentifier(0.05f, 5);
        var output = processor.Process( identifier);
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            var frameIds = new HashSet<int>();
            // Assert all detections have a unique ID
            Assert.All(frame.Detections, x => frameIds.Add(x?.Id ?? -1));
            foreach (var detection in frame.Detections)
            {
                uniqueIds.Add(detection.Id ?? -1);
            }
        }

        Assert.Equal(3, uniqueIds.Count); 
        
        Assert.DoesNotContain(-1, uniqueIds);

    }
    
    [Fact]
    public void Run03_LargerThreshold()
    {
        var processor = GetProcessor("run03.json");
        var identifier = new ObjectIdentifier(0.05f, 10);
        var output = processor.Process( identifier );
        
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            var frameIds = new HashSet<int>();
            // Assert all detections have a unique ID
            Assert.All(frame.Detections, x => frameIds.Add(x?.Id ?? -1));
            foreach(var detection in frame.Detections)
            {
                uniqueIds.Add(detection.Id ?? -1);
            }
        }
        Assert.Equal(2, uniqueIds.Count);
        Assert.DoesNotContain(-1, uniqueIds);
    }
}
namespace ObjectDetector.Tests;

public class ObjectIdentifierDatafileTests
{
    private const string dataRoot = "/Users/johnewart/Projects/ObjectDetector/data";
    
    private FrameProcessor _frameProcessor = new FrameProcessor(
        Path.Combine(dataRoot, "input"),
        Path.Combine(dataRoot, "output"),
        Path.Combine(dataRoot, "visualization")
    );
            
    [Fact]
    // use the test run01.json file and verify the output
    public void Run01()
    {

        var identifier = new ObjectIdentifier(0.05f, 5);
        var output = _frameProcessor.Process("run01.json", identifier );
        
        // Verify the results
        // expect that there are two objects in all the frames
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            Assert.Equal(2, frame.Detections.Count);
            Assert.NotEqual(frame.Detections[0].Id, frame.Detections[1].Id);
            uniqueIds.Add(frame.Detections[0].Id ?? -1);
        }
        Assert.Equal(2, uniqueIds.Count); // Expecting two unique object IDs
    }
    
    [Fact]
    // use the test run02.json file and verify the output
    public void Run02()
    {
        var identifier = new ObjectIdentifier(0.05f, 5);
        var output = _frameProcessor.Process("run02.json", identifier );
        
        // Verify the results
        // expect that there are two objects in all the frames
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            // ReSharper disable once CollectionNeverQueried.Local
            var frameIds = new HashSet<int>();
            // Assert all detections have a unique ID
            Assert.All(frame.Detections, x => frameIds.Add(x?.Id ?? -1));
            foreach(var detection in frame.Detections)
            {
             uniqueIds.Add(detection.Id ?? -1);
            }
        }
        Assert.Equal(2, uniqueIds.Count); // Expecting two unique object IDs
        // Ensure no -1 IDs exist
        Assert.DoesNotContain(-1, uniqueIds);
    }

    [Fact]
    // use the test run02.json file and verify the output
    public void Run03_SmallThreshold()
    {
        var identifier = new ObjectIdentifier(0.05f, 5);
        var output = _frameProcessor.Process("run03.json", identifier);

        // Verify the results
        // expect that there are two objects in all the frames
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            // ReSharper disable once CollectionNeverQueried.Local
            var frameIds = new HashSet<int>();
            // Assert all detections have a unique ID
            Assert.All(frame.Detections, x => frameIds.Add(x?.Id ?? -1));
            foreach (var detection in frame.Detections)
            {
                uniqueIds.Add(detection.Id ?? -1);
            }
        }

        Assert.Equal(3, uniqueIds.Count); // Expecting two unique object IDs
        // Ensure no -1 IDs exist
        Assert.DoesNotContain(-1, uniqueIds);

    }
    
    [Fact]
    // use the test run02.json file and verify the output
    public void Run03_LargerThreshold()
    {
        var identifier = new ObjectIdentifier(0.05f, 10);
        var output = _frameProcessor.Process("run03.json", identifier );
        
        // Verify the results
        // expect that there are two objects in all the frames
        var uniqueIds = new HashSet<int>();
        foreach (var frame in output)
        {
            // ReSharper disable once CollectionNeverQueried.Local
            var frameIds = new HashSet<int>();
            // Assert all detections have a unique ID
            Assert.All(frame.Detections, x => frameIds.Add(x?.Id ?? -1));
            foreach(var detection in frame.Detections)
            {
                uniqueIds.Add(detection.Id ?? -1);
            }
        }
        Assert.Equal(2, uniqueIds.Count); // Expecting two unique object IDs
        // Ensure no -1 IDs exist
        Assert.DoesNotContain(-1, uniqueIds);
    }
}
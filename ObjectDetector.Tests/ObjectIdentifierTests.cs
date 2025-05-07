using Xunit;

namespace ObjectDetector.Tests;

public class ObjectIdentifierTests
{
    [Fact]
    public void IdentifyObject_ShouldAddNewObject_WhenNoOverlapExists()
    {
        var identifier = new ObjectIdentifier(0.01f, 3);
        var detection = new Detection { X = 0.1f, Y = 0.2f, Width = 0.05f, Height = 0.05f };
        var objectId = identifier.IdentifyObject(1, detection);

        Assert.Equal(0, objectId); // First object should have ID 0
    }

    [Fact]
    public void IdentifyObject_ShouldReturnSameObjectId_WhenOverlapExistsWithinFrameThreshold()
    {
        var identifier = new ObjectIdentifier(0.01f, 3);
        var detection1 = new Detection { X = 0.1f, Y = 0.2f, Width = 0.05f, Height = 0.05f };
        var detection2 = new Detection { X = 0.12f, Y = 0.22f, Width = 0.05f, Height = 0.05f };

        var objectId1 = identifier.IdentifyObject(1, detection1);
        var objectId2 = identifier.IdentifyObject(2, detection2);

        Assert.Equal(objectId1, objectId2); // Overlapping objects should have the same ID
    }

    [Fact]
    public void IdentifyObject_ShouldCreateNewObject_WhenNoOverlapExistsWithinFrameThreshold()
    {
        var identifier = new ObjectIdentifier(0.01f, 3);
        var detection1 = new Detection { X = 0.1f, Y = 0.2f, Width = 0.05f, Height = 0.05f };
        var detection2 = new Detection { X = 0.5f, Y = 0.5f, Width = 0.1f, Height = 0.1f };

        var objectId1 = identifier.IdentifyObject(1, detection1);
        var objectId2 = identifier.IdentifyObject(2, detection2);

        Assert.NotEqual(objectId1, objectId2); // Non-overlapping objects should have different IDs
    }

    [Fact]
    public void IdentifyObject_ShouldEvictObject_WhenFrameDifferenceExceedsThreshold()
    {
        var identifier = new ObjectIdentifier(0.01f, 3);
        var detection1 = new Detection { X = 0.1f, Y = 0.2f, Width = 0.05f, Height = 0.05f };

        var objectId1 = identifier.IdentifyObject(1, detection1);
        var objectId2 = identifier.IdentifyObject(5, detection1);

        Assert.NotEqual(objectId1, objectId2); // Object should be evicted due to frame threshold
    }

    [Fact]
    public void IdentifyObject_ShouldHandleMultipleSizeBuckets()
    {
        var identifier = new ObjectIdentifier(0.01f, 3);
        var detection1 = new Detection { X = 0.1f, Y = 0.2f, Width = 0.05f, Height = 0.05f };
        var detection2 = new Detection { X = 0.3f, Y = 0.4f, Width = 0.1f, Height = 0.1f };

        var objectId1 = identifier.IdentifyObject(1, detection1);
        var objectId2 = identifier.IdentifyObject(1, detection2);

        Assert.NotEqual(objectId1, objectId2); // Objects in different size buckets should have different IDs
    }
    
   
}
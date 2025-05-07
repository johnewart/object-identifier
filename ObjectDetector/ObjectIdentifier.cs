namespace ObjectDetector;

using System.Collections.Generic;

public class ObjectIdentifier
{
    private readonly int _frameThreshold;
    // Use detection size to create a map
    private readonly Dictionary<int, List<Detection>> _objects = new();
    private readonly Dictionary<int, int> _lastSeenFrameMap = new(); // objectId -> last seen frame
    private int _objectIdCounter = 0;
    private readonly float _sizeTolerance;
    private readonly float _distanceTolerance;

    public ObjectIdentifier(float sizeTolerance, int frameThreshold, float distanceTolerance = 0.05f)
    {
        _frameThreshold = frameThreshold;
        _sizeTolerance = sizeTolerance;
        _distanceTolerance = distanceTolerance;
    }

    public int IdentifyObject(int frameId, Detection d)
    {
        // convert width and height plus tolerance to a bucket
        var sizeBucket = (int)((d.Width * 100) * (d.Height * 100) / (_sizeTolerance * 100));
        var objectId = -1;
        
        // Check if an object with the same rough size and location is already in the map
        if (_objects.TryGetValue(sizeBucket, out var detections))
        {
            foreach (var detection in detections)
            {
                if (IsOverlapping(d, detection) || IsInGeneralVicinity(d, detection))
                {
                    // Check if the object was seen in the last _frameThreshold frames
                    if (_lastSeenFrameMap.TryGetValue(detection.Id ?? -1, out var lastSeenFrame) &&
                        frameId - lastSeenFrame < _frameThreshold)
                    {
                        objectId = detection.Id ?? -1;
                        _lastSeenFrameMap[objectId] = frameId;
                        
                        // Update the detection with the new frame ID, location, and size
                        detection.X = d.X;
                        detection.Y = d.Y;
                        detection.Width = d.Width;
                        detection.Height = d.Height;
                        
                        return objectId;
                    } else {
                        // Evict the object if it hasn't been seen for too long
                        _lastSeenFrameMap.Remove(detection.Id ?? -1);
                        detections.Remove(detection);
                        break;
                    }
                }
            }
        }
        
        // If no existing object is found, create a new one
        objectId = _objectIdCounter++;
        _lastSeenFrameMap[objectId] = frameId;
        if (!_objects.ContainsKey(sizeBucket))
        {
            _objects[sizeBucket] = new List<Detection>();
        }
        _objects[sizeBucket].Add(d with { Id = objectId });
        return objectId;
    }

    private bool IsInGeneralVicinity(Detection detection, Detection detection1)
    {
        // Check if the two detections are within a certain distance of each other
        return (Math.Abs(detection.X - detection1.X) < _distanceTolerance &&
                Math.Abs(detection.Y - detection1.Y) < _distanceTolerance);
    }

    private bool IsOverlapping(Detection d1, Detection d2)
    {
        // Check for overlap using axis-aligned bounding box (AABB) collision detection
        return !(
            d1.X + d1.Width < d2.X || d1.X > d2.X + d2.Width ||
            d1.Y + d1.Height < d2.Y || d1.Y > d2.Y + d2.Height);
    }
}
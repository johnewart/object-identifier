# Agricultural Robotics Take-Home Challenge: Crop Tracking System

## Implementation

There are two primary components to this implementation:
* FrameProcessor: This class is responsible for processing the frames from the camera, it handles
  the reading of frames, generation of output and visualization of the results.
* ObjectIdentifier: This class is responsible for identifying the objects in the frames, it handles
  the identification of objects in frames over time and has three primary knobs:
    * Frame threshold: The number of frames after which an object is dropped if it is not seen
    * Distance tolerance: The 'fuzzy' factor between two objects to be considered the same 
    * Size tolerance: The 'fuzzy' factor between two objects to be considered the same based on their size


## Object identification

The TL;DR is that the object identifier uses hashing to put detected objects into buckets based on size, then it looks 
to see if anything is overlapping in that bucket or considered to be "close" (as defined by the distance tolerance).

Effectively, it maintains a list of objects that are currently being tracked, and for each new object, it checks if it is close to any of the
existing objects. If it is, it updates the existing object with the new information. If it is not, it adds the new object to the list of tracked objects.
During this, if the identifier believes two objects are the same, it looks to see how old the existing object is
and will evict the older object if it has surpassed the frame threshold.

## Usage

1. Clone the repository
2. `make image` from inside `ObjectIdentifier`
3. Run using `docker run -v $(pwd):/data tracking-solution --input /data/input/run01.json --output /data/output/run01-output.json --vis-dir /data/visualization`
4. There will be an `index.html` and PNG files in the `visualization` directory - the HTML file shows the 
   visualization of the frames and the PNG files are the individual frames with the objects drawn on them and identified. 

## Possible improvements

The identifier could possibly take into account the vector of an object over a series of frames rather than
just "are you close enough and about the right size"? (Computationally more expensive but potentially more accurate) 
  
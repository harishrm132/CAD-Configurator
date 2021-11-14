namespace SolidworksCore
{
    public struct Planes
    {
        public const string Front = "Front";
        public const string Top = "Top";
        public const string Right = "Right";
    }

    public struct FeatureType
    {
        public const string Plane = "PLANE";
        public const string Sketch = "SKETCH";
        public const string SketchSegment = "EXTSKETCHSEGMENT";
        public const string Face = "FACE";
        public const string Edge = "EDGE";

        public const string Component = "COMPONENT";

        public const string DrawingView = "DRAWINGVIEW";
    }
}

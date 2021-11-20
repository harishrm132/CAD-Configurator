namespace SolidworksCore
{
    public struct Planes
    {
        public const string Front = "Front";
        public const string Top = "Top";
        public const string Right = "Right";
    }

    public struct SwFileExtension
    {
        public const string Part = "SLDPRT";
        public const string Assembly = "SLDASM";
        public const string Drawing = "SLDDRW";
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

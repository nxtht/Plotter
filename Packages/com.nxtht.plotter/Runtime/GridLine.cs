using UnityEngine;

namespace Nxtht.Plotter
{
    public struct GridLine
    {
        public int Position;
        public long Significand;
        public decimal Value;
        public Vector2Int From;
        public Vector2Int To;
    }

    public struct DataLine
    {
        public Vector2Int From;
        public Vector2Int To;
    }

    [System.Serializable]
    public struct LineStyle
    {
        public int Width;
        public Color Color;
    }

    public struct LineSegment
    {
        public Vector2 From;
        public Vector2 To;
    }
}
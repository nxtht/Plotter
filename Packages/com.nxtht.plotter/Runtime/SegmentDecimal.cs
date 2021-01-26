using System.Xml;

namespace Nxtht.Plotter
{
    public struct SegmentDecimal
    {
        public Vector2Decimal Start { get; set; }
        public Vector2Decimal End { get; set; }

        public decimal X1
        {
            get => Start.X;
        }

        public decimal X2
        {
            get => End.X;
        }

        public decimal Y1
        {
            get => Start.Y;
        }

        public decimal Y2
        {
            get => End.Y;
        }

        public SegmentDecimal(Vector2Decimal start, Vector2Decimal end)
        {
            Start = start;
            End = end;
        }

        public SegmentDecimal(decimal x1, decimal y1, decimal x2, decimal y2)
            : this(new Vector2Decimal(x1, y1), new Vector2Decimal(x2, y2))
        {
        }

        public override string ToString()
        {
            return $"({X1}, {Y1}), ({X2}, {Y2})";
        }
    }
}
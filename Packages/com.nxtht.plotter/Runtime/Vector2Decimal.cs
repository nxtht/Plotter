using System;

namespace Nxtht.Plotter
{
    public struct Vector2Decimal : IEquatable<Vector2Decimal>
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public Vector2Decimal(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public void Set(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is Vector2Decimal other && this.Equals(other);

        public bool Equals(Vector2Decimal other) => this.X == other.X && this.Y == other.Y;
    }
}
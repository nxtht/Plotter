using System;

namespace Nxtht.Plotter
{
    public class GridUtils
    {
        private static readonly long[] PowBase10Cache =
        {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
        };

        public static int MaxAbsExponent => PowBase10Cache.Length - 1;

        public static decimal GetStartByUnit(decimal start, decimal unit)
        {
            return (Math.Floor(start / unit)) * unit;
        }

        public static decimal GetStartByExponent(decimal start, int exponent)
        {
            return GetStartByUnit(start, DecimalPowBase10(exponent));
        }

        public static decimal DecimalPowBase10(int exponent)
        {
            if (Math.Abs(exponent) > MaxAbsExponent)
            {
                throw new ArgumentOutOfRangeException(nameof(exponent));
            }

            if (exponent < 0)
            {
                return 1m / PowBase10Cache[-exponent];
            }

            return PowBase10Cache[exponent];
        }

        public static bool TryClipLiangBarsky(RectDecimal rect, SegmentDecimal segment, out SegmentDecimal result)
        {
            result = new SegmentDecimal();

            // defining variables
            var p1 = -(segment.X2 - segment.X1);
            var p2 = -p1;
            var p3 = -(segment.Y2 - segment.Y1);
            var p4 = -p3;

            var q1 = segment.X1 - rect.X;
            var q2 = rect.XMax - segment.X1;
            var q3 = segment.Y1 - rect.Y;
            var q4 = rect.YMax - segment.Y1;

            var posArr = new decimal[5];
            var negArr = new decimal[5];
            var posInd = 1;
            var negInd = 1;
            posArr[0] = 1;
            negArr[0] = 0;

            if ((p1 == 0 && q1 < 0) || (p2 == 0 && q2 < 0) || (p3 == 0 && q3 < 0) || (p4 == 0 && q4 < 0))
            {
                // Line is parallel to clipping window!
                return false;
            }

            if (p1 != 0)
            {
                var r1 = q1 / p1;
                var r2 = q2 / p2;
                if (p1 < 0)
                {
                    negArr[negInd++] = r1; // for negative p1, add it to negative array
                    posArr[posInd++] = r2; // and add p2 to positive array
                }
                else
                {
                    negArr[negInd++] = r2;
                    posArr[posInd++] = r1;
                }
            }

            if (p3 != 0)
            {
                var r3 = q3 / p3;
                var r4 = q4 / p4;
                if (p3 < 0)
                {
                    negArr[negInd++] = r3;
                    posArr[posInd++] = r4;
                }
                else
                {
                    negArr[negInd++] = r4;
                    posArr[posInd++] = r3;
                }
            }

            var rn1 = MaxI(negArr, negInd);
            var rn2 = MinI(posArr, posInd);

            if (rn1 > rn2)
            {
                // reject
                // Line is outside the clipping window!
                return false;
            }

            var xn1 = segment.X1 + p2 * rn1;
            var yn1 = segment.Y1 + p4 * rn1;

            var xn2 = segment.X1 + p2 * rn2;
            var yn2 = segment.Y1 + p4 * rn2;
            
            result = new SegmentDecimal(
                new Vector2Decimal(xn1, yn1),
                new Vector2Decimal(xn2, yn2)
            );
            return true;
        }

        // this function gives the maximum
        private static decimal MaxI(decimal[] arr, int n)
        {
            decimal m = 0;
            for (var i = 0; i < n; ++i)
                if (m < arr[i])
                    m = arr[i];
            return m;
        }

        // this function gives the minimum
        private static decimal MinI(decimal[] arr, int n)
        {
            decimal m = 1;
            for (var i = 0; i < n; ++i)
                if (m > arr[i])
                    m = arr[i];
            return m;
        }
    }
}
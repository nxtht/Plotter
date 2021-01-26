using System.Collections;
using System.Xml;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Nxtht.Plotter
{
    public class GridUtilsTests
    {
        [TestCaseSource(typeof(TestData), nameof(TestData.Q1FromStartCases))]
        public void TryClipLiangBarsky_CollinearSegmentsOutsideCorrect_Q1FromStart(
            decimal x, decimal y, decimal xMax, decimal yMax,
            decimal x1, decimal y1, decimal x2, decimal y2
        )
        {
            var actual = GridUtils.TryClipLiangBarsky(
                new RectDecimal(x, y, xMax - x, yMax - y),
                new SegmentDecimal(x1, y1, x2, y2),
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CollinearSegmentsOutsideCorrect_Q1_Cases))]
        public void TryClipLiangBarsky_CollinearSegmentsOutsideCorrect_Q1(
            decimal x, decimal y, decimal xMax, decimal yMax,
            decimal x1, decimal y1, decimal x2, decimal y2
        )
        {
            var actual = GridUtils.TryClipLiangBarsky(
                new RectDecimal(x, y, xMax - x, yMax - y),
                new SegmentDecimal(x1, y1, x2, y2),
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CollinearSegmentsOutsideCorrect_Q2_Cases))]
        public void TryClipLiangBarsky_CollinearSegmentsOutsideCorrect_Q2(
            decimal x, decimal y, decimal xMax, decimal yMax,
            decimal x1, decimal y1, decimal x2, decimal y2
        )
        {
            var actual = GridUtils.TryClipLiangBarsky(
                new RectDecimal(x, y, xMax - x, yMax - y),
                new SegmentDecimal(x1, y1, x2, y2),
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }


        [TestCaseSource(typeof(TestData), nameof(TestData.CollinearSegmentsOutsideCorrect_Q3_Cases))]
        public void TryClipLiangBarsky_CollinearSegmentsOutsideCorrect_Q3(
            decimal x, decimal y, decimal xMax, decimal yMax,
            decimal x1, decimal y1, decimal x2, decimal y2
        )
        {
            var actual = GridUtils.TryClipLiangBarsky(
                new RectDecimal(x, y, xMax - x, yMax - y),
                new SegmentDecimal(x1, y1, x2, y2),
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }


        [TestCaseSource(typeof(TestData), nameof(TestData.CollinearSegmentsOutsideCorrect_Q4_Cases))]
        public void TryClipLiangBarsky_CollinearSegmentsOutsideCorrect_Q4(
            decimal x, decimal y, decimal xMax, decimal yMax,
            decimal x1, decimal y1, decimal x2, decimal y2
        )
        {
            var actual = GridUtils.TryClipLiangBarsky(
                new RectDecimal(x, y, xMax - x, yMax - y),
                new SegmentDecimal(x1, y1, x2, y2),
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.ParallelSegmentsOutsideCorrect_Q1_Cases))]
        [TestCaseSource(typeof(TestData), nameof(TestData.ParallelSegmentsOutsideCorrect_Q2_Cases))]
        [TestCaseSource(typeof(TestData), nameof(TestData.ParallelSegmentsOutsideCorrect_Q3_Cases))]
        [TestCaseSource(typeof(TestData), nameof(TestData.ParallelSegmentsOutsideCorrect_Q4_Cases))]
        public void TryClipLiangBarsky_ParallelSegmentsOutsideCorrect(
            decimal x, decimal y, decimal xMax, decimal yMax,
            decimal x1, decimal y1, decimal x2, decimal y2
        )
        {
            var actual = GridUtils.TryClipLiangBarsky(
                new RectDecimal(x, y, xMax - x, yMax - y),
                new SegmentDecimal(x1, y1, x2, y2),
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.CollinearSegmentsInsideCorrect_Q1_Cases))]
        public void TryClipLiangBarsky_CollinearSegmentsInsideCorrect_Q1(
            RectDecimal rect, SegmentDecimal seg1, SegmentDecimal expected, decimal eps)
        {
            var actual = GridUtils.TryClipLiangBarsky(
                rect,
                seg1,
                out var clippedSegment);

            Assert.That(actual, Is.True);
            Assert.That(clippedSegment.X1, Is.EqualTo(expected.X1).Within(eps));
            Assert.That(clippedSegment.Y1, Is.EqualTo(expected.Y1).Within(eps));
            Assert.That(clippedSegment.X2, Is.EqualTo(expected.X2).Within(eps));
            Assert.That(clippedSegment.Y2, Is.EqualTo(expected.Y2).Within(eps));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.SegmentsCrossCorners_Q1_Cases))]
        public void TryClipLiangBarsky_SegmentsCrossCorners_Q1(
            RectDecimal rect, SegmentDecimal seg1, SegmentDecimal expected, decimal eps)
        {
            var actual = GridUtils.TryClipLiangBarsky(
                rect,
                seg1,
                out var clippedSegment);

            Assert.That(actual, Is.True);
            Assert.That(clippedSegment.X1, Is.EqualTo(expected.X1).Within(eps));
            Assert.That(clippedSegment.Y1, Is.EqualTo(expected.Y1).Within(eps));
            Assert.That(clippedSegment.X2, Is.EqualTo(expected.X2).Within(eps));
            Assert.That(clippedSegment.Y2, Is.EqualTo(expected.Y2).Within(eps));
        }
        
        [TestCaseSource(typeof(TestData), nameof(TestData.SegmentsOutsideCorners_Q1_Cases))]
        public void TryClipLiangBarsky_SegmentsOutsideCorners_Q1(
            RectDecimal rect, SegmentDecimal seg1)
        {
            var actual = GridUtils.TryClipLiangBarsky(
                rect,
                seg1,
                out var clippedSegment);

            Assert.That(actual, Is.False);
        }
    }

    public class TestData
    {
        private const decimal XMinQ1 = 10;
        private const decimal YMinQ1 = 20;
        private const decimal XMaxQ1 = 1010;
        private const decimal YMaxQ1 = 520;

        private const decimal XMinQ2 = -XMaxQ1;
        private const decimal YMinQ2 = YMinQ1;
        private const decimal XMaxQ2 = -XMinQ1;
        private const decimal YMaxQ2 = YMaxQ1;

        private const decimal XMinQ3 = -XMaxQ1;
        private const decimal YMinQ3 = -YMaxQ1;
        private const decimal XMaxQ3 = -XMinQ1;
        private const decimal YMaxQ3 = -YMinQ1;

        private const decimal XMinQ4 = XMinQ1;
        private const decimal YMinQ4 = -YMaxQ1;
        private const decimal XMaxQ4 = XMaxQ1;
        private const decimal YMaxQ4 = -YMinQ1;
        private const decimal Epsilon = 1e-22m;


        private const decimal Width = 1000;
        private const decimal Height = 500;

        public static IEnumerable Q1FromStartCases
        {
            get
            {
                yield return new TestCaseData(0m, 0m, XMaxQ1, YMaxQ1, XMinQ1 - 50, 0m, XMinQ1 - 20, 0m)
                    .SetName("LowerLine Left");
                yield return new TestCaseData(0m, 0m, XMaxQ1, YMaxQ1, XMaxQ1 + 20, 0m, XMaxQ1 + 50, 0m)
                    .SetName("LowerLine Right");
                yield return new TestCaseData(0m, 0m, XMaxQ1, YMaxQ1, XMinQ1 - 50, YMaxQ1, XMinQ1 - 20, YMaxQ1)
                    .SetName("UpperLine Left");
                yield return new TestCaseData(0m, 0m, XMaxQ1, YMaxQ1, XMaxQ1 + 20, YMaxQ1, XMaxQ1 + 50, YMaxQ1)
                    .SetName("UpperLine Right");
            }
        }

        public static IEnumerable CollinearSegmentsOutsideCorrect_Q1_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1 - 50, YMinQ1, XMinQ1 - 20, YMinQ1)
                    .SetName("LowerLine Left");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMaxQ1 + 20, YMinQ1, XMaxQ1 + 50, YMinQ1)
                    .SetName("LowerLine Right");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1 - 50, YMaxQ1, XMinQ1 - 20, YMaxQ1)
                    .SetName("UpperLine Left");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMaxQ1 + 20, YMaxQ1, XMaxQ1 + 50, YMaxQ1)
                    .SetName("UpperLine Right");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1, YMinQ1 - 50, XMinQ1, YMinQ1 - 20)
                    .SetName("LeftLine Below");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1, YMaxQ1 + 20, XMinQ1, YMaxQ1 + 50)
                    .SetName("LeftLine Above");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMaxQ1, YMinQ1 - 50, XMaxQ1, YMinQ1 - 20)
                    .SetName("RightLine Below");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMaxQ1, YMaxQ1 + 20, XMaxQ1, YMaxQ1 + 50)
                    .SetName("RightLine Above");
            }
        }

        public static IEnumerable CollinearSegmentsOutsideCorrect_Q2_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2 - 50, YMinQ2, XMinQ2 - 20, YMinQ2)
                    .SetName("LowerLine Left");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMaxQ2 + 20, YMinQ2, XMaxQ2 + 50, YMinQ2)
                    .SetName("LowerLine Right");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2 - 50, YMaxQ2, XMinQ2 - 20, YMaxQ2)
                    .SetName("UpperLine Left");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMaxQ2 + 20, YMaxQ2, XMaxQ2 + 50, YMaxQ2)
                    .SetName("UpperLine Right");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2, YMinQ2 - 50, XMinQ2, YMinQ2 - 20)
                    .SetName("LeftLine Below");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2, YMaxQ2 + 20, XMinQ2, YMaxQ2 + 50)
                    .SetName("LeftLine Above");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMaxQ2, YMinQ2 - 50, XMaxQ2, YMinQ2 - 20)
                    .SetName("RightLine Below");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMaxQ2, YMaxQ2 + 20, XMaxQ2, YMaxQ2 + 50)
                    .SetName("RightLine Above");
            }
        }

        public static IEnumerable CollinearSegmentsOutsideCorrect_Q3_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3 - 50, YMinQ3, XMinQ3 - 20, YMinQ3)
                    .SetName("LowerLine Left");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMaxQ3 + 20, YMinQ3, XMaxQ3 + 50, YMinQ3)
                    .SetName("LowerLine Right");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3 - 50, YMaxQ3, XMinQ3 - 20, YMaxQ3)
                    .SetName("UpperLine Left");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMaxQ3 + 20, YMaxQ3, XMaxQ3 + 50, YMaxQ3)
                    .SetName("UpperLine Right");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3, YMinQ3 - 50, XMinQ3, YMinQ3 - 20)
                    .SetName("LeftLine Below");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3, YMaxQ3 + 20, XMinQ3, YMaxQ3 + 50)
                    .SetName("LeftLine Above");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMaxQ3, YMinQ3 - 50, XMaxQ3, YMinQ3 - 20)
                    .SetName("RightLine Below");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMaxQ3, YMaxQ3 + 20, XMaxQ3, YMaxQ3 + 50)
                    .SetName("RightLine Above");
            }
        }

        public static IEnumerable CollinearSegmentsOutsideCorrect_Q4_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4 - 50, YMinQ4, XMinQ4 - 20, YMinQ4)
                    .SetName("LowerLine Left");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMaxQ4 + 20, YMinQ4, XMaxQ4 + 50, YMinQ4)
                    .SetName("LowerLine Right");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4 - 50, YMaxQ4, XMinQ4 - 20, YMaxQ4)
                    .SetName("UpperLine Left");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMaxQ4 + 20, YMaxQ4, XMaxQ4 + 50, YMaxQ4)
                    .SetName("UpperLine Right");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4, YMinQ4 - 50, XMinQ4, YMinQ4 - 20)
                    .SetName("LeftLine Below");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4, YMaxQ4 + 20, XMinQ4, YMaxQ4 + 50)
                    .SetName("LeftLine Above");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMaxQ4, YMinQ4 - 50, XMaxQ4, YMinQ4 - 20)
                    .SetName("RightLine Below");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMaxQ4, YMaxQ4 + 20, XMaxQ4, YMaxQ4 + 50)
                    .SetName("RightLine Above");
            }
        }

        public static IEnumerable ParallelSegmentsOutsideCorrect_Q1_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1, YMinQ1 - 10, XMaxQ1, YMinQ1 - 10)
                    .SetName("Q1 Horiz Below Rect");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1, YMaxQ1 + 10, XMaxQ1, YMaxQ1 + 10)
                    .SetName("Q1 Horiz Above Rect");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMinQ1 - 10, YMinQ1, XMinQ1 - 10, YMaxQ1)
                    .SetName("Q1 Vert to the Left");
                yield return new TestCaseData(XMinQ1, YMinQ1, XMaxQ1, YMaxQ1, XMaxQ1 + 10, YMinQ1, XMaxQ1 + 10, YMaxQ1)
                    .SetName("Q1 Vert to the Right");
            }
        }

        public static IEnumerable ParallelSegmentsOutsideCorrect_Q2_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2, YMinQ2 - 10, XMaxQ2, YMinQ2 - 10)
                    .SetName("Q2 Horiz Below Rect");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2, YMaxQ2 + 10, XMaxQ2, YMaxQ2 + 10)
                    .SetName("Q2 Horiz Above Rect");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMinQ2 - 10, YMinQ2, XMinQ2 - 10, YMaxQ2)
                    .SetName("Q2 Vert to the Left");
                yield return new TestCaseData(XMinQ2, YMinQ2, XMaxQ2, YMaxQ2, XMaxQ2 + 10, YMinQ2, XMaxQ2 + 10, YMaxQ2)
                    .SetName("Q2 Vert to the Right");
            }
        }

        public static IEnumerable ParallelSegmentsOutsideCorrect_Q3_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3, YMinQ3 - 10, XMaxQ3, YMinQ3 - 10)
                    .SetName("Q3 Horiz Below Rect");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3, YMaxQ3 + 10, XMaxQ3, YMaxQ3 + 10)
                    .SetName("Q3 Horiz Above Rect");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMinQ3 - 10, YMinQ3, XMinQ3 - 10, YMaxQ3)
                    .SetName("Q3 Vert to the Left");
                yield return new TestCaseData(XMinQ3, YMinQ3, XMaxQ3, YMaxQ3, XMaxQ3 + 10, YMinQ3, XMaxQ3 + 10, YMaxQ3)
                    .SetName("Q3 Vert to the Right");
            }
        }

        public static IEnumerable ParallelSegmentsOutsideCorrect_Q4_Cases
        {
            get
            {
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4, YMinQ4 - 10, XMaxQ4, YMinQ4 - 10)
                    .SetName("Q4 Horiz Below Rect");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4, YMaxQ4 + 10, XMaxQ4, YMaxQ4 + 10)
                    .SetName("Q4 Horiz Above Rect");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMinQ4 - 10, YMinQ4, XMinQ4 - 10, YMaxQ4)
                    .SetName("Q4 Vert to the Left");
                yield return new TestCaseData(XMinQ4, YMinQ4, XMaxQ4, YMaxQ4, XMaxQ4 + 10, YMinQ4, XMaxQ4 + 10, YMaxQ4)
                    .SetName("Q4 Vert to the Right");
            }
        }

        public static IEnumerable CollinearSegmentsInsideCorrect_Q1_Cases
        {
            get
            {
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1, YMinQ1, XMaxQ1, YMinQ1),
                        new SegmentDecimal(XMinQ1, YMinQ1, XMaxQ1, YMinQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg = LowerLine");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1, YMaxQ1, XMaxQ1, YMaxQ1),
                        new SegmentDecimal(XMinQ1, YMaxQ1, XMaxQ1, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg = UpperLine");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1, YMinQ1, XMinQ1, YMaxQ1),
                        new SegmentDecimal(XMinQ1, YMinQ1, XMinQ1, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg = LeftLine");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMaxQ1, YMinQ1, XMaxQ1, YMaxQ1),
                        new SegmentDecimal(XMaxQ1, YMinQ1, XMaxQ1, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg = RightLine");

                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1 - 10, YMinQ1, XMaxQ1 + 10, YMinQ1),
                        new SegmentDecimal(XMinQ1, YMinQ1, XMaxQ1, YMinQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg around LowerLine");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1 - 10, YMaxQ1, XMaxQ1 + 10, YMaxQ1),
                        new SegmentDecimal(XMinQ1, YMaxQ1, XMaxQ1, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg around UpperLine");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1, YMinQ1 - 10, XMinQ1, YMaxQ1 + 10),
                        new SegmentDecimal(XMinQ1, YMinQ1, XMinQ1, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg around LeftLine");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMaxQ1, YMinQ1 - 10, XMaxQ1, YMaxQ1 + 10),
                        new SegmentDecimal(XMaxQ1, YMinQ1, XMaxQ1, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg around RightLine");
            }
        }

        public static IEnumerable SegmentsCrossCorners_Q1_Cases
        {
            get
            {
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1 - 10m, YMinQ1 + 20m, XMinQ1 + 20m, YMinQ1 - 10m),
                        new SegmentDecimal(XMinQ1, YMinQ1 + 10m, XMinQ1 + 10m, YMinQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg crosses LowerLeft");

                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMaxQ1 - 20m, YMinQ1 - 10m, XMaxQ1 + 10m, YMinQ1 + 20m),
                        new SegmentDecimal(XMaxQ1 - 10m, YMinQ1, XMaxQ1, YMinQ1 + 10m),
                        Epsilon
                    )
                    .SetName("Q1 Seg crosses LowerRight");

                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMaxQ1 - 20m, YMaxQ1 + 10m, XMaxQ1 + 10m, YMaxQ1 - 20m),
                        new SegmentDecimal(XMaxQ1 - 10m, YMaxQ1, XMaxQ1, YMaxQ1 - 10m),
                        Epsilon
                    )
                    .SetName("Q1 Seg crosses UpperRight");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1 - 10m, YMaxQ1 - 20m, XMinQ1 + 20m, YMaxQ1 + 10m),
                        new SegmentDecimal(XMinQ1, YMaxQ1 - 10m, XMinQ1 + 10m, YMaxQ1),
                        Epsilon
                    )
                    .SetName("Q1 Seg crosses UpperLeft");
            }
        }

        public static IEnumerable SegmentsOutsideCorners_Q1_Cases
        {
            get
            {
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1 - 30m, YMinQ1 + 20m, XMinQ1 + 20m, YMinQ1 - 30m)
                    )
                    .SetName("Q1 Seg out LowerLeft");

                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMaxQ1 - 20m, YMinQ1 - 30m, XMaxQ1 + 30m, YMinQ1 + 20m)
                    )
                    .SetName("Q1 Seg out LowerRight");

                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMaxQ1 - 20m, YMaxQ1 + 30m, XMaxQ1 + 30m, YMaxQ1 - 20m)
                    )
                    .SetName("Q1 Seg out UpperRight");
                yield return new TestCaseData(
                        new RectDecimal(XMinQ1, YMinQ1, XMaxQ1 - XMinQ1, YMaxQ1 - YMinQ1),
                        new SegmentDecimal(XMinQ1 - 30m, YMaxQ1 - 20m, XMinQ1 + 20m, YMaxQ1 + 30m)
                    )
                    .SetName("Q1 Seg out UpperLeft");
            }
        }
    }
}
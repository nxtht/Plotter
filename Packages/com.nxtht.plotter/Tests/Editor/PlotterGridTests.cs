using System.Collections;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

namespace Nxtht.Plotter
{
    public class PlotterGridTests
    {
        private PlotterGrid GetGrid(int width = 200, int height = 100)
        {
            return new PlotterGrid(width, height, null);
        }
        
        [TestCase(5, 5.0)]
        [TestCase(100, 100.0)]
        [TestCase(100_000_000, 100_000_000.0)]
        [TestCase(1_000_000_000, 1_000_000_000.0)]
        [TestCase(-5, -5.0)]
        [TestCase(-100, -100.0)]
        [TestCase(-100_000_000, -100_000_000.0)]
        [TestCase(-1_000_000_000, -1_000_000_000.0)]
        public void MoveStartX_FromDefault_Test(int steps, decimal expected)
        {
            var g = GetGrid();
            g.MoveStartX(steps);
            Assert.That(g.StartX, Is.EqualTo(expected).Within(1e-8));
        }

        [TestCase(5, 5.0)]
        [TestCase(100, 100.0)]
        [TestCase(100_000_000, 100_000_000.0)]
        [TestCase(1_000_000_000, 1_000_000_000.0)]
        [TestCase(-5, -5.0)]
        [TestCase(-100, -100.0)]
        [TestCase(-100_000_000, -100_000_000.0)]
        [TestCase(-1_000_000_000, -1_000_000_000.0)]
        public void MoveStartY_FromDefault_Test(int steps, decimal expected)
        {
            var g = GetGrid();
            g.MoveStartY(steps);
            Assert.That(g.StartY, Is.EqualTo(expected).Within(1e-8));
        }

        [TestCase(5, 0.5)]
        [TestCase(100, 10.0)]
        [TestCase(100_000_000, 10_000_000.0)]
        [TestCase(1_000_000_000, 100_000_000.0)]
        [TestCase(-5, -0.5)]
        [TestCase(-100, -10.0)]
        [TestCase(-100_000_000, -10_000_000.0)]
        [TestCase(-1_000_000_000, -100_000_000.0)]
        public void MoveStartX_WithScaleMinusOne_Test(int steps, decimal expected)
        {
            var g = GetGrid();
            g.ScaleUpX(10);
            g.MoveStartX(steps);
            Assert.That(g.StartX, Is.EqualTo(expected).Within(1e-8));
        }

        [TestCase(5, 0.5)]
        [TestCase(100, 10.0)]
        [TestCase(100_000_000, 10_000_000.0)]
        [TestCase(1_000_000_000, 100_000_000.0)]
        [TestCase(-5, -0.5)]
        [TestCase(-100, -10.0)]
        [TestCase(-100_000_000, -10_000_000.0)]
        [TestCase(-1_000_000_000, -100_000_000.0)]
        public void MoveStartY_WithScaleMinusOne_Test(int steps, decimal expected)
        {
            var g = GetGrid();
            g.ScaleUpY(10);
            g.MoveStartY(steps);
            Assert.That(g.StartY, Is.EqualTo(expected).Within(1e-8));
        }

        [TestCase(5, 50.0)]
        [TestCase(100, 1000.0)]
        [TestCase(100_000_000, 1_000_000_000.0)]
        [TestCase(500_000_000, 5_000_000_000.0)]
        [TestCase(-5, -50.0)]
        [TestCase(-100, -1000.0)]
        [TestCase(-100_000_000, -1_000_000_000.0)]
        [TestCase(-500_000_000, -5_000_000_000.0)]
        public void MoveStartX_WithScalePlusOne_Test(int steps, decimal expected)
        {
            var g = GetGrid();
            g.ScaleUpX(-1);
            g.MoveStartX(steps);
            Assert.That(g.StartX, Is.EqualTo(expected).Within(1e-8));
        }

        [TestCase(5, 50.0)]
        [TestCase(100, 1000.0)]
        [TestCase(100_000_000, 1_000_000_000.0)]
        [TestCase(500_000_000, 5_000_000_000.0)]
        [TestCase(-5, -50.0)]
        [TestCase(-100, -1000.0)]
        [TestCase(-100_000_000, -1_000_000_000.0)]
        [TestCase(-500_000_000, -5_000_000_000.0)]
        public void MoveStartY_WithScalePlusOne_Test(int steps, decimal expected)
        {
            var g = GetGrid();
            g.ScaleUpY(-1);
            g.MoveStartY(steps);
            Assert.That(g.StartY, Is.EqualTo(expected).Within(1e-8));
        }


        [Test]
        public void ToGridCoord_Test()
        {
            var g = GetGrid();
            var data = new Vector2Decimal(10, 10);
            Assert.That(g.ToGridCoord(data), Is.EqualTo(new UnityEngine.Vector2Int(100, 100)));
        }

        [Test]
        public void ToGridCoordPositiveStart_Test()
        {
            var g = GetGrid();
            g.MoveStartX(5);
            var data = new Vector2Decimal(10, 10);
            Assert.That(g.ToGridCoord(data), Is.EqualTo(new UnityEngine.Vector2Int(50, 100)));
        }

        [Test]
        public void ToGridCoord_Width_ScaleX_MoveStart_Test()
        {
            var g = GetGrid();
            g.ScaleUpX(-1); // PpuX = 90, ExponentX = 1, UnitCost = 10, PixelCost = 
            g.MoveStartX(-1); // StartX = -10.0
            var data = new Vector2Decimal(10, 10);
            Assert.That(g.ToGridCoord(data), Is.EqualTo(new UnityEngine.Vector2Int(180, 100)));
        }

        [TestCase(1, 20, 0, TestName = "1 step up")]
        [TestCase(8, 90, 0, TestName = "8 step up")]
        [TestCase(9, 10, -1, TestName = "9 step up")]
        [TestCase(10, 20, -1, TestName = "10 step up")]
        [TestCase(-1, 90, 1, TestName = "-1 step up")]
        [TestCase(-9, 10, 1, TestName = "-9 step up")]
        [TestCase(-10, 90, 2, TestName = "-10 step up")]
        public void ScaleUpXCorrectFromZero_Test(int scaleStep, int expectedPpu, int expectedExp)
        {
            var g = GetGrid();
            g.ScaleUpX(scaleStep);
            Assert.That(g.PpuX, Is.EqualTo(expectedPpu));
            Assert.That(g.ExponentX, Is.EqualTo(expectedExp));
        }

        [TestCase(1, 20, 0, TestName = "1 step up")]
        [TestCase(8, 90, 0, TestName = "8 step up")]
        [TestCase(9, 10, -1, TestName = "9 step up")]
        [TestCase(10, 20, -1, TestName = "10 step up")]
        [TestCase(-1, 90, 1, TestName = "-1 step up")]
        [TestCase(-9, 10, 1, TestName = "-9 step up")]
        [TestCase(-10, 90, 2, TestName = "-10 step up")]
        public void ScaleUpYCorrectFromZero_Test(int scaleStep, int expectedPpu, int expectedExp)
        {
            var g = GetGrid();
            g.ScaleUpY(scaleStep);
            Assert.That(g.PpuY, Is.EqualTo(expectedPpu));
            Assert.That(g.ExponentY, Is.EqualTo(expectedExp));
        }
    }
}
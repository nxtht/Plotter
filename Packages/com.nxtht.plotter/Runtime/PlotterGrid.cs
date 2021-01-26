using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nxtht.Plotter
{
    public class PlotterGrid : IPlotterGrid
    {
        private const long MinRange = -999_999_999_999_999_999L;
        private const long MaxRange = 999_999_999_999_999_999L;
        private const int MaxExp = 8;
        private const int MinExp = -9;
        private const int ExpBias = 9;

        private IGraphFileReader _reader;
        private Vector2Decimal[] _data;
        private readonly long[] _mult;

        private int _exponentX;
        private int _exponentY;
        private long _startXInternal;
        private long _startYInternal;

        private long StartXInternal
        {
            get => _startXInternal;
            set
            {
                if (Math.Abs(value) > MaxRange)
                    throw new ArgumentOutOfRangeException(nameof(StartXInternal));

                _startXInternal = value;
            }
        }

        private long StartYInternal
        {
            get => _startYInternal;
            set
            {
                if (Math.Abs(value) > MaxRange)
                    throw new ArgumentOutOfRangeException(nameof(StartYInternal));

                _startYInternal = value;
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int PpuX { get; set; }
        public int PpuY { get; set; }
        public int Base { get; set; }
        public bool IsDirty { get; private set; }

        public decimal StartX
        {
            get => (decimal) _startXInternal / _mult[ExpBias];
        }

        public decimal StartY
        {
            get => (decimal) _startYInternal / _mult[ExpBias];
        }

        public int ExponentX
        {
            get => _exponentX;
            set
            {
                if (value > MaxExp || value < MinExp)
                    throw new ArgumentOutOfRangeException(nameof(ExponentX));

                _exponentX = value;
                _startXInternal = (_startXInternal / _mult[ExpBias + value]) * (_mult[ExpBias + value]);
            }
        }

        public int ExponentY
        {
            get => _exponentY;
            set
            {
                if (value > MaxExp || value < MinExp)
                    throw new ArgumentOutOfRangeException(nameof(ExponentY));

                _exponentY = value;
                _startYInternal = (_startYInternal / _mult[ExpBias + value]) * (_mult[ExpBias + value]);
            }
        }

        public PlotterGrid(int width, int height, IGraphFileReader reader)
        {
            _reader = reader;
            _mult = InitMultipliers(Math.Abs(MinExp) * 2);

            Width = width;
            Height = height;
            StartXInternal = 0;
            StartYInternal = 0;
            Base = 10;
            PpuX = Base;
            PpuY = Base;
            ExponentX = 0;
            ExponentY = 0;
        }

        public IEnumerable<GridLine> GetHorizontalUnitLinesByModulo(int m)
        {
            return GetHorizontalUnitLines().Where(u => u.Significand % m == 0);
        }

        private IEnumerable<GridLine> GetHorizontalUnitLines()
        {
            var start = StartYInternal;
            var exp = ExponentY;
            var ppu = PpuY;

            var significand = start / _mult[ExpBias + exp];
            for (int i = 0; i < Height; i += ppu, significand++)
            {
                decimal value = exp < 0
                    ? significand / (decimal) _mult[-exp]
                    : significand * _mult[exp];
                yield return new GridLine
                {
                    Position = i,
                    Significand = significand,
                    Value = value,
                    From = new Vector2Int(0, i),
                    To = new Vector2Int(Width - 1, i)
                };
            }
        }

        public IEnumerable<GridLine> GetVerticalUnitLinesByModulo(int m)
        {
            return GetVerticalUnitLines().Where(u => u.Significand % m == 0);
        }

        private IEnumerable<GridLine> GetVerticalUnitLines()
        {
            var start = StartXInternal;
            var exp = ExponentX;
            var ppu = PpuX;

            var significand = start / _mult[ExpBias + exp];
            for (var i = 0; i < Width; i += ppu, significand++)
            {
                decimal value = exp < 0
                    ? significand / (decimal) _mult[-exp]
                    : significand * _mult[exp];

                yield return new GridLine
                {
                    Position = i,
                    Significand = significand,
                    Value = value,
                    From = new Vector2Int(i, 0),
                    To = new Vector2Int(i, Height - 1)
                };
            }
        }

        public void ScaleUpX(int move)
        {
            if (move < 0)
            {
                while (move < 0)
                {
                    DecrementScaleX();
                    move++;
                }
            }
            else if (move > 0)
            {
                while (move > 0)
                {
                    IncrementScaleX();
                    move--;
                }
            }
        }

        public void ScaleUpY(int move)
        {
            if (move < 0)
            {
                while (move < 0)
                {
                    DecrementScaleY();
                    move++;
                }
            }
            else if (move > 0)
            {
                while (move > 0)
                {
                    IncrementScaleY();
                    move--;
                }
            }
        }

        private void IncrementScaleX()
        {
            if (ExponentX == MinExp && PpuX == 90)
                return;

            PpuX += 10;
            if (PpuX == 100)
            {
                PpuX = 10;
                ExponentX--;
            }
        }

        private void IncrementScaleY()
        {
            if (ExponentY == MinExp && PpuY == 90)
                return;

            PpuY += 10;
            if (PpuY == 100)
            {
                PpuY = 10;
                ExponentY--;
            }
        }

        private void DecrementScaleX()
        {
            if (ExponentX == MaxExp && PpuX == 10)
                return;

            if (PpuX == 10)
            {
                PpuX = 100;
                ExponentX++;
            }

            PpuX -= 10;
        }

        private void DecrementScaleY()
        {
            if (ExponentY == MaxExp && PpuY == 10)
                return;

            if (PpuY == 10)
            {
                PpuY = 100;
                ExponentY++;
            }

            PpuY -= 10;
        }

        public void MoveStartX(int move)
        {
            _startXInternal += _mult[ExponentX + ExpBias] * move;
        }

        public void MoveStartY(int move)
        {
            _startYInternal += _mult[ExponentY + ExpBias] * move;
        }

        public Vector2Int ToGridCoord(Vector2Decimal point)
        {
            var x = (point.X - StartX) * PpuX;
            if (_exponentX < 0)
            {
                x *= _mult[-_exponentX];
            }
            else
            {
                x /= _mult[_exponentX];
            }

            var y = (point.Y - StartY) * PpuY;
            if (_exponentY < 0)
            {
                y *= _mult[-_exponentY];
            }
            else
            {
                y /= _mult[_exponentY];
            }

            return new Vector2Int((int) x, (int) y);
        }

        public void ClearData()
        {
            _data = null;
        }

        public void LoadDataFromFile(string path)
        {
            _data = _reader.ReadFile(path);
        }

        public bool HasData()
        {
            return (_data != null && _data.Length > 0);
        }

        public IEnumerable<DataLine> GetDataLines()
        {
            var startIndex = -1;
            var start = StartX;
            for (var i = 0; i < _data.Length; i++)
            {
                if (_data[i].X < start)
                    continue;

                startIndex = i;
                break;
            }

            if (startIndex < 0)
            {
                yield break;
            }

            if (startIndex > 0)
            {
                startIndex--;
            }

            for (int i = startIndex; i < _data.Length - 1; i++)
            {
                yield return new DataLine
                {
                    From = ToGridCoord(_data[i]),
                    To = ToGridCoord(_data[i + 1])
                };
            }
        }

        private long[] InitMultipliers(int n)
        {
            var result = new long[n];
            result[0] = 1;
            for (var i = 1; i < n; i++)
            {
                result[i] = result[i - 1] * 10;
            }

            return result;
        }
    }
}
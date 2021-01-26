using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nxtht.Plotter
{
    public class DecimalPlotterGrid : IPlotterGrid
    {
        private const int MaxExp = 8;
        private const int MinExp = -9;

        private IGraphFileReader _reader;
        private Vector2Decimal[] _data;
        private int _exponentX;
        private int _exponentY;
        private decimal _unitX;
        private decimal _unitY;
        private decimal _startX;
        private decimal _startY;

        public int Width { get; set; }
        public int Height { get; set; }
        public int PpuX { get; set; }
        public int PpuY { get; set; }
        public int Base { get; set; }
        public bool IsDirty { get; private set; }

        public decimal StartX
        {
            get => _startX;
            set => _startX = value;
        }

        public decimal StartY
        {
            get => _startY;
            set => _startY = value;
        }

        public decimal UnitX => _unitX;

        public decimal UnitY => _unitY;

        public int ExponentX
        {
            get => _exponentX;
            set
            {
                if (value > MaxExp || value < MinExp)
                    throw new ArgumentOutOfRangeException(nameof(ExponentX));

                _exponentX = value;
                _startX = GridUtils.GetStartByExponent(_startX, _exponentX);
                _unitX = GridUtils.DecimalPowBase10(_exponentX);
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
                _startY = GridUtils.GetStartByExponent(_startY, _exponentY);
                _unitY = GridUtils.DecimalPowBase10(_exponentY);
            }
        }

        public DecimalPlotterGrid(int width, int height, IGraphFileReader reader)
        {
            _reader = reader;

            Width = width;
            Height = height;
            StartX = 0m;
            StartY = 0m;
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

        public IEnumerable<GridLine> GetVerticalUnitLinesByModulo(int m)
        {
            return GetVerticalUnitLines().Where(u => u.Significand % m == 0);
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

        public void MoveStartX(int move)
        {
            StartX += _unitX * move;
        }

        public void MoveStartY(int move)
        {
            StartY += _unitY * move;
        }

        public Vector2Int ToGridCoord(Vector2Decimal point)
        {
            return new Vector2Int(
                ToGridCoord(point.X, _startX, _unitX, PpuX),
                ToGridCoord(point.Y, _startY, _unitY, PpuY));
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
            var end = _startX + Width * (_unitX / PpuX);
            var startIndex = GetStartIndexToDraw(_data);

            if (startIndex < 0)
            {
                yield break;
            }

            var rd = new RectDecimal(
                _startX,
                _startY,
                Width * (_unitX / PpuX),
                Height * (_unitY / PpuY));

            for (int i = startIndex; i < _data.Length - 1; i++)
            {
                if (GridUtils.TryClipLiangBarsky(rd, new SegmentDecimal(_data[i], _data[i + 1]), out var clipped))
                {
                    yield return new DataLine
                    {
                        From = ToGridCoord(clipped.Start),
                        To = ToGridCoord(clipped.End)
                    };
                }
            }
        }

        private IEnumerable<GridLine> GetHorizontalUnitLines()
        {
            var start = StartY;
            var ppu = PpuY;
            var size = Height;
            var unit = UnitY;

            var count = size / ppu + 1;

            for (int i = 0; i < count; i++)
            {
                var pos = i * ppu;
                var value = start + i * unit;
                yield return new GridLine
                {
                    Position = pos,
                    Value = value,
                    Significand = (long) Math.Floor(value / unit),
                    From = new Vector2Int(0, pos),
                    To = new Vector2Int(Width - 1, pos)
                };
            }
        }

        private IEnumerable<GridLine> GetVerticalUnitLines()
        {
            var start = StartX;
            var ppu = PpuX;
            var size = Width;
            var unit = UnitX;

            var count = size / ppu + 1;

            for (int i = 0; i < count; i++)
            {
                var pos = i * ppu;
                var value = start + i * unit;
                yield return new GridLine
                {
                    Position = pos,
                    Value = value,
                    Significand = (long) Math.Floor(value / unit),
                    From = new Vector2Int(pos, 0),
                    To = new Vector2Int(pos, Height - 1)
                };
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

        private int GetStartIndexToDraw(Vector2Decimal[] data)
        {
            var end = _startX + Width * (_unitX / PpuX);
            var startIndex = Array.FindIndex(_data, 0, p => !(p.X < _startX));

            if (startIndex < 0)
                return startIndex;


            if (startIndex > 0)
                startIndex--;

            if (data[startIndex].X >= end)
                return -1;

            return startIndex;
        }

        private int GetLastIndexToDraw(Vector2Decimal[] data, int startIndex)
        {
            var end = _startX + Width * (_unitX / PpuX);

            for (; startIndex < data.Length; startIndex++)
            {
                if (data[startIndex].X >= end)
                    return startIndex;
            }

            return data.Length - 1;
        }

        private int ToGridCoord(decimal v, decimal start, decimal unit, int ppu)
        {
            return (int) Math.Floor((v - start) * ppu / unit);
        }
    }
}
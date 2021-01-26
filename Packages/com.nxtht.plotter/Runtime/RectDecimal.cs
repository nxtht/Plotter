using System;
using UnityEngine;

namespace Nxtht.Plotter
{
    public struct RectDecimal
    {
        private decimal _x;
        private decimal _y;
        private decimal _xMax;
        private decimal _yMax;
        private decimal _width;
        private decimal _height;


        public decimal X
        {
            get => _x;
            set => _x = value;
        }

        public decimal Y
        {
            get => _y;
            set => _y = value;
        }

        public decimal XMax
        {
            get => _xMax;
            set => _xMax = value;
        }

        public decimal YMax
        {
            get => _yMax;
            set => _yMax = value;
        }

        public decimal Width
        {
            get => _width;
            set => _width = value;
        }

        public decimal Height
        {
            get => _height;
            set => _height = value;
        }


        public RectDecimal(decimal x, decimal y, decimal width, decimal height)
        {
            if (width < 0)
                throw new ArgumentException($"{nameof(width)} can not be negative!");

            if (height < 0)
                throw new ArgumentException($"{nameof(height)} can not be negative!");

            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _xMax = x + width;
            _yMax = y + height;
        }

        public override string ToString()
        {
            return $"X:{X}; Y:{Y}; XMax:{XMax}; YMax:{YMax}";
        }
    }
}
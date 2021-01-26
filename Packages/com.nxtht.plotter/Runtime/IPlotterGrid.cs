using System.Collections.Generic;
using UnityEngine;

namespace Nxtht.Plotter
{
    public interface IPlotterGrid
    {
        int Width { get; set; }
        int Height { get; set; }
        int PpuX { get; set; }
        int PpuY { get; set; }
        int Base { get; set; }
        bool IsDirty { get; }
        decimal StartX { get; }
        decimal StartY { get; }
        int ExponentX { get; set; }
        int ExponentY { get; set; }
        IEnumerable<GridLine> GetHorizontalUnitLinesByModulo(int m);
        IEnumerable<GridLine> GetVerticalUnitLinesByModulo(int m);
        void ScaleUpX(int move);
        void ScaleUpY(int move);
        void MoveStartX(int move);
        void MoveStartY(int move);
        Vector2Int ToGridCoord(Vector2Decimal point);
        void ClearData();
        void LoadDataFromFile(string path);
        bool HasData();
        IEnumerable<DataLine> GetDataLines();
    }
}
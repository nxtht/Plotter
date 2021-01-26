using UnityEngine;

namespace Nxtht.Plotter
{
    public interface IGraphFileReader
    {
        Vector2Decimal[] ReadFile(string path);
    }
}
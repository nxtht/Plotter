using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace Nxtht.Plotter
{
    public class GraphFileReader : IGraphFileReader
    {
        public Vector2Decimal[] ReadFile(string path)
        {
            var separator = new[] {','};
            var content = File.ReadAllLines(path);
            var result = new Vector2Decimal[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                var split = content[i].Split(separator);
                if (split.Length != 2)
                {
                    throw new SerializationException($"Invalid data at line {i}");
                }

                if (!Decimal.TryParse(split[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x)
                    || !Decimal.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
                {
                    throw new SerializationException($"Invalid data at line {i}");
                }

                result[i] = new Vector2Decimal(x, y);
            }

            return result;
        }
    }
}
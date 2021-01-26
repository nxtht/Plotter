using UnityEngine;

namespace Nxtht.Plotter.Editor
{
    public class FieldNameAttribute : PropertyAttribute
    {
        public string Name { get; }

        public FieldNameAttribute(string name)
        {
            Name = name;
        }
    }
}
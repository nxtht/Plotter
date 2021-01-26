using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Nxtht.Plotter
{
    [FilePath("ProjectSettings/Nxtht/PlotterSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class PlotterSettings : ScriptableSingleton<PlotterSettings>
    {
        public int LeftBorder = 100;
        public int RightBorder = 40;
        public int UpperBorder = 40;
        public int LowerBorder = 40;
        public bool DrawGrid = true;
        public LineStyle Unit1GridLine = new LineStyle {Color = ColorUtils.FromHex("#444444"), Width = 1};
        public LineStyle Unit5GridLine = new LineStyle {Color = ColorUtils.FromHex("#5C5C5C"), Width = 1};
        public LineStyle Unit10GridLine = new LineStyle {Color = ColorUtils.FromHex("#5C5C5C"), Width = 2};
        public LineStyle GraphLine = new LineStyle {Color = Color.green, Width = 1};
        public Color BackgroundColor = ColorUtils.FromHex("#383838");

        public event Action ValuesChanged;

        [MenuItem("Edit/Plotter Settings")]
        public static void ShowInspector()
        {
            UnityEditor.Selection.activeObject = instance;
        }

        public void Save()
        {
            ValuesChanged?.Invoke();
            this.Save(true);
        }
    }
}
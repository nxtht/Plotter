using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nxtht.Plotter
{
    public class PlotterSettingsProvider : SettingsProvider
    {
        const string ProjectPath = "Preferences/Plotter";
        private bool _showUnit1Style;
        private bool _showUnit5Style;
        private bool _showUnit10Style;
        private bool _showGraphLineStyle;

        public PlotterSettingsProvider(string path, SettingsScope scopes = SettingsScope.User)
            : base(path, scopes)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            PlotterSettings.instance.ValuesChanged += SettingsOnValuesChanged;
        }

        private void SettingsOnValuesChanged()
        {
            Repaint();
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginChangeCheck();

            PlotterSettings.instance.LeftBorder =
                EditorGUILayout.IntField(new GUIContent(Keys.LeftBorderLabel), PlotterSettings.instance.LeftBorder);
            PlotterSettings.instance.RightBorder =
                EditorGUILayout.IntField(new GUIContent(Keys.RightBorderLabel), PlotterSettings.instance.RightBorder);
            PlotterSettings.instance.UpperBorder =
                EditorGUILayout.IntField(new GUIContent(Keys.UpperBorderLabel), PlotterSettings.instance.UpperBorder);
            PlotterSettings.instance.LowerBorder =
                EditorGUILayout.IntField(new GUIContent(Keys.LowerBorderLabel), PlotterSettings.instance.LowerBorder);
            
            PlotterSettings.instance.DrawGrid =
                EditorGUILayout.Toggle("Draw Grid", PlotterSettings.instance.DrawGrid);

            _showUnit1Style = EditorGUILayout.Foldout(_showUnit1Style, "Unit 1 GridLine Style");
            if (_showUnit1Style)
            {
                var prevIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                PlotterSettings.instance.Unit1GridLine.Width =
                    EditorGUILayout.IntField("Width", PlotterSettings.instance.Unit1GridLine.Width);
                PlotterSettings.instance.Unit1GridLine.Color =
                    EditorGUILayout.ColorField("Color", PlotterSettings.instance.Unit1GridLine.Color);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = prevIndent;
            }

            _showUnit5Style = EditorGUILayout.Foldout(_showUnit5Style, "Unit 5 GridLine Style");
            if (_showUnit5Style)
            {
                var prevIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                PlotterSettings.instance.Unit5GridLine.Width =
                    EditorGUILayout.IntField("Width", PlotterSettings.instance.Unit5GridLine.Width);
                PlotterSettings.instance.Unit5GridLine.Color =
                    EditorGUILayout.ColorField("Color", PlotterSettings.instance.Unit5GridLine.Color);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = prevIndent;
            }

            _showUnit10Style = EditorGUILayout.Foldout(_showUnit10Style, "Unit 10 GridLine Style");
            if (_showUnit10Style)
            {
                var prevIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                PlotterSettings.instance.Unit10GridLine.Width =
                    EditorGUILayout.IntField("Width", PlotterSettings.instance.Unit10GridLine.Width);
                PlotterSettings.instance.Unit10GridLine.Color =
                    EditorGUILayout.ColorField("Color", PlotterSettings.instance.Unit10GridLine.Color);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = prevIndent;
            }

            _showGraphLineStyle = EditorGUILayout.Foldout(_showGraphLineStyle, "GraphLine Style");
            if (_showGraphLineStyle)
            {
                var prevIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                PlotterSettings.instance.GraphLine.Width =
                    EditorGUILayout.IntField("Width", PlotterSettings.instance.GraphLine.Width);
                PlotterSettings.instance.GraphLine.Color =
                    EditorGUILayout.ColorField("Color", PlotterSettings.instance.GraphLine.Color);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = prevIndent;
            }

            EditorGUILayout.BeginHorizontal();
            PlotterSettings.instance.BackgroundColor =
                EditorGUILayout.ColorField("Background Color", PlotterSettings.instance.BackgroundColor);
            EditorGUILayout.EndHorizontal();
            
            if (EditorGUI.EndChangeCheck())
            {
                PlotterSettings.instance.Save();
            }
        }

        public override void OnDeactivate()
        {
            PlotterSettings.instance.ValuesChanged -= SettingsOnValuesChanged;
        }

        [SettingsProvider]
        public static SettingsProvider CreateEditorGraphSettingsProvider()
        {
            var provider = new PlotterSettingsProvider(ProjectPath, SettingsScope.User);

            provider.label = "Plotter";
            provider.keywords = new[] {"Plotter"};
            return provider;
        }
    }
}
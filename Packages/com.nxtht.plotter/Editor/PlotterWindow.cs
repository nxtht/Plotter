using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Nxtht.Plotter
{
    public class PlotterWindow : EditorWindow
    {
        private const float WindowMinWidth = 250;
        private const float WindowMinHeight = 250;

        private Rect _menuBar;
        private Rect _mainPanel;

        private Texture2D _settingsIcon;

        private int _leftBorder = 100;
        private int _rightBorder = 40;
        private int _upperBorder = 40;
        private int _lowerBorder = 40;

        private Material _material;
        private IPlotterGrid _grid;

        private bool _needRepaint;
        private float _menuBarHeight = 20f;

        [MenuItem("Window/Nxtht/Plotter")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<PlotterWindow>().Show();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Plotter");
            this.minSize = new Vector2(WindowMinWidth, WindowMinHeight);
            if (EditorGUIUtility.isProSkin)
            {
                _settingsIcon = EditorGUIUtility.Load("icons/d_SettingsIcon.png") as Texture2D;
            }
            else
            {
                _settingsIcon = EditorGUIUtility.Load("icons/SettingsIcon.png") as Texture2D;
            }

            PlotterSettings.instance.ValuesChanged += Repaint;
            var graphRect = new Rect(
                _leftBorder,
                _upperBorder,
                position.width - (_leftBorder + _rightBorder),
                position.height - (_upperBorder + _lowerBorder));

            _grid = new DecimalPlotterGrid(
                (int) graphRect.width,
                (int) graphRect.height,
                new GraphFileReader()
            );
        }

        private void OnDisable()
        {
            PlotterSettings.instance.ValuesChanged -= Repaint;
            _grid.ClearData();
        }

        private void OnGUI()
        {
            UpdateSettings();

            // TODO: Extract input handling
            var ev = Event.current;
            bool isScaling = ev.shift;
            if (isScaling)
            {
                if (ev.isScrollWheel)
                {
                    _needRepaint = true;
                    if (ev.control)
                    {
                        _grid.ScaleUpY(-Math.Sign(ev.delta.y));
                    }
                    else
                    {
                        _grid.ScaleUpX(-Math.Sign(ev.delta.y));
                    }
                }
            }
            else
            {
                if (ev.isScrollWheel)
                {
                    _needRepaint = true;
                    if (ev.control)
                    {
                        _grid.MoveStartY(Math.Sign(ev.delta.y));
                    }
                    else
                    {
                        _grid.MoveStartX(Math.Sign(ev.delta.y));
                    }
                }
            }

            if (ev.type == EventType.Repaint)
            {
                _material = new Material(Shader.Find("Hidden/Internal-Colored"));
                _grid.Width = (int) position.width - (_leftBorder + _rightBorder);
                _grid.Height = (int) position.height - (_upperBorder + _lowerBorder);
                DrawBackground();
                DrawGrid();
                DrawGraph();
                DrawGridLabels();
            }

            DrawMenuBar();
            //DrawMainPanel();
        }

        private void Update()
        {
            if (_needRepaint)
            {
                _needRepaint = false;
                Repaint();
            }
        }

        private void DrawMenuBar()
        {
            _menuBar = new Rect(0, 0, position.width, _menuBarHeight);
            GUILayout.BeginArea(_menuBar, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button(new GUIContent("Clear"), EditorStyles.toolbarButton, GUILayout.Width(50)))
            {
                _grid.ClearData();
            }

            var drawGridContent = new GUIContent("Draw Grid");
            var drawGridSize = GUI.skin.button.CalcSize(drawGridContent);
            PlotterSettings.instance.DrawGrid =
                GUILayout.Toggle(PlotterSettings.instance.DrawGrid, drawGridContent, EditorStyles.toolbarButton,
                    GUILayout.Width(drawGridSize.x));

            if (GUILayout.Button(new GUIContent("Open file"), EditorStyles.toolbarButton))
            {
                var path = EditorUtility.OpenFilePanel("Select file with graph data", "", "csv");
                if (path.Length != 0)
                {
                    _grid.LoadDataFromFile(path);
                }
            }

            GUILayout.FlexibleSpace();
            var scaleXString = $"Unit Size X: {_grid.PpuX} px";
            var scaleXContent = new GUIContent(scaleXString);
            GUILayout.Label(scaleXContent, EditorStyles.label, GUILayout.Width(150));

            var scaleYString = $"Unit Size Y: {_grid.PpuY} px";
            var scaleYContent = new GUIContent(scaleYString);
            GUILayout.Label(scaleYContent, EditorStyles.label, GUILayout.Width(150));

            if (EditorGUI.EndChangeCheck())
            {
                PlotterSettings.instance.Save();
            }

            if (GUILayout.Button(new GUIContent(_settingsIcon), EditorStyles.toolbarButton, GUILayout.Width(40)))
            {
                SettingsService.OpenUserPreferences("Preferences/Plotter");
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawMainPanel()
        {
            _mainPanel = new Rect(
                0,
                _menuBarHeight,
                position.width,
                position.height - _menuBarHeight);

            GUILayout.BeginArea(_mainPanel);
            EditorGUILayout.LabelField("Position", position.ToString());
            EditorGUILayout.LabelField("Graph Size", $"{_grid.Width}x{_grid.Height}");
            var expX = _grid.ExponentX < 0 ? -_grid.ExponentX : 1;
            var expY = _grid.ExponentY < 0 ? -_grid.ExponentY : 1;
            EditorGUILayout.LabelField(
                "Start X",
                _grid.StartX.ToString("N" + expX, CultureInfo.InvariantCulture));
            EditorGUILayout.LabelField(
                "Start Y",
                _grid.StartY.ToString("N" + expY, CultureInfo.InvariantCulture));
            GUILayout.EndArea();
        }

        private void DrawBackground()
        {
            GUI.BeginClip(new Rect(_leftBorder, _upperBorder, _grid.Width, _grid.Height));
            GL.PushMatrix();
            _material.SetPass(0);

            GL.Begin(GL.QUADS);
            GL.Color(PlotterSettings.instance.BackgroundColor);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(_grid.Width, 0, 0);
            GL.Vertex3(_grid.Width, _grid.Height, 0);
            GL.Vertex3(0, _grid.Height, 0);
            GL.End();

            GL.PopMatrix();
            GUI.EndClip();
        }

        private void DrawGrid()
        {
            GUI.BeginClip(new Rect(_leftBorder, _upperBorder, _grid.Width, _grid.Height));
            GL.PushMatrix();
            GL.Clear(true, false, Color.clear);
            _material.SetPass(0);

            if (PlotterSettings.instance.DrawGrid)
            {
                GL.Begin(GL.QUADS);
                DrawGridLines();
                GL.End();
            }

            GL.PopMatrix();
            GUI.EndClip();
        }

        private void DrawGraph()
        {
            if (!_grid.HasData())
                return;

            GUI.BeginClip(new Rect(_leftBorder, _upperBorder, _grid.Width, _grid.Height));
            GL.PushMatrix();
            _material.SetPass(0);

            GL.Begin(GL.QUADS);
            foreach (var line in _grid.GetDataLines())
            {
                DrawLine(
                    ToScreenPos(line.From),
                    ToScreenPos(line.To),
                    PlotterSettings.instance.GraphLine
                );
            }

            GL.End();

            GL.PopMatrix();
            GUI.EndClip();
        }

        private void DrawPoint(Vector2Int p, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(p.x, p.y, 0);
            GL.Vertex3(p.x + lineStyle.Width, p.y, 0);
            GL.Vertex3(p.x + lineStyle.Width, p.y - lineStyle.Width, 0);
            GL.Vertex3(p.x, p.y + lineStyle.Width, 0);
        }

        private void DrawVertical(Vector2 from, Vector2 to, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(from.x, from.y, 0);
            GL.Vertex3(from.x + lineStyle.Width, from.y, 0);
            GL.Vertex3(from.x + lineStyle.Width, to.y, 0);
            GL.Vertex3(from.x, to.y, 0);
        }

        private void DrawHorizontal(Vector2 from, Vector2 to, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(from.x, from.y, 0);
            GL.Vertex3(to.x, to.y, 0);
            GL.Vertex3(to.x, to.y - lineStyle.Width, 0);
            GL.Vertex3(from.x, from.y - lineStyle.Width, 0);
        }

        private void DrawGridLines()
        {
            // draw unit lines
            DrawGridLinesByModulo(1, PlotterSettings.instance.Unit1GridLine);
            // draw 5 unit lines
            DrawGridLinesByModulo(5, PlotterSettings.instance.Unit5GridLine);
            // draw 10 unit lines
            DrawGridLinesByModulo(10, PlotterSettings.instance.Unit10GridLine);
        }

        /// <summary>
        /// Draw a line in Screen Coordinates
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lineStyle"></param>
        private void DrawLine(Vector2Int from, Vector2Int to, LineStyle lineStyle)
        {
            var v = to - from;
            if (v.x == 0 && v.y == 0)
            {
                DrawPoint(from, lineStyle);
                return;
            }

            if (v.x == 0)
            {
                DrawVertical(from, to, lineStyle);
            }
            else if (v.y == 0)
            {
                DrawHorizontal(from, to, lineStyle);
            }
            else
            {
                if (v.x > 0 && v.y > 0)
                {
                    DrawQuad4(from, to, lineStyle);
                }
                else if (v.x < 0 && v.y > 0)
                {
                    DrawQuad3(from, to, lineStyle);
                }
                else if (v.x < 0 && v.y < 0)
                {
                    DrawQuad2(from, to, lineStyle);
                }
                else
                {
                    DrawQuad1(from, to, lineStyle);
                }
            }
        }

        private void DrawQuad1(Vector2Int from, Vector2Int to, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(from.x + lineStyle.Width, from.y, 0);
            GL.Vertex3(to.x + lineStyle.Width, to.y, 0);
            GL.Vertex3(to.x, to.y - lineStyle.Width, 0);
            GL.Vertex3(from.x, from.y - lineStyle.Width, 0);
        }

        private void DrawQuad2(Vector2Int from, Vector2Int to, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(from.x, from.y, 0);
            GL.Vertex3(from.x + lineStyle.Width, from.y - lineStyle.Width, 0);
            GL.Vertex3(to.x + lineStyle.Width, to.y - lineStyle.Width, 0);
            GL.Vertex3(to.x, to.y, 0);
        }

        private void DrawQuad3(Vector2Int from, Vector2Int to, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(from.x + lineStyle.Width, from.y, 0);
            GL.Vertex3(from.x, from.y - lineStyle.Width, 0);
            GL.Vertex3(to.x, to.y - lineStyle.Width, 0);
            GL.Vertex3(to.x + lineStyle.Width, to.y, 0);
        }

        private void DrawQuad4(Vector2Int from, Vector2Int to, LineStyle lineStyle)
        {
            GL.Color(lineStyle.Color);
            GL.Vertex3(from.x, from.y, 0);
            GL.Vertex3(to.x, to.y, 0);
            GL.Vertex3(to.x + lineStyle.Width, to.y - lineStyle.Width, 0);
            GL.Vertex3(from.x + lineStyle.Width, from.y - lineStyle.Width, 0);
        }

        private void DrawGridLinesByModulo(int m, LineStyle lineStyle)
        {
            foreach (var line in _grid.GetVerticalUnitLinesByModulo(m))
            {
                DrawVertical(ToScreenPos(line.From), ToScreenPos(line.To), lineStyle);
            }

            foreach (var line in _grid.GetHorizontalUnitLinesByModulo(m))
            {
                DrawHorizontal(ToScreenPos(line.From), ToScreenPos(line.To), lineStyle);
            }
        }

        private void DrawGridLabels()
        {
            DrawVerticalLineNumbers(_grid.GetVerticalUnitLinesByModulo(_grid.Base).ToList());
            DrawHorizontalLineNumbers(_grid.GetHorizontalUnitLinesByModulo(_grid.Base).ToList());
        }

        private void DrawVerticalLineNumbers(List<GridLine> lines)
        {
            var exp = _grid.ExponentX;
            var contents = new (GUIContent, Vector2)[lines.Count];
            var maxSize = _grid.PpuX * 10 - 10;
            var isOverlapping = false;
            for (int i = 0; i < lines.Count; i++)
            {
                var f = "N" + (exp < 0 ? -exp : 1);
                var content = new GUIContent(lines[i].Value.ToString(f, CultureInfo.InvariantCulture));
                var cSize = GUI.skin.label.CalcSize(content);
                contents[i].Item1 = content;
                contents[i].Item2 = cSize;
                if (!isOverlapping && cSize.x > maxSize)
                {
                    isOverlapping = true;
                    Debug.Log("Overlapping");
                }
            }

            var verticalSpacing = isOverlapping ? contents[0].Item2.y + 4 : 0f;
            for (var i = 0; i < lines.Count; i++)
            {
                GUI.Label(
                    new Rect(
                        _leftBorder + lines[i].Position,
                        _upperBorder + _grid.Height + verticalSpacing * ((Math.Abs(lines[i].Significand) / 10) % 2),
                        contents[i].Item2.x,
                        contents[i].Item2.y),
                    contents[i].Item1);
            }
        }

        private void DrawHorizontalLineNumbers(List<GridLine> lines)
        {
            var exp = _grid.ExponentY;
            for (int i = 0; i < lines.Count; i++)
            {
                var f = "N" + (exp < 0 ? -exp : 1);
                var content = new GUIContent(lines[i].Value.ToString(f, CultureInfo.InvariantCulture));
                var cSize = GUI.skin.label.CalcSize(content);
                GUI.Label(
                    new Rect(
                        _leftBorder - cSize.x,
                        _upperBorder + _grid.Height - lines[i].Position - cSize.y,
                        cSize.x,
                        cSize.y),
                    content);
            }
        }

        private void UpdateSettings()
        {
            _leftBorder = PlotterSettings.instance.LeftBorder;
            _rightBorder = PlotterSettings.instance.RightBorder;
            _upperBorder = PlotterSettings.instance.UpperBorder;
            _lowerBorder = PlotterSettings.instance.LowerBorder;
        }

        private Vector2Int ToScreenPos(Vector2Int value)
        {
            return new Vector2Int(value.x, _grid.Height - value.y);
        }
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nxtht.Plotter.Editor
{
    [CustomPropertyDrawer(typeof(FieldNameAttribute))]
    public class FieldNamePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.PropertyField(
                position,
                property,
                new GUIContent((attribute as FieldNameAttribute).Name)
            );
        }
    }
}
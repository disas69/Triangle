using UnityEditor;
using UnityEngine;

namespace Framework.DataStructures.Editor
{
    public abstract class SerializableDictionaryEditor<TKey, TValue> : PropertyDrawer
    {
        private const float Space = 5f;

        private bool _foldout;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (_foldout)
            {
                var count = property.FindPropertyRelative("_keys").arraySize;
                return EditorGUIUtility.singleLineHeight * count + EditorGUIUtility.singleLineHeight * 2 + Space;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var lineRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            _foldout = EditorGUI.Foldout(lineRect, _foldout, new GUIContent(string.Format(label.text + " : SerializableDictionary<{0}, {1}>", typeof(TKey).Name, typeof(TValue).Name)));

            if (_foldout)
            {
                EditorGUI.indentLevel = 1;
                var keys = property.FindPropertyRelative("_keys");
                var values = property.FindPropertyRelative("_values");

                for (var i = 0; i < keys.arraySize; i++)
                {
                    lineRect.y += EditorGUIUtility.singleLineHeight;
                    DrawMainProperties(lineRect, keys, values, i);
                }

                lineRect.y += EditorGUIUtility.singleLineHeight + Space;
                if (GUI.Button(lineRect, "Add"))
                {
                    keys.arraySize++;
                    values.arraySize++;
                }
            }

            EditorGUI.indentLevel = indent;
        }

        private void DrawMainProperties(Rect rect, SerializedProperty keys, SerializedProperty values, int index)
        {
            rect.width = (rect.width - 2 * Space) / 4;
            EditorGUI.LabelField(rect, new GUIContent("Element " + index));
            rect.x += rect.width + Space;
            DrawProperty(rect, keys.GetArrayElementAtIndex(index));
            rect.x += rect.width + Space;
            DrawProperty(rect, values.GetArrayElementAtIndex(index));
            rect.x += rect.width + Space;

            if (GUI.Button(rect, "Remove"))
            {
                keys.DeleteArrayElementAtIndex(index);
                values.DeleteArrayElementAtIndex(index);
            }
        }

        private void DrawProperty(Rect rect, SerializedProperty property)
        {
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        }
    }
}
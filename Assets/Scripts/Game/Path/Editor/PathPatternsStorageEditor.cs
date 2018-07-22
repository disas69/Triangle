using Framework.Editor;
using Game.Path.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Path.Editor
{
    [CustomEditor(typeof(PathPatternsStorage))]
    public class PathPatternsStorageEditor : CustomEditorBase<PathPatternsStorage>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.LabelField("Default Settings", HeaderStyle);

            var defaultSettings = serializedObject.FindProperty("DefaultSettings");
            DrawSettings(defaultSettings);

            EditorGUILayout.LabelField("Path Patterns", HeaderStyle);
            if (GUILayout.Button("Add New Pattern"))
            {
                RecordObject("Path Patterns Storage Change");
                Target.PathPatterns.Add(new PathPattern());
            }

            var patterns = serializedObject.FindProperty("PathPatterns");
            var count = patterns.arraySize;

            for (int i = 0; i < count; i++)
            {
                DrawPattern(patterns.GetArrayElementAtIndex(i), i);
            }
        }

        private void DrawPattern(SerializedProperty serializedProperty, int index)
        {
            var patternName = serializedProperty.FindPropertyRelative("Name");
            var values = serializedProperty.FindPropertyRelative("Values");

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        var labelWidth = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 50f;
                        EditorGUILayout.PropertyField(patternName, GUILayout.Width(300f));
                        EditorGUIUtility.labelWidth = labelWidth;

                        serializedProperty.isExpanded = GUILayout.Toggle(serializedProperty.isExpanded, "Edit", new GUIStyle(GUI.skin.button));

                        if (GUILayout.Button("Remove"))
                        {
                            RemovePattern(index);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (serializedProperty.isExpanded)
                    {
                        EditorGUILayout.LabelField("Pattern Settings", LabelStyle);
                        if (GUILayout.Button("Add"))
                        {
                            RecordObject("Path Patterns Storage Change");
                            Target.PathPatterns[index].Values.Add(new Settings());
                        }

                        var count = values.arraySize;
                        for (int i = 0; i < count; i++)
                        {
                            DrawSettings(values.GetArrayElementAtIndex(i), index, i);
                        }

                        serializedObject.ApplyModifiedProperties();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DrawSettings(SerializedProperty serializedProperty, int patternIndex = -1, int index = -1)
        {
            var type = serializedProperty.FindPropertyRelative("LineType");
            var countable = serializedProperty.FindPropertyRelative("Countable");
            var length = serializedProperty.FindPropertyRelative("Length");
            var rotationAngle = serializedProperty.FindPropertyRelative("RotationAngle");

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.PropertyField(type);
                    EditorGUILayout.PropertyField(countable);
                    EditorGUILayout.PropertyField(length);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Rotation Angle");
                        var rotationAngleValue = rotationAngle.floatValue;

                        if (GUILayout.Button("-15"))
                        {
                            rotationAngleValue -= 15f;
                        }

                        EditorGUIUtility.editingTextField = false;
                        EditorGUILayout.PropertyField(rotationAngle, new GUIContent(string.Empty));
                        EditorGUIUtility.editingTextField = true;

                        if (GUILayout.Button("+15"))
                        {
                            rotationAngleValue += 15f;
                        }

                        rotationAngleValue = Mathf.Clamp(rotationAngleValue, -45f, 45f);
                        rotationAngle.floatValue = rotationAngleValue;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                if (patternIndex >= 0 && index >= 0)
                {
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        RemoveSettings(patternIndex, index);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void RemovePattern(int index)
        {
            RecordObject("Path Patterns Storage Change");
            Target.PathPatterns.RemoveAt(index);
        }

        private void RemoveSettings(int patternIndex, int index)
        {
            RecordObject("Path Patterns Storage Change");
            Target.PathPatterns[patternIndex].Values.RemoveAt(index);
        }
    }
}
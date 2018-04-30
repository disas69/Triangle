﻿using Game.Path.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Path.Editor
{
    [CustomEditor(typeof(PathPatternsStorage))]
    public class PathPatternsStorageEditor : UnityEditor.Editor
    {
        private PathPatternsStorage _storage;
        private GUIStyle _headerStyle;

        private void OnEnable()
        {
            _storage = target as PathPatternsStorage;
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.black},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Default Settings", _headerStyle);
            EditorGUI.BeginChangeCheck();

            var defaultSettings = serializedObject.FindProperty("DefaultSettings");
            DrawSettings(defaultSettings);

            EditorGUILayout.LabelField("Path Patterns", _headerStyle);
            var patterns = serializedObject.FindProperty("PathPatterns");
            var count = patterns.arraySize;

            for (int i = 0; i < count; i++)
            {
                DrawPattern(patterns.GetArrayElementAtIndex(i), i);
            }

            if (GUILayout.Button("Add New Pattern"))
            {
                RecordObject();
                _storage.PathPatterns.Add(new PathPattern());
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_storage);
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

                        serializedProperty.isExpanded =
                            GUILayout.Toggle(serializedProperty.isExpanded, "Edit", new GUIStyle(GUI.skin.button));

                        if (GUILayout.Button("Remove"))
                        {
                            RemovePattern(index);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (serializedProperty.isExpanded)
                    {
                        EditorGUILayout.LabelField("Pattern Settings");
                        var count = values.arraySize;
                        for (int i = 0; i < count; i++)
                        {
                            DrawSettings(values.GetArrayElementAtIndex(i), index, i);
                        }

                        if (GUILayout.Button("Add"))
                        {
                            RecordObject();
                            _storage.PathPatterns[index].Values.Add(new Settings());
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
                    if (GUILayout.Button("Remove", GUILayout.Width(100f)))
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
            RecordObject();
            _storage.PathPatterns.RemoveAt(index);
        }

        private void RemoveSettings(int patternIndex, int index)
        {
            RecordObject();
            _storage.PathPatterns[patternIndex].Values.RemoveAt(index);
        }

        private void RecordObject(string changeDescription = "Path Patterns Storage Change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}
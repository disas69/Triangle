using Framework.Editor;
using Game.Path.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Path.Editor
{
    [CustomEditor(typeof(PathLinesMapping))]
    public class PathLinesMappingEditor : CustomEditorBase<PathLinesMapping>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.LabelField("Lines Prefabs", HeaderStyle);
            var prefabsList = serializedObject.FindProperty("PathLinePrefabs");
            var count = prefabsList.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical();
                {
                    for (int i = 0; i < count; i++)
                    {
                        DrawPrefabMapping(prefabsList.GetArrayElementAtIndex(i), i);
                    }
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add"))
            {
                RecordObject("Path Lines Mapping Change");
                Target.PathLinePrefabs.Add(new PathLinePrefab());
            }
        }

        private void DrawPrefabMapping(SerializedProperty serializedProperty, int index)
        {
            var type = serializedProperty.FindPropertyRelative("Type");
            var prefab = serializedProperty.FindPropertyRelative("Prefab");

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.PropertyField(type);
                    EditorGUILayout.PropertyField(prefab);
                }
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                {
                    RecordObject("Path Lines Mapping Change");
                    Target.PathLinePrefabs.RemoveAt(index);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
    }
}
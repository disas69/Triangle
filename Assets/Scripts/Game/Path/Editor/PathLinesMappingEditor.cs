using Game.Path.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Path.Editor
{
    [CustomEditor(typeof(PathLinesMapping))]
    public class PathLinesMappingEditor : UnityEditor.Editor
    {
        private PathLinesMapping _mapping;
        private GUIStyle _headerStyle;

        private void OnEnable()
        {
            _mapping = target as PathLinesMapping;
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.black},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Lines Prefabs", _headerStyle);
            var prefabsList = serializedObject.FindProperty("PathLinePrefabs");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical();
            {
                var count = prefabsList.arraySize;
                for (int i = 0; i < count; i++)
                {
                    DrawPrefabMapping(prefabsList.GetArrayElementAtIndex(i), i);
                }
            }
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Add"))
            {
                RecordObject();
                _mapping.PathLinePrefabs.Add(new PathLinePrefab());
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_mapping);
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
                    RecordObject();
                    _mapping.PathLinePrefabs.RemoveAt(index);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void RecordObject(string changeDescription = "Path Lines Mapping Change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}
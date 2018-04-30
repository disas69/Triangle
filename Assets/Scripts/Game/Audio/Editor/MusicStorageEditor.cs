using Game.Audio.Configuration;
using UnityEditor;
using UnityEngine;

namespace Game.Audio.Editor
{
    [CustomEditor(typeof(MusicStorage))]
    public class MusicStorageEditor : UnityEditor.Editor
    {
        private MusicStorage _musicStorage;
        private GUIStyle _headerStyle;

        private void OnEnable()
        {
            _musicStorage = target as MusicStorage;
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.black},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Audio Clips", _headerStyle);
            var clipsList = serializedObject.FindProperty("MusicClips");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                var count = clipsList.arraySize;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = clipsList.GetArrayElementAtIndex(i);
                        var elementName = element.objectReferenceValue != null
                            ? element.objectReferenceValue.name
                            : "Audio Clip";

                        EditorGUILayout.PropertyField(element, new GUIContent(elementName));
                        if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                        {
                            RecordObject();
                            _musicStorage.MusicClips.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Add"))
            {
                RecordObject();
                _musicStorage.MusicClips.Add(null);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_musicStorage);
            }
        }

        private void RecordObject(string changeDescription = "Music Storage Change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}
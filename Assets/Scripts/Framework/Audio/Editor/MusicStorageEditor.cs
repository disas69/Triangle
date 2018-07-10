using Framework.Audio.Configuration;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Audio.Editor
{
    [CustomEditor(typeof(MusicStorage))]
    public class MusicStorageEditor : CustomEditorBase<MusicStorage>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.LabelField("Audio Clips", HeaderStyle);
            var clipsList = serializedObject.FindProperty("MusicClips");
            var count = clipsList.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
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
                                RecordObject("Music Storage Change");
                                Target.MusicClips.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Add"))
            {
                RecordObject("Music Storage Change");
                Target.MusicClips.Add(null);
            }
        }
    }
}
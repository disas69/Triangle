using Framework.Editor;
using Framework.UI.Notifications.Configuration;
using UnityEditor;
using UnityEngine;

namespace Framework.UI.Notifications.Editor
{
    [CustomEditor(typeof(NotificationsConfiguration))]
    public class NotificationsConfigurationEditor : CustomEditorBase<NotificationsConfiguration>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.LabelField("Notifications Configuration", HeaderStyle);
            if (GUILayout.Button("Add"))
            {
                RecordObject("Notifications Configuration Change");
                Target.Configs.Add(new NotificationConfig());
            }

            var configs = serializedObject.FindProperty("Configs");
            var count = configs.arraySize;

            if (count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    for (int i = 0; i < count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var config = configs.GetArrayElementAtIndex(i);
                            var type = config.FindPropertyRelative("Type");
                            var prefab = config.FindPropertyRelative("Prefab");
                            var label = prefab.objectReferenceValue != null
                                ? prefab.objectReferenceValue.GetType().Name
                                : "None";

                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.LabelField(label, LabelStyle);
                                EditorGUILayout.PropertyField(type);
                                EditorGUILayout.PropertyField(prefab);
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Notifications Configuration Change");
                                Target.Configs.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
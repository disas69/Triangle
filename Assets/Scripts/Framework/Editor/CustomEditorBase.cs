using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public class CustomEditorBase<T> : UnityEditor.Editor where T : Object
    {
        public T Target { get; private set; }
        public GUIStyle HeaderStyle { get; private set; }
        public GUIStyle LabelStyle { get; private set; }

        protected virtual void OnEnable()
        {
            Target = target as T;
            HeaderStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            LabelStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawInspector();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(Target);
            }
        }

        protected virtual void DrawInspector()
        {
        }

        protected void RecordObject(string changeDescription = "CustomEditor change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}
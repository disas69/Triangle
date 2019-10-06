using System.Text;
using Framework.Editor;
using Framework.Editor.GUIUtilities;
using UnityEditor;
using UnityEngine;

namespace Framework.Tools.FSM.Editor
{
    [CustomEditor(typeof(FSMController))]
    public class FSMControllerEditor : CustomEditorBase<FSMController>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            var states = serializedObject.FindProperty("_states");

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Finite State Machine Controller", HeaderStyle);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("States", HeaderStyle);
                    var count = states.arraySize;
                    for (var i = 0; i < count; i++)
                    {
                        var element = states.GetArrayElementAtIndex(i);

                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var elementName = element.objectReferenceValue != null
                                ? element.objectReferenceValue.name
                                : "None";

                            EditorGUILayout.PropertyField(element, new GUIContent(string.Format("{0}. {1}", i + 1, elementName)));
                            if (element.objectReferenceValue != null)
                            {
                                element.isExpanded = GUILayout.Toggle(element.isExpanded, "Edit", new GUIStyle(GUI.skin.button), GUILayout.Width(50f));
                            }

                            if (GUILayout.Button("Remove", GUILayout.Width(60f)))
                            {
                                RecordObject("FSMController Change");
                                states.GetArrayElementAtIndex(i).objectReferenceValue = null;
                                states.DeleteArrayElementAtIndex(i);
                                break;
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        if (element.objectReferenceValue != null && element.isExpanded)
                        {
                            var editor = CreateEditor(element.objectReferenceValue);
                            using (new GUIIndent())
                            {
                                editor.OnInspectorGUI();
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Debug info", HeaderStyle);

                    var stringBuilder = new StringBuilder();
                    var info = "Null";

                    var currentState = Target.CurrentState;
                    if (currentState != null)
                    {
                        stringBuilder.AppendLine(string.Format("Current state: {0}", currentState.Name));
                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine("Action:");

                        if (currentState.Action != null)
                        {
                            stringBuilder.AppendLine(string.Format("{0}", currentState.Action.GetType().Name));
                        }

                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine("Transitions:");

                        for (var i = 0; i < currentState.Transitions.Count; i++)
                        {
                            var transition = currentState.Transitions[i];
                            stringBuilder.AppendLine(string.Format("{0}. Transition: {1} -> {2}", i + 1,
                                transition.Condition.GetType().Name,
                                transition.State.Name));
                        }

                        info = stringBuilder.ToString();
                    }

                    using (new GUIEnabled(false))
                    {
                        EditorGUILayout.TextArea(info);
                        Repaint();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                if (GUILayout.Button("Add"))
                {
                    RecordObject("FSMController Change");
                    states.arraySize++;
                    states.GetArrayElementAtIndex(states.arraySize - 1).objectReferenceValue = null;
                }
            }
        }
    }
}
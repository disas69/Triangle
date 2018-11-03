using System.Text;
using Framework.Editor;
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

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Finite State Machine Controller", HeaderStyle);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("States", HeaderStyle);
                    var states = serializedObject.FindProperty("_states");
                    var count = states.arraySize;
                    for (int i = 0; i < count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var element = states.GetArrayElementAtIndex(i);
                            var elementName = element.objectReferenceValue != null
                                ? element.objectReferenceValue.name
                                : "None";

                            if (i == 0)
                            {
                                elementName += " (Initial state)";
                            }

                            EditorGUILayout.PropertyField(element,
                                new GUIContent(string.Format("{0}. {1}", i + 1, elementName)));
                            if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                            {
                                RecordObject("FSMController Change");
                                Target.States.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Debug info", HeaderStyle);

                    var info = "Null";
                    var stringBuilder = new StringBuilder();

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
                        for (int i = 0; i < currentState.Transitions.Count; i++)
                        {
                            var transition = currentState.Transitions[i];
                            stringBuilder.AppendLine(string.Format("{0}. Transition: {1} -> {2}", i + 1,
                                transition.Condition.GetType().Name,
                                transition.State.Name));
                        }

                        info = stringBuilder.ToString();
                    }

                    EditorGUILayout.TextArea(info);
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                if (GUILayout.Button("Add"))
                {
                    RecordObject("FSMController Change");
                    Target.States.Add(null);
                }
            }
        }
    }
}
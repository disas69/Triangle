using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Tools.FSM.Editor
{
    [CustomEditor(typeof(FSMState))]
    public class FSMStateEditor : CustomEditorBase<FSMState>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField(string.Format("FSMState: {0}", Target.name), HeaderStyle);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    var stateName = serializedObject.FindProperty("_name");
                    EditorGUILayout.PropertyField(stateName);

                    EditorGUILayout.LabelField("Action", LabelStyle);
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        var action = serializedObject.FindProperty("_action");
                        EditorGUILayout.PropertyField(action);
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Transitions", LabelStyle);

                    if (GUILayout.Button("Add transition"))
                    {
                        RecordObject("FSMState Change");
                        Target.Transitions.Add(new FSMTransition());
                    }

                    var transitions = serializedObject.FindProperty("_transitions");
                    for (int i = 0; i < transitions.arraySize; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                var element = transitions.GetArrayElementAtIndex(i);
                                var condition = element.FindPropertyRelative("_condition");
                                var state = element.FindPropertyRelative("_state");

                                var transitionName = "None";
                                if (condition.objectReferenceValue != null && state.objectReferenceValue != null)
                                {
                                    transitionName = string.Format("{0}. Transition: {1} -> {2}", i + 1,
                                        condition.objectReferenceValue.name, state.objectReferenceValue.name);
                                }

                                EditorGUILayout.LabelField(transitionName);
                                EditorGUILayout.PropertyField(condition);
                                EditorGUILayout.PropertyField(state);
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("FSMState Change");
                                Target.Transitions.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
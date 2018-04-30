using UnityEditor;
using UnityEngine;

namespace Framework.Localization.Editor
{
    [CustomEditor(typeof(LocalizationStorage))]
    public class LocalizationStorageEditor : UnityEditor.Editor
    {
        private LocalizationStorage _localizationStorage;
        private string[] _editorBars;
        private GUIStyle _headerStyle;
        private int _currentBarIndex;
        private int _langIndex;

        private void OnEnable()
        {
            _localizationStorage = (LocalizationStorage) target;
            _editorBars = new[] {"Dictionary", "Languages"};
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.black},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Localization Storage", _headerStyle);
            _currentBarIndex = GUILayout.Toolbar(_currentBarIndex, _editorBars);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            if (_currentBarIndex == 0)
            {
                DrawDictionaryEditor();
            }
            else if (_currentBarIndex == 1)
            {
                DrawLanguagesEditor();
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_localizationStorage);
            }
        }

        private void DrawLanguagesEditor()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                for (int i = 0; i < _localizationStorage.Strings.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var selectedLanguage = EditorGUILayout.EnumPopup(new GUIContent("Language " + (i + 1)),
                            _localizationStorage.Strings[i].Language);
                        _localizationStorage.Strings[i].Language = (SystemLanguage) selectedLanguage;

                        if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                        {
                            RecordObject();
                            _localizationStorage.Strings.RemoveAt(i);
                            _langIndex = 0;
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

                var newIndex = _localizationStorage.Strings.Count;
                _localizationStorage.Strings.Add(new LanguageData());
                _localizationStorage.Strings[newIndex].Language = SystemLanguage.English;

                for (int i = 0; i < _localizationStorage.Keys.Count; i++)
                {
                    _localizationStorage.Strings[newIndex].Texts.Add("?");
                }
            }
        }

        private void DrawDictionaryEditor()
        {
            if (_localizationStorage.Strings.Count == 0)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("<-", GUILayout.Width(100f)))
                {
                    _langIndex--;
                    if (_langIndex < 0)
                    {
                        _langIndex = _localizationStorage.Strings.Count - 1;
                    }
                }

                var label = string.Format("{0}. {1}", _langIndex + 1,
                    _localizationStorage.Strings[_langIndex].Language.ToString());
                var subHeaderStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };

                EditorGUILayout.LabelField(label, subHeaderStyle);

                if (GUILayout.Button("->", GUILayout.Width(100f)))
                {
                    _langIndex++;
                    if (_langIndex >= _localizationStorage.Strings.Count)
                    {
                        _langIndex = 0;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Key", GUILayout.Width(100));
                    EditorGUILayout.LabelField("String", GUILayout.Width(100));
                }
                EditorGUILayout.EndVertical();

                for (int i = 0; i < _localizationStorage.Keys.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        _localizationStorage.Keys[i] =
                            EditorGUILayout.TextField(_localizationStorage.Keys[i], GUILayout.Width(100));
                        _localizationStorage.Strings[_langIndex].Texts[i] =
                            EditorGUILayout.TextArea(_localizationStorage.Strings[_langIndex].Texts[i]);

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject();

                            _localizationStorage.Keys.RemoveAt(i);
                            for (int k = 0; k < _localizationStorage.Strings.Count; k++)
                            {
                                _localizationStorage.Strings[k].Texts.RemoveAt(i);
                            }
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

                _localizationStorage.Keys.Add("Key " + _localizationStorage.Keys.Count);
                for (int i = 0; i < _localizationStorage.Strings.Count; i++)
                {
                    _localizationStorage.Strings[i].Texts.Add("?");
                }
            }
        }

        private void RecordObject(string changeDescription = "Localization Storage Change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}
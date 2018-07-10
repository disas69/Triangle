using System.IO;
using System.Text;
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

        private static int BarIndex
        {
            get { return EditorPrefs.GetInt("ls_bar_index", 0); }
            set { EditorPrefs.SetInt("ls_bar_index", value);}
        }

        private static int LangIndex
        {
            get { return EditorPrefs.GetInt("ls_lang_index", 0); }
            set { EditorPrefs.SetInt("ls_lang_index", value); }
        }

        private void OnEnable()
        {
            _localizationStorage = (LocalizationStorage) target;
            _editorBars = new[] {"Dictionary", "Languages"};
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Localization Storage", _headerStyle);
            BarIndex = GUILayout.Toolbar(BarIndex, _editorBars);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            if (BarIndex == 0)
            {
                DrawDictionaryEditor();
            }
            else if (BarIndex == 1)
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
            if (_localizationStorage.LanguagesData.Count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    for (int i = 0; i < _localizationStorage.LanguagesData.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var selectedLanguage = EditorGUILayout.EnumPopup(new GUIContent("Language " + (i + 1)), _localizationStorage.LanguagesData[i].Language);
                            _localizationStorage.LanguagesData[i].Language = (SystemLanguage) selectedLanguage;

                            if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                            {
                                RecordObject();
                                _localizationStorage.LanguagesData.RemoveAt(i);
                                LangIndex = 0;
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
                RecordObject();

                var newIndex = _localizationStorage.LanguagesData.Count;
                _localizationStorage.LanguagesData.Add(new LanguageData());
                _localizationStorage.LanguagesData[newIndex].Language = SystemLanguage.English;

                for (int i = 0; i < _localizationStorage.Keys.Count; i++)
                {
                    _localizationStorage.LanguagesData[newIndex].Strings.Add("?");
                }
            }
        }

        private void DrawDictionaryEditor()
        {
            if (_localizationStorage.LanguagesData.Count == 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Add new Language using \"Languages\" tab.");
                }
                EditorGUILayout.EndVertical();
                
                return;
            }

            if (LangIndex >= _localizationStorage.LanguagesData.Count)
            {
                LangIndex = 0;
            }

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("<-", GUILayout.Width(100f)))
                {
                    LangIndex--;
                    if (LangIndex < 0)
                    {
                        LangIndex = _localizationStorage.LanguagesData.Count - 1;
                    }
                }

                var label = string.Format("{0}. {1}", LangIndex + 1, _localizationStorage.LanguagesData[LangIndex].Language.ToString());
                var subHeaderStyle = new GUIStyle(GUI.skin.label)
                {
                    normal = {textColor = Color.gray},
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Italic
                };

                EditorGUILayout.LabelField(label, subHeaderStyle);

                if (GUILayout.Button("->", GUILayout.Width(100f)))
                {
                    LangIndex++;
                    if (LangIndex >= _localizationStorage.LanguagesData.Count)
                    {
                        LangIndex = 0;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Import CSV"))
                    {
                        ImportCSV();
                    }

                    if (GUILayout.Button("Export CSV"))
                    {
                        ExportCSV();
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

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
                        _localizationStorage.Keys[i] = EditorGUILayout.TextField(_localizationStorage.Keys[i], GUILayout.Width(100));
                        _localizationStorage.LanguagesData[LangIndex].Strings[i] = EditorGUILayout.TextArea(_localizationStorage.LanguagesData[LangIndex].Strings[i]);

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject();

                            _localizationStorage.Keys.RemoveAt(i);
                            for (int k = 0; k < _localizationStorage.LanguagesData.Count; k++)
                            {
                                _localizationStorage.LanguagesData[k].Strings.RemoveAt(i);
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
                for (int i = 0; i < _localizationStorage.LanguagesData.Count; i++)
                {
                    _localizationStorage.LanguagesData[i].Strings.Add("?");
                }
            }
        }

        private void ImportCSV()
        {
            var path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                var file = File.ReadAllText(path);
                var lines = file.Split('\n');

                for (int j = 0; j < lines.Length; j++)
                {
                    var line = lines[j];
                    var strings = line.Split(',');

                    if (strings.Length > 1)
                    {
                        var key = strings[0];
                        var index = _localizationStorage.Keys.FindIndex(k => k == key);
                        if (index >= 0)
                        {
                            _localizationStorage.LanguagesData[LangIndex].Strings[index] = strings[1];
                        }
                        else
                        {
                            _localizationStorage.Keys.Add(key);
                            for (int i = 0; i < _localizationStorage.LanguagesData.Count; i++)
                            {
                                _localizationStorage.LanguagesData[i].Strings.Add(i == LangIndex ? strings[1] : "?");
                            }
                        }
                    }
                }
            }
        }

        private void ExportCSV()
        {
            var path = EditorUtility.SaveFilePanel("Export CSV", "", _localizationStorage.LanguagesData[LangIndex].Language.ToString(), "csv");
            if (!string.IsNullOrEmpty(path))
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < _localizationStorage.Keys.Count; i++)
                {
                    var key = _localizationStorage.Keys[i];
                    var value = _localizationStorage.LanguagesData[LangIndex].Strings[i];
                    stringBuilder.AppendLine(string.Format("{0},{1}", key, value));
                }

                File.WriteAllText(path, stringBuilder.ToString());
            }
        }

        private void RecordObject(string changeDescription = "Localization Storage Change")
        {
            Undo.RecordObject(serializedObject.targetObject, changeDescription);
        }
    }
}
using System.IO;
using System.Text;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Localization.Editor
{
    [CustomEditor(typeof(LocalizationStorage))]
    public class LocalizationStorageEditor : CustomEditorBase<LocalizationStorage>
    {
        private string[] _editorBars;
        private SearchBar _searchBar;

        private static int BarIndex
        {
            get { return EditorPrefs.GetInt("ls_bar_index", 0); }
            set { EditorPrefs.SetInt("ls_bar_index", value); }
        }

        private static int LangIndex
        {
            get { return EditorPrefs.GetInt("ls_lang_index", 0); }
            set { EditorPrefs.SetInt("ls_lang_index", value); }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _editorBars = new[] {"Dictionary", "Languages"};
            _searchBar = new SearchBar();
        }

        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.LabelField("Localization Storage", HeaderStyle);
            BarIndex = GUILayout.Toolbar(BarIndex, _editorBars);
            EditorGUILayout.Space();

            if (BarIndex == 0)
            {
                DrawDictionaryEditor();
            }
            else if (BarIndex == 1)
            {
                DrawLanguagesEditor();
            }
        }

        private void DrawLanguagesEditor()
        {
            if (GUILayout.Button("Add language"))
            {
                RecordObject("Localization Storage Change");

                var newIndex = Target.LanguagesData.Count;
                Target.LanguagesData.Add(new LanguageData());
                Target.LanguagesData[newIndex].Language = SystemLanguage.English;

                for (int i = 0; i < Target.Keys.Count; i++)
                {
                    Target.LanguagesData[newIndex].Strings.Add("?");
                }
            }

            if (Target.LanguagesData.Count > 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    for (int i = 0; i < Target.LanguagesData.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            var selectedLanguage = EditorGUILayout.EnumPopup(new GUIContent("Language " + (i + 1)), Target.LanguagesData[i].Language);
                            Target.LanguagesData[i].Language = (SystemLanguage) selectedLanguage;

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                RecordObject("Localization Storage Change");
                                Target.LanguagesData.RemoveAt(i);
                                LangIndex = 0;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawDictionaryEditor()
        {
            if (Target.LanguagesData.Count == 0)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Add new Language using \"Languages\" tab.");
                }
                EditorGUILayout.EndVertical();

                return;
            }

            if (LangIndex >= Target.LanguagesData.Count)
            {
                LangIndex = 0;
            }

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("<-", GUILayout.MaxWidth(100f)))
                {
                    LangIndex--;
                    if (LangIndex < 0)
                    {
                        LangIndex = Target.LanguagesData.Count - 1;
                    }
                }

                var label = string.Format("{0}. {1}", LangIndex + 1, Target.LanguagesData[LangIndex].Language.ToString());
                var subHeaderStyle = new GUIStyle(GUI.skin.label)
                {
                    normal = {textColor = Color.gray},
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Italic
                };

                EditorGUILayout.LabelField(label, subHeaderStyle);

                if (GUILayout.Button("->", GUILayout.MaxWidth(100f)))
                {
                    LangIndex++;
                    if (LangIndex >= Target.LanguagesData.Count)
                    {
                        LangIndex = 0;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add key"))
                        {
                            RecordObject("Localization Storage Change");

                            Target.Keys.Add("Key " + Target.Keys.Count);
                            for (int i = 0; i < Target.LanguagesData.Count; i++)
                            {
                                Target.LanguagesData[i].Strings.Add("?");
                            }
                        }

                        if (GUILayout.Button("Import CSV"))
                        {
                            ImportCSV();
                        }

                        if (GUILayout.Button("Export CSV"))
                        {
                            ExportCSV();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    _searchBar.Draw();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Key", GUILayout.Width(100));
                    EditorGUILayout.LabelField("String", GUILayout.Width(100));
                }
                EditorGUILayout.EndVertical();

                var textAreaStyle = GUI.skin.box;
                textAreaStyle.normal.textColor = Color.gray;
                textAreaStyle.fontStyle = FontStyle.Normal;
                textAreaStyle.alignment = TextAnchor.MiddleLeft;

                int searchResultsCount = 0;
                for (int i = 0; i < Target.Keys.Count; i++)
                {
                    if (!_searchBar.IsEmpty && !_searchBar.IsMatchingTheFilter(Target.Keys[i]) && !_searchBar.IsMatchingTheFilter(Target.LanguagesData[LangIndex].Strings[i]))
                    {
                        continue;
                    }

                    searchResultsCount++;
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        Target.Keys[i] = EditorGUILayout.TextField(Target.Keys[i], GUILayout.Width(100));
                        Target.LanguagesData[LangIndex].Strings[i] = EditorGUILayout.TextArea(Target.LanguagesData[LangIndex].Strings[i], textAreaStyle, GUILayout.ExpandWidth(true));

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Localization Storage Change");

                            Target.Keys.RemoveAt(i);
                            for (int k = 0; k < Target.LanguagesData.Count; k++)
                            {
                                Target.LanguagesData[k].Strings.RemoveAt(i);
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }

                if (searchResultsCount == 0)
                {
                    EditorGUILayout.LabelField("No matches found...", LabelStyle);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void ImportCSV()
        {
            var path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                var file = File.ReadAllText(path);
                var lines = file.Split('\n');

                int index = -1;
                for (int j = 0; j < lines.Length; j++)
                {
                    var line = lines[j];
                    var strings = line.Split(',');

                    if (strings.Length == 2)
                    {
                        var key = strings[0];

                        index = Target.Keys.FindIndex(k => k == key);
                        if (index >= 0)
                        {
                            Target.LanguagesData[LangIndex].Strings[index] = strings[1];
                        }
                        else
                        {
                            Target.Keys.Add(key);
                            for (int i = 0; i < Target.LanguagesData.Count; i++)
                            {
                                Target.LanguagesData[i].Strings.Add(i == LangIndex ? strings[1] : "?");
                            }
                        }
                    }
                    else if (strings.Length == 1)
                    {
                        var value = strings[0];
                        if (index >= 0)
                        {
                            Target.LanguagesData[LangIndex].Strings[index] += string.Format("\n{0}", value);
                        }
                    }
                    else
                    {
                        Debug.LogWarningFormat("ImportCSV error: CSV file contains invalid line \"{0}\" that has been omitted during import", j);
                    }
                }
            }
        }

        private void ExportCSV()
        {
            var path = EditorUtility.SaveFilePanel("Export CSV", "", Target.LanguagesData[LangIndex].Language.ToString(), "csv");
            if (!string.IsNullOrEmpty(path))
            {
                int index = 0;
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < Target.Keys.Count; i++)
                {
                    var key = Target.Keys[i];
                    var value = Target.LanguagesData[LangIndex].Strings[i];

                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        var strings = string.Format("{0},{1}", key, value);
                        if (index != 0)
                        {
                            stringBuilder.AppendLine();
                        }

                        stringBuilder.Append(strings);
                        index++;
                    }
                }

                File.WriteAllText(path, stringBuilder.ToString());
            }
        }
    }
}
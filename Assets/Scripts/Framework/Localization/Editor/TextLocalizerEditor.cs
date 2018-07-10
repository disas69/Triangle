using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Localization.Editor
{
    [CustomEditor(typeof(TextLocalizer))]
    public class TextLocalizerEditor : UnityEditor.Editor
    {
        private TextLocalizer _textLocalizer;
        private List<string> _keys;
        private int _selectedIndex;

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            _textLocalizer = (TextLocalizer) target;

            var dictionary = (LocalizationStorage) AssetDatabase.LoadAssetAtPath(LocalizationStorage.AssetPath + "LocalizationStorage.asset", typeof(LocalizationStorage));
            if (dictionary != null)
            {
                _keys = dictionary.Keys;
                _selectedIndex = Mathf.Max(0, _keys.FindIndex(key => key == _textLocalizer.Key));
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_keys != null)
            {
                if (_keys.Count == 0)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.LabelField("LocalizationStorage asset doesn't contain any strings yet.");
                        if (GUILayout.Button("Add strings"))
                        {
                            var dictionary = (LocalizationStorage) AssetDatabase.LoadAssetAtPath(LocalizationStorage.AssetPath + "LocalizationStorage.asset", typeof(LocalizationStorage));
                            Selection.activeObject = dictionary;
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    _selectedIndex = EditorGUILayout.Popup("Key", _selectedIndex, _keys.ToArray());

                    var keyProperty = serializedObject.FindProperty("Key");
                    keyProperty.stringValue = _keys[_selectedIndex];

                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
            }
            else
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Failed to find valid LocalizationStorage asset at path:");
                    EditorGUILayout.LabelField(LocalizationStorage.AssetPath + "LocalizationStorage.asset");
                    
                    if (GUILayout.Button("Create"))
                    {
                        if (!Directory.Exists(LocalizationStorage.AssetPath))
                        {
                            Directory.CreateDirectory(LocalizationStorage.AssetPath);
                        }

                        var localizationStorage = ScriptableObject.CreateInstance<LocalizationStorage>();
                        AssetDatabase.CreateAsset(localizationStorage, LocalizationStorage.AssetPath + "LocalizationStorage.asset");
                        Initialize();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
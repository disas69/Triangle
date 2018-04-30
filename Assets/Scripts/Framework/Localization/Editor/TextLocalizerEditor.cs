using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Localization.Editor
{
    [CustomEditor(typeof(TextLocalizer))]
    public class TextLocalizerEditor : UnityEditor.Editor
    {
        private const string AssetPath = "Assets/Resources/Localization/LocalizationStorage.asset";

        private TextLocalizer _textLocalizer;
        private List<string> _keys;
        private int _selectedIndex;

        private void OnEnable()
        {
            _textLocalizer = (TextLocalizer) target;

            var dictionary = (LocalizationStorage) AssetDatabase.LoadAssetAtPath(AssetPath,
                typeof(LocalizationStorage));

            if (dictionary != null)
            {
                _keys = dictionary.Keys;
                _selectedIndex = Mathf.Max(0, _keys.FindIndex(key => key == _textLocalizer.Key));
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_keys != null && _keys.Count > 0)
            {
                _selectedIndex = EditorGUILayout.Popup("Key", _selectedIndex, _keys.ToArray());

                var keyProperty = serializedObject.FindProperty("Key");
                keyProperty.stringValue = _keys[_selectedIndex];
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.LabelField("Failed to find valid LocalizationStorage asset at path:");
                    EditorGUILayout.LabelField(AssetPath);
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
using UnityEditor;

namespace Framework.Localization.Editor
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var localizationStorage = serializedObject.FindProperty("_localizationStorageAsset");
            var useSystemLanguage = serializedObject.FindProperty("_useSystemLanguage");
            var startupLanguage = serializedObject.FindProperty("_language");

            EditorGUILayout.PropertyField(localizationStorage);
            EditorGUILayout.PropertyField(useSystemLanguage);

            if (!useSystemLanguage.boolValue)
            {
                EditorGUILayout.PropertyField(startupLanguage);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}
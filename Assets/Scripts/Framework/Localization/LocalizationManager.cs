using System;
using System.Collections.Generic;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private static Dictionary<string, string> _languageDictionary;
        private static LocalizationStorage _localizationStorage;
        private static bool _isInitialized;

        [SerializeField] private LocalizationStorage _localizationStorageAsset;
        [SerializeField] private bool _useSystemLanguage;
        [SerializeField] private SystemLanguage _language;

        public static event Action LanguageChanged;

        public static SystemLanguage CurrentLanguage { get; private set; }

        public static void SetLanguage(SystemLanguage language)
        {
            if (language == CurrentLanguage)
            {
                return;
            }

            CurrentLanguage = language;
            UpdateLanguageDictionary();
            LanguageChanged.SafeInvoke();
        }

        public static string GetString(string key)
        {
            string value;
            if (_languageDictionary.TryGetValue(key, out value))
            {
                return value;
            }

            Debug.LogError(string.Format("Failed to find string by key \"{0}\" for language \"{1}\"", key, CurrentLanguage));
            return "?";
        }

        private void Awake()
        {
            if (_localizationStorageAsset != null)
            {
                Initialize(_localizationStorageAsset, _language, _useSystemLanguage);
                UpdateLanguageDictionary();
            }
            else
            {
                Debug.LogError("Failed to initialize LocalizationManager!");
            }
        }

        private static void Initialize(LocalizationStorage localizationStorage, SystemLanguage language, bool useSystemLanguage)
        {
            if (_isInitialized)
            {
                return;
            }

            _languageDictionary = new Dictionary<string, string>();
            _localizationStorage = localizationStorage;

            CurrentLanguage = useSystemLanguage ? GetSystemLanguage() : language;
            _isInitialized = true;
        }

        private static SystemLanguage GetSystemLanguage()
        {
            var systemLanguage = Application.systemLanguage;

            foreach (var languageData in _localizationStorage.LanguagesData)
            {
                var language = languageData.Language;
                if (language == systemLanguage)
                {
                    return language;
                }
            }

            return SystemLanguage.English;
        }

        private static void UpdateLanguageDictionary()
        {
            _languageDictionary.Clear();

            var languageData = _localizationStorage.LanguagesData.Find(data => data.Language == CurrentLanguage);
            if (languageData != null)
            {
                for (int i = 0; i < _localizationStorage.Keys.Count; i++)
                {
                    var key = _localizationStorage.Keys[i];
                    var value = languageData.Strings[i];

                    _languageDictionary.Add(key, value);
                }
            }
            else
            {
                Debug.LogError(string.Format("Failed to find language data for language \"{0}\"", CurrentLanguage));
            }
        }
    }
}
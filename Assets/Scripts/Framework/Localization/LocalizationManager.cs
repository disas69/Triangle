using UnityEngine;

namespace Framework.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        private static LocalizationStorage _localizationStorage;
        private static SystemLanguage _currentLanguage;
        private static bool _isInitialized;

        [SerializeField] private LocalizationStorage _localizationStorageAsset;
        [SerializeField] private bool _useSystemLanguage;
        [SerializeField] private SystemLanguage _startupLanguage;

        public static SystemLanguage CurrentLanguage
        {
            get { return _currentLanguage; }
        }

        public static string GetString(string key)
        {
            if (_isInitialized)
            {
                var languageData = _localizationStorage.Strings.Find(data => data.Language == _currentLanguage);
                if (languageData != null)
                {
                    var keyIndex = _localizationStorage.Keys.FindIndex(k => k == key);
                    if (keyIndex >= 0)
                    {
                        return languageData.Texts[keyIndex];
                    }
                }
            }

            Debug.LogError(string.Format("Failed to find string by key \"{0}\" for language \"{1}\"",
                key, _currentLanguage));
            return null;
        }

        private void Awake()
        {
            if (_localizationStorageAsset != null)
            {
                Initialize(_localizationStorageAsset);
            }
            else
            {
                Debug.LogError("Failed to initialize LocalizationManager!");
                return;
            }

            if (_useSystemLanguage)
            {
                _currentLanguage = GetSystemLanguage();
            }
            else
            {
                _currentLanguage = _startupLanguage;
            }
        }

        private static void Initialize(LocalizationStorage dictionary)
        {
            if (_isInitialized)
            {
                return;
            }

            _localizationStorage = dictionary;
            _isInitialized = true;
        }

        private static SystemLanguage GetSystemLanguage()
        {
            var systemLanguage = Application.systemLanguage;

            foreach (var languageData in _localizationStorage.Strings)
            {
                var language = languageData.Language;
                if (language == systemLanguage)
                {
                    return language;
                }
            }

            return SystemLanguage.English;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Localization
{
    public enum TextLocalizerMode
    {
        NoMode,
        ToUpper,
        ToLower
    }
    
    [RequireComponent(typeof(Text))]
    public class TextLocalizer : MonoBehaviour
    {
        private SystemLanguage _currentLanguage;

        [HideInInspector] public string Key;
        public TextLocalizerMode Mode = TextLocalizerMode.NoMode;

        private void Awake()
        {
            Localize();
        }

        private void LateUpdate()
        {
            if (_currentLanguage != LocalizationManager.CurrentLanguage)
            {
                Localize();
            }
        }

        private void Localize()
        {
            var textComponent = GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = GetString(Key);
            }

            _currentLanguage = LocalizationManager.CurrentLanguage;
        }

        private string GetString(string key)
        {
            var result = LocalizationManager.GetString(key);

            switch (Mode)
            {
                case TextLocalizerMode.ToUpper:
                    result = result.ToUpper();
                    break;
                case TextLocalizerMode.ToLower:
                    result = result.ToLower();
                    break;
            }

            return result;
        }
    }
}
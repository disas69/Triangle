using UnityEngine;
using UnityEngine.UI;

namespace Framework.Localization
{
    [RequireComponent(typeof(Text))]
    public class TextLocalizer : MonoBehaviour
    {
        private SystemLanguage _currentLanguage;

        [HideInInspector] public string Key;

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
                textComponent.text = LocalizationManager.GetString(Key);
            }

            _currentLanguage = LocalizationManager.CurrentLanguage;
        }
    }
}
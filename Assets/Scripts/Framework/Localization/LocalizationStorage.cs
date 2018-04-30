using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Localization
{
    [Serializable]
    [CreateAssetMenu(fileName = "LocalizationStorage", menuName = "Localization/LocalizationStorage")]
    public class LocalizationStorage : ScriptableObject
    {
        public List<string> Keys = new List<string>();
        public List<LanguageData> Strings = new List<LanguageData>();
    }

    [Serializable]
    public class LanguageData
    {
        public SystemLanguage Language;
        public List<string> Texts = new List<string>();
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Screen = Framework.UI.Structure.Base.Screen;

namespace Framework.UI.Configuration
{
    [CreateAssetMenu(fileName = "ScreensMapping", menuName = "UI/ScreensMapping")]
    public class ScreensMapping : ScriptableObject
    {
        public List<ScreenSetting> ScreenSettings = new List<ScreenSetting>();
    }

    [Serializable]
    public class ScreenSetting
    {
        public Screen Screen;
        public bool IsCached;
    }
}
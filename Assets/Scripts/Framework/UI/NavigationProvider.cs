using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Extensions;
using Framework.UI.Configuration;
using Framework.UI.Structure;
using UnityEngine;
using Screen = Framework.UI.Structure.Base.Screen;

namespace Framework.UI
{
    public class NavigationProvider : MonoBehaviour, INavigationProvider
    {
        private bool _isInitialized;
        private Dictionary<Type, Screen> _cachedScreens;
        private Dictionary<Type, ScreenSetting> _screenSettingsDictionary;
        private Coroutine _openingCoroutine;

        [SerializeField] private ScreensMapping _screensMapping;
        [SerializeField] private Transform _sceensRoot;

        public Screen CurrentScreen { get; private set; }

        public void OpenScreen<T>() where T : Screen
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            this.SafeStopCoroutine(_openingCoroutine);
            _openingCoroutine = StartCoroutine(OpenScreen(typeof(T)));
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _cachedScreens = new Dictionary<Type, Screen>();
            _screenSettingsDictionary = new Dictionary<Type, ScreenSetting>(_screensMapping.ScreenSettings.Count);

            for (int i = 0; i < _screensMapping.ScreenSettings.Count; i++)
            {
                var screenSettings = _screensMapping.ScreenSettings[i];
                if (screenSettings != null)
                {
                    _screenSettingsDictionary.Add(screenSettings.Screen.GetType(), screenSettings);
                }
            }

            _isInitialized = true;
        }

        private IEnumerator OpenScreen(Type screenType)
        {
            if (CurrentScreen != null)
            {
                CurrentScreen.OnExit();

                while (CurrentScreen.IsInTransition)
                {
                    yield return null;
                }

                Screen currentScreen;
                if (!_cachedScreens.TryGetValue(CurrentScreen.GetType(), out currentScreen))
                {
                    Destroy(CurrentScreen.gameObject);
                }
            }

            Screen cachedScreen;
            if (_cachedScreens.TryGetValue(screenType, out cachedScreen))
            {
                cachedScreen.OnEnter();
                CurrentScreen = cachedScreen;
            }
            else
            {
                ScreenSetting screenSetting;
                if (_screenSettingsDictionary.TryGetValue(screenType, out screenSetting))
                {
                    var screen = Instantiate(screenSetting.Screen, _sceensRoot);
                    if (screenSetting.IsCached)
                    {
                        _cachedScreens.Add(screen.GetType(), screen);
                    }

                    screen.OnEnter();
                    CurrentScreen = screen;
                }
                else
                {
                    throw new NotImplementedException(string.Format("Screen of type {0} isn't implemented yet",
                        screenType.Name));
                }
            }

            while (CurrentScreen.IsInTransition)
            {
                yield return null;
            }

            _openingCoroutine = null;
        }
    }
}
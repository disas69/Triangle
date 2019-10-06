using System.Collections;
using Framework.UI;
using Game.UI.Screens.Play;
using Game.UI.Screens.Replay;
using Game.UI.Screens.Start;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using Screen = Framework.UI.Structure.Base.Screen;

namespace Editor.Tests
{
    public class NavigationProviderTests
    {
        private const string ScenePath = "Assets/Scenes/Main.unity";

        //[UnityTest]
        public IEnumerator _1_Open_Start_Screen()
        {
            EditorSceneManager.OpenScene(ScenePath);
            yield return null;

            TestNavigation<StartPage>();
        }

        //[UnityTest]
        public IEnumerator _2_Open_Play_Screen()
        {
            EditorSceneManager.OpenScene(ScenePath);
            yield return null;

            TestNavigation<PlayPage>();
        }

        //[UnityTest]
        public IEnumerator _2_Open_Replay_Screen()
        {
            EditorSceneManager.OpenScene(ScenePath);
            yield return null;

            TestNavigation<ReplayPage>();
        }

        private static void TestNavigation<T>() where T : Screen
        {
            var navigationProvider = Object.FindObjectOfType<NavigationManager>();
            if (navigationProvider != null)
            {
                navigationProvider.OpenScreen<T>();
                Assert.AreEqual(navigationProvider.CurrentScreen.GetType(), typeof(T));
            }
            else
            {
                Debug.LogErrorFormat("Failed to find NavigationProvider instance in {0} scene", ScenePath);
            }
        }
    }
}
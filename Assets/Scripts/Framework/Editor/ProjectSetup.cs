using System.IO;
using Framework.Audio;
using Framework.Localization;
using Framework.Signals.Listeners;
using Framework.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Editor
{
    public class ProjectSetup
    {
        private const string MainScenePath = "Assets/Scenes/Main.unity";

        [MenuItem("Project/Create Hierarchy")]
        public static void CreateProjectHierarchy()
        {
            CreateFolder("Assets", "Audio");
            CreateFolder("Assets", "Editor");
            CreateFolder("Assets", "Graphics");
            CreateFolder("Assets", "Plugins");
            CreateFolder("Assets", "Prefabs");
            CreateFolder("Assets", "Resources");
            CreateFolder("Assets", "Scenes");
            CreateFolder("Assets", "Scripts");

            AssetDatabase.Refresh();
        }

        [MenuItem("Project/Create Main Scene")]
        public static void CreateMainScene()
        {
            if (File.Exists(MainScenePath))
            {
                EditorSceneManager.OpenScene(MainScenePath);
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var gameRoot = new GameObject("Game");
            gameRoot.AddComponent<LocalizationManager>();
            gameRoot.transform.SetParent(null);

            var audio = new GameObject("Audio");
            audio.AddComponent<AudioPlayer>();
            audio.AddComponent<StringSignalListener>();
            audio.transform.SetParent(gameRoot.transform);

            var camera = new GameObject("Camera");
            camera.AddComponent<Camera>();
            camera.AddComponent<AudioListener>();
            camera.transform.SetParent(gameRoot.transform);

            var canvas = new GameObject("Canvas");
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.AddComponent<GraphicRaycaster>();
            canvas.AddComponent<NavigationManager>();
            canvas.transform.SetParent(null);

            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(null);

            CreateFolder("Assets", "Scenes");
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, MainScenePath);
            EditorSceneManager.OpenScene(scene.path);
        }

        private static void CreateFolder(string path, string name)
        {
            if (AssetDatabase.IsValidFolder(string.Format("{0}/{1}", path, name)))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, name);
        }
    }
}
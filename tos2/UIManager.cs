using System.Collections.Generic;
using System.Linq;
using MRK.Data;
using MRK.Data.Shim;
using MRK.Scenes;
using MRK.Scenes.Home;
using MRK.Textures;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MRK
{
    public class UIManager : MonoBehaviour
    {
        private Dictionary<string, BaseScene> _scenes;
        private TextureManager _textureManager;
        private UIRootRenderer _renderer;

        public List<BaseScene> Scenes => _scenes.Values.ToList();
        public TextureManager TextureManager => _textureManager;
        public UIRootRenderer Renderer => _renderer;

        public Font FontRuncible { get; private set; }

        public IDataProvider DataProvider { get; set; }
        public bool IsRunningShim => DataProvider is ShimDataProvider;

        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            _scenes = new Dictionary<string, BaseScene>();
            _textureManager = new TextureManager();
            _renderer = new UIRootRenderer();

            Logger.Log("UIManager Awake");
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        private void Start()
        {
            Logger.Log("UIManager started");

            RegisterFonts();

            // Init renderer
            if (!_renderer.Initialize())
            {
                Logger.LogError("Failed to initialize UIRenderer");
                return;
            }

            // Register scene handlers
            _scenes["HomeScene"] = new HomeScene();

            Logger.Log($"Registered scene handlers: {string.Join(", ", _scenes.Keys)}");

            gameObject.AddComponent<DebugWindow>();

            // Disable all handlers initially
            foreach (var handler in _scenes.Values)
            {
                handler.OnLoadTextures();
                handler.IsActive = false;
            }

            // Initially activate handler for current scene
            var currentScene = SceneManager.GetActiveScene();
            Logger.Log($"Current active scene: {currentScene.name}");

            SetSceneHandlerEnabled(currentScene.name);
        }

        private void OnGUI()
        {
            // Reset
            GUI.color = Color.white;
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;

            // Render current scene UI
            var activeHandler = _scenes.Values.FirstOrDefault(h => h.IsActive);
            activeHandler?.OnGUI();

            GUI.Label(new Rect(0f, 0f, 200f, 20f), "<size=10><b>MRK UI Manager</b></size>");
        }

        private void OnDestroy()
        {
            Logger.Log("UIManager destroyed");
            Logger.Shutdown();
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            Logger.Log($"Scene changed: {oldScene.name} -> {newScene.name}");

            // Disable all handlers first
            foreach (var handler in _scenes.Values)
            {
                if (handler.IsActive)
                {
                    handler.OnSceneDeactivated();
                }

                handler.IsActive = false;
            }

            // Enable new handler
            SetSceneHandlerEnabled(newScene.name);
        }

        private void SetSceneHandlerEnabled(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                return;
            }

            if (_scenes.TryGetValue(sceneName, out var handler))
            {
                Logger.Log($"Enabling handler for scene: {sceneName}");
                handler.IsActive = true;
                StartCoroutine(handler.OnSceneActivated());
            }
        }

        private void RegisterFonts()
        {
            // For now users must install Runcible manually..
            FontRuncible = Font.CreateDynamicFontFromOSFont("Runcible", 14);
        }
    }
}

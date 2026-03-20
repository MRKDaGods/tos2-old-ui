using MRK.SceneHandlers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MRK
{
    public class UIManager : MonoBehaviour
    {
        private Dictionary<string, BaseSceneHandler> _sceneHandlers;
        private Textures _textures;

        public Textures Textures => _textures;

        public static UIManager Instance { get; private set; }


        private void Awake()
        {
            Instance = this;

            _sceneHandlers = new Dictionary<string, BaseSceneHandler>();
            _textures = new Textures();

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

            // Register scene handlers
            _sceneHandlers["HomeScene"] = gameObject.AddComponent<HomeSceneHandler>();

            Logger.Log($"Registered scene handlers: {string.Join(", ", _sceneHandlers.Keys)}");

            // Disable all handlers initially
            foreach (var handler in _sceneHandlers.Values)
            {
                handler.OnLoadTextures();
                handler.enabled = false;
            }

            // Initially activate handler for current scene
            var currentScene = SceneManager.GetActiveScene();
            Logger.Log($"Current active scene: {currentScene.name}");

            SetSceneHandlerEnabled(currentScene.name);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0f, 0f, 200f, 20f), "<b>MRK UI Manager</b>");
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            Logger.Log($"Scene changed: {oldScene.name} -> {newScene.name}");

            // Disable all handlers first
            foreach (var handler in _sceneHandlers.Values)
            {
                if (handler.enabled)
                {
                    handler.OnSceneDeactivated();
                }

                handler.enabled = false;
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

            if (_sceneHandlers.TryGetValue(sceneName, out var handler))
            {
                Logger.Log($"Enabling handler for scene: {sceneName}");
                handler.enabled = true;
                handler.OnSceneActivated();
            }
        }
    }
}

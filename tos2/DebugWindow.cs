using System.Collections.Generic;
using System.Linq;
using MRK.Scenes;
using UnityEngine;

namespace MRK
{
    public class DebugWindow : MonoBehaviour
    {
        private bool _visible;
        private Rect _windowRect;
        private Vector2 _scrollPos;
        private GUIStyle _sceneButtonStyle;
        private readonly Dictionary<BaseScene, bool> _sceneDebugStates;

        public DebugWindow()
        {
            _windowRect = new Rect(20f, 20f, 340f, 520f);
            _sceneDebugStates = new Dictionary<BaseScene, bool>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                _visible = !_visible;
        }

        private void OnGUI()
        {
            if (!_visible)
                return;

            _windowRect = GUI.Window(0x4D524B, _windowRect, DrawWindow, "MRK Debug");
        }

        private void DrawWindow(int id)
        {
            _sceneButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
            };

            GUILayout.BeginVertical();
            var mousePos = (Vector2)Input.mousePosition;
            mousePos.y = Screen.height - mousePos.y;
            GUILayout.Label($"Mouse Position: {mousePos}");

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);

            foreach (var scene in UIManager.Instance.Scenes)
            {
                var hasDebugUI = scene.UIComponents.Any(comp => comp.HasDebugUI);
                if (!hasDebugUI)
                    continue;

                var isShown = _sceneDebugStates.TryGetValue(scene, out var value) && value;
                if (
                    GUILayout.Button(
                        $"[{(isShown ? "x" : "")}] {scene.GetType().Name}",
                        _sceneButtonStyle
                    )
                )
                    isShown = !isShown;

                if (isShown != value)
                    _sceneDebugStates[scene] = isShown;

                if (isShown)
                {
                    GUILayout.BeginVertical();

                    foreach (var comp in scene.UIComponents)
                    {
                        if (comp.HasDebugUI)
                        {
                            GUILayout.Space(6f);
                            GUILayout.Label($"--- {comp.Name} ---");
                            comp.OnDebugWindow();
                            GUILayout.Space(6f);
                        }
                    }

                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private static void SectionLabel(string title)
        {
            GUILayout.Space(6f);
            GUILayout.Label($"--- {title} ---");
        }
    }
}

using UnityEngine;

namespace MRK.Scenes.Home.Components
{
    /// <summary>
    /// REF: <b>Home2_1_8_7983.swf::joshua.brittain.home.LeftSection::LeftSection</b>
    /// </summary>
    public class LeftSection : UIComponent
    {
        private const float SEPARATOR_WIDTH = 10f;
        private const float HOME_BUTTON_WIDTH = 80f;
        private const float HOME_BUTTON_HEIGHT = 42f;

        private Texture2D? _separatorTex;
        private SideButtonRenderer _sideButtonRenderer;

        public LeftSection()
        {
            Position = Vector2.zero;
        }

        public override void RefreshRect()
        {
            Size = new Vector2(100f, Screen.height);
        }

        public override void OnLoadTextures()
        {
            _separatorTex = TextureManager.FromResource("HomeLeftSepSymbol");

            _sideButtonRenderer = new SideButtonRenderer();
            if (!_sideButtonRenderer.Initialize())
            {
                Logger.LogError($"Failed to initialize {nameof(SideButtonRenderer)}");
                return;
            }
        }

        public override void OnGUI()
        {
            // Draw separator texture
            var sepRect = new Rect(Size.x - SEPARATOR_WIDTH, 0f, SEPARATOR_WIDTH, Size.y);
            if (_separatorTex != null)
            {
                GUI.DrawTexture(sepRect, _separatorTex);
            }

            // Draw content
            var contentPadding = new RectOffset(10, (int)SEPARATOR_WIDTH, 22, 0);
            var contentRect = new Rect(
                contentPadding.left,
                contentPadding.top,
                Size.x - contentPadding.horizontal,
                Size.y - contentPadding.vertical
            );
            GUILayout.BeginArea(contentRect);
            GUILayout.BeginVertical();

            // Home button
            if (
                Renderer.RenderMainMenuButton(
                    "Home",
                    14,
                    Color.yellow,
                    null,
                    GUILayout.Width(HOME_BUTTON_WIDTH),
                    GUILayout.Height(HOME_BUTTON_HEIGHT)
                )
            )
            {
                Logger.Log("HOMEEEEEEEE");
            }

            // Side buttons
            var sideMargin = new RectOffset(5, 0, 25, 0);

            if (
                _sideButtonRenderer.RenderSideIconButton(
                    Renderer.Icons.Character,
                    "Customization",
                    sideMargin
                )
            )
            {
                Logger.Log("CUSSS");
            }

            if (_sideButtonRenderer.RenderSideIconButton(Renderer.Icons.Chest, "Shop", sideMargin))
            {
                Logger.Log("SHOPPP");
            }

            if (
                _sideButtonRenderer.RenderSideIconButton(
                    Renderer.Icons.Medal,
                    "Ranked Stats",
                    sideMargin
                )
            )
            {
                Logger.Log("RANKED STATSSSS");
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}

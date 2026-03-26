using UnityEngine;

namespace MRK.Scenes.Home.Components
{
    /// <summary>
    /// REF: <b>Home2_1_8_7983.swf::joshua.brittain.home.TopSection::TopSection</b>
    /// </summary>
    public partial class TopSection : UIComponent
    {
        private const float HEIGHT = 75f;
        private const float SEPARATOR_HEIGHT = 10f;
        private const float PLAY_BUTTON_WIDTH = 100f;
        private const float PLAY_BUTTON_HEIGHT = 50f;
        private const float ICON_BUTTON_SIZE = 50f;
        private const float ACC_DISPLAY_WIDTH = 252f;
        private const float ACC_DISPLAY_HEIGHT = 52f;

        private Texture2D? _separatorTex;
        private Texture2D? _accDisplayTex;

        private readonly LeftSection _leftSection;

        public override bool HasDebugUI => true;

        public TopSection(LeftSection leftSection)
        {
            _leftSection = leftSection;
        }

        public override void RefreshRect()
        {
            Position = new Vector2(_leftSection.Rect.xMax, 0f);
            Size = new Vector2(Screen.width - _leftSection.Rect.width, HEIGHT);
        }

        public override void OnLoadTextures()
        {
            _separatorTex = TextureManager.FromResource("HomeSepSymbol");
            _accDisplayTex = TextureManager.FromResource("HomeAccountDisplaySymbol");
        }

        public override void OnGUI()
        {
            // Draw separator
            var sepRect = new Rect(0f, Size.y - SEPARATOR_HEIGHT, Size.x, SEPARATOR_HEIGHT);
            if (_separatorTex != null)
                GUI.DrawTexture(sepRect, _separatorTex);

            // Draw content
            var contentPadding = new RectOffset(15, 15, 0, 0);
            var contentRect = new Rect(
                contentPadding.left,
                contentPadding.top,
                Size.x - contentPadding.horizontal,
                Size.y - SEPARATOR_HEIGHT
            );

            // Draw play button centered horizontally on screen
            var playRect = new Rect(
                Screen.width / 2f - Position.x - contentRect.x - PLAY_BUTTON_WIDTH / 2f,
                (contentRect.height - PLAY_BUTTON_HEIGHT) / 2f,
                PLAY_BUTTON_WIDTH,
                PLAY_BUTTON_HEIGHT
            );
            if (Renderer.RenderMainMenuButton(playRect, "PLAY", 16, Color.yellow))
            {
                Logger.Log("PLAYYYYYY");
            }

            GUILayout.BeginArea(contentRect);
            GUILayout.BeginHorizontal();

            // Left
            DrawActionButtons();

            GUILayout.FlexibleSpace();

            // Right
            // Reserve slot for account display
            var accountDisplaySlot = GUILayoutUtility.GetRect(
                ACC_DISPLAY_WIDTH,
                ACC_DISPLAY_WIDTH,
                0,
                float.MaxValue
            );

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            // Draw account display in the reserved slot
            DrawAccountDisplay(
                new Rect(
                    contentRect.x + accountDisplaySlot.x,
                    contentRect.y + accountDisplaySlot.y,
                    accountDisplaySlot.width,
                    accountDisplaySlot.height
                )
            );
        }

        private void DrawActionButtons()
        {
            // Settings
            if (
                CenterVertically(() =>
                    Renderer.RenderIconButton(
                        Renderer.Icons.WoodenWheel,
                        "Settings",
                        9f,
                        null,
                        GUILayout.Width(ICON_BUTTON_SIZE),
                        GUILayout.Height(ICON_BUTTON_SIZE)
                    )
                )
            )
            {
                Logger.Log("SETTINGS");
            }

            // Cauldron
            DrawCauldronButton();
        }

        private void DrawAccountDisplay(Rect slot)
        {
            var accDisplayRect = new Rect(
                slot.x,
                slot.y + (slot.height - ACC_DISPLAY_HEIGHT) / 2f,
                ACC_DISPLAY_WIDTH,
                ACC_DISPLAY_HEIGHT
            );
            if (_accDisplayTex != null)
                GUI.DrawTexture(accDisplayRect, _accDisplayTex);

            // Content
            var displayContentRect = new Rect(
                accDisplayRect.x + 55f,
                accDisplayRect.y + 5f,
                accDisplayRect.width - 58f,
                accDisplayRect.height - 10f
            );

            // Slot has 0 height in layout pass, so GUILayout won't work correctly if we use it directly
            // SOL: defer layout to new group
            GUI.BeginGroup(displayContentRect);
            GUILayout.BeginArea(
                new Rect(0f, 0f, displayContentRect.width, displayContentRect.height)
            );
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            // Account name
            GUILayout.Label(
                UIManager.DataProvider.User.Username,
                Renderer.GetLabelStyle(12, Color.white, FontStyle.Bold)
            );

            // Town points
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (Renderer.Icons.GoldCoin != null)
            {
                GUILayout.Label(
                    new GUIContent(Renderer.Icons.GoldCoin),
                    Renderer.GetLabelStyle(12, Color.white),
                    GUILayout.Width(16f),
                    GUILayout.Height(16f)
                );
            }

            GUILayout.Label(
                UIManager.DataProvider.User.TownPoints.ToString(),
                Renderer.GetLabelStyle(12, Color.white),
                GUILayout.Height(16f)
            );

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUI.EndGroup();
        }

        public override void OnDebugWindow()
        {
            GUILayout.Label("Cauldron Time Override");
            GUILayout.BeginHorizontal();

            CauldronSettings.TimeOverrideEnabled = GUILayout.Toggle(
                CauldronSettings.TimeOverrideEnabled,
                "Enabled"
            );
            if (CauldronSettings.TimeOverrideEnabled)
            {
                GUILayout.Label("Secs", GUILayout.ExpandWidth(false));

                var overrideStr = GUILayout.TextField(
                    ((int)CauldronSettings.TimeOverride).ToString(),
                    GUILayout.Width(60f)
                );
                if (int.TryParse(overrideStr, out var overrideSecs))
                    CauldronSettings.TimeOverride = overrideSecs;
            }

            GUILayout.EndHorizontal();
        }
    }
}

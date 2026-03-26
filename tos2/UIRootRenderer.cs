using MRK.Textures;
using UnityEngine;

namespace MRK
{
    public class UIRootRenderer : UIRenderer
    {
        private MainMenuButtonTextures _mainMenuButtonTextures;
        private GUIStyle _mainMenuButtonGuiStyle;

        private IconButtonTextures _iconButtonTextures;
        private GUIStyle _iconButtonGuiStyle;

        private GUIStyle _normalLabelStyle;

        public Icons Icons { get; private set; }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            // Load button textures
            _mainMenuButtonTextures = LoadButtonTextures<MainMenuButtonTextures>(
                "MainMenuButtonSpriteAtlas"
            );
            _iconButtonTextures = LoadButtonTextures<IconButtonTextures>("IconButtonSpriteAtlas");

            // Load icons
            var iconAtlasTex = TextureManager.FromResource("IconAtlas");
            if (iconAtlasTex == null)
            {
                Logger.Throw("Failed to load icon atlas");
            }
            Icons = new Icons(new TextureAtlas(iconAtlasTex));

            Initialized = true;
            return true;
        }

        public bool RenderMainMenuButton(
            Rect rect,
            string content,
            int fontSize = 18,
            Color? color = null
        )
        {
            _mainMenuButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                font = UIManager.FontRuncible,
                fontSize = 18,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { background = _mainMenuButtonTextures.Idle },
                hover = { background = _mainMenuButtonTextures.Hover },
                active = { background = _mainMenuButtonTextures.Down },
            };

            color ??= Color.white;
            _mainMenuButtonGuiStyle.normal.textColor = color.Value;
            _mainMenuButtonGuiStyle.hover.textColor = color.Value;
            _mainMenuButtonGuiStyle.active.textColor = color.Value;
            _mainMenuButtonGuiStyle.fontSize = fontSize;

            return GUI.Button(rect, content, _mainMenuButtonGuiStyle);
        }

        public bool RenderIconButton(
            Rect rect,
            Texture2D icon,
            string tooltip = "",
            float shrink = 6f
        )
        {
            _iconButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { background = _iconButtonTextures.Idle },
                hover = { background = _iconButtonTextures.Hover },
                active = { background = _iconButtonTextures.Down },
            };

            bool clicked = GUI.Button(
                rect,
                new GUIContent(string.Empty, tooltip),
                _iconButtonGuiStyle
            );
            var iconRect = rect.Shrink(shrink);
            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);

            return clicked;
        }

        public void RenderLabel(
            Rect rect,
            string content,
            int fontSize = 14,
            Color? color = null,
            FontStyle fontStyle = FontStyle.Normal,
            Font? font = null
        )
        {
            var style = GetLabelStyle(fontSize, color, fontStyle, font);
            GUI.Label(rect, content, style);
        }

        public void RenderLabelWithEffects(
            Rect rect,
            string content,
            int fontSize = 14,
            Color? color = null,
            FontStyle fontStyle = FontStyle.Normal,
            float glowRadius = 2f,
            Color? glowColor = null,
            float shadowDist = 2f,
            Color? shadowColor = null,
            Font? font = null
        )
        {
            var glow = glowColor ?? Color.black;
            var shadow = shadowColor ?? new Color(0f, 0f, 0f, 0.8f);

            // 8-direction glow passes
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    RenderLabel(
                        rect.Add(new Rect(dx * glowRadius, dy * glowRadius, 0f, 0f)),
                        content,
                        fontSize,
                        glow,
                        fontStyle,
                        font
                    );
                }
            }

            // Drop shadow
            RenderLabel(
                rect.Add(new Rect(shadowDist, shadowDist, 0f, 0f)),
                content,
                fontSize,
                shadow,
                fontStyle,
                font
            );

            // Main text
            RenderLabel(rect, content, fontSize, color, fontStyle, font);
        }

        public GUIStyle GetLabelStyle(
            int fontSize = 14,
            Color? color = null,
            FontStyle fontStyle = FontStyle.Normal,
            Font? font = null
        )
        {
            _normalLabelStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 0, 0),
            };

            color ??= Color.white;
            _normalLabelStyle.normal.textColor = color.Value;
            _normalLabelStyle.hover.textColor = color.Value;
            _normalLabelStyle.active.textColor = color.Value;

            _normalLabelStyle.fontSize = fontSize;
            _normalLabelStyle.fontStyle = fontStyle;
            _normalLabelStyle.font = font; // null = Arial
            return _normalLabelStyle;
        }

        // ===================================================================
        // GUILayout variants
        // ===================================================================

        public bool RenderMainMenuButton(
            string content,
            int fontSize = 18,
            Color? color = null,
            RectOffset? margin = null,
            params GUILayoutOption[] options
        )
        {
            _mainMenuButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                font = UIManager.FontRuncible,
                fontSize = 18,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { background = _mainMenuButtonTextures.Idle },
                hover = { background = _mainMenuButtonTextures.Hover },
                active = { background = _mainMenuButtonTextures.Down },
            };

            color ??= Color.white;
            _mainMenuButtonGuiStyle.normal.textColor = color.Value;
            _mainMenuButtonGuiStyle.hover.textColor = color.Value;
            _mainMenuButtonGuiStyle.active.textColor = color.Value;
            _mainMenuButtonGuiStyle.fontSize = fontSize;

            var originalMargin = _mainMenuButtonGuiStyle.margin;
            if (margin != null)
                _mainMenuButtonGuiStyle.margin = margin;

            bool clicked = GUILayout.Button(content, _mainMenuButtonGuiStyle, options);

            _mainMenuButtonGuiStyle.margin = originalMargin;
            return clicked;
        }

        public bool RenderIconButton(
            Texture2D icon,
            string tooltip = "",
            float shrink = 6f,
            RectOffset? margin = null,
            params GUILayoutOption[] options
        )
        {
            _iconButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { background = _iconButtonTextures.Idle },
                hover = { background = _iconButtonTextures.Hover },
                active = { background = _iconButtonTextures.Down },
            };

            var originalMargin = _iconButtonGuiStyle.margin;
            if (margin != null)
                _iconButtonGuiStyle.margin = margin;

            bool clicked = GUILayout.Button(
                new GUIContent(string.Empty, tooltip),
                _iconButtonGuiStyle,
                options
            );

            _iconButtonGuiStyle.margin = originalMargin;

            GUI.DrawTexture(
                GUILayoutUtility.GetLastRect().Shrink(shrink),
                icon,
                ScaleMode.ScaleToFit
            );

            return clicked;
        }

        public void RenderLabel(
            string content,
            int fontSize = 14,
            Color? color = null,
            FontStyle fontStyle = FontStyle.Normal,
            Font? font = null,
            RectOffset? margin = null,
            params GUILayoutOption[] options
        )
        {
            var style = GetLabelStyle(fontSize, color, fontStyle, font);
            var originalMargin = style.margin;
            if (margin != null)
                style.margin = margin;

            GUILayout.Label(content, style, options);
            style.margin = originalMargin;
        }

        public void RenderLabelWithEffects(
            string content,
            int fontSize = 14,
            Color? color = null,
            FontStyle fontStyle = FontStyle.Normal,
            float glowRadius = 2f,
            Color? glowColor = null,
            float shadowDist = 2f,
            Color? shadowColor = null,
            Font? font = null,
            RectOffset? margin = null,
            params GUILayoutOption[] options
        )
        {
            var glow = glowColor ?? Color.black;
            var shadow = shadowColor ?? new Color(0f, 0f, 0f, 0.8f);

            // Reserve layout space using the final text style so sizing is accurate
            var style = GetLabelStyle(fontSize, color, fontStyle, font);
            var originalMargin = style.margin;
            if (margin != null)
                style.margin = margin;

            var rect = GUILayoutUtility.GetRect(new GUIContent(content), style, options);
            style.margin = originalMargin; // restore before the rect-based passes

            // 8-direction glow passes
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    RenderLabel(
                        rect.Add(new Rect(dx * glowRadius, dy * glowRadius, 0f, 0f)),
                        content,
                        fontSize,
                        glow,
                        fontStyle,
                        font
                    );
                }
            }

            // Drop shadow
            RenderLabel(
                rect.Add(new Rect(shadowDist, shadowDist, 0f, 0f)),
                content,
                fontSize,
                shadow,
                fontStyle,
                font
            );

            // Main text
            RenderLabel(rect, content, fontSize, color, fontStyle, font);
        }
    }
}

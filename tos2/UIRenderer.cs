using MRK.Textures;
using System;
using UnityEngine;

namespace MRK
{
    public class UIRenderer
    {
        private bool _initialized;
        private Icons _icons;

        private MainMenuButtonTextures _mainMenuButtonTextures;
        private GUIStyle _mainMenuButtonGuiStyle;

        private IconButtonTextures _iconButtonTextures;
        private GUIStyle _iconButtonGuiStyle;

        private SideIconButtonTextures _sideIconButtonTextures;
        private GUIStyle _sideIconButtonGuiStyle;

        public Icons Icons => _icons;

        private UIManager UIManager => UIManager.Instance;
        private TextureManager TextureManager => UIManager.TextureManager;

        public bool Initialize()
        {
            if (_initialized)
            {
                Logger.Log("UIRenderer is already initialized.");
                return true;
            }

            // Load button textures
            _mainMenuButtonTextures = LoadButtonTextures<MainMenuButtonTextures>("MainMenuButtonSpriteAtlas");
            _iconButtonTextures = LoadButtonTextures<IconButtonTextures>("IconButtonSpriteAtlas");
            _sideIconButtonTextures = LoadButtonTextures<SideIconButtonTextures>("IconButtonSpriteAtlas"); // TODO: reuse atlas?

            // Load icons
            var iconAtlasTex = TextureManager.FromResource("IconAtlas");
            if (iconAtlasTex == null)
            {
                Logger.Throw("Failed to load icon atlas");
            }
            _icons = new Icons(new TextureAtlas(iconAtlasTex));

            _initialized = true;
            return true;
        }

        private T LoadButtonTextures<T>(string atlasTexName) where T : BaseButtonTextures
        {
            var atlasTex = TextureManager.FromResource(atlasTexName);
            if (atlasTex == null)
            {
                Logger.Throw("Failed to load texture atlas '{0}' for button textures.", atlasTexName);
            }

            return (T)Activator.CreateInstance(typeof(T), new TextureAtlas(atlasTex));
        }

        public bool RenderMainMenuButton(Rect rect, string content, int fontSize = 18, Color? color = null)
        {
            _mainMenuButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                font = UIManager.FontRuncible,
                fontSize = 18,
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(0, 0, 0, 0),
                normal = { background = _mainMenuButtonTextures.Idle },
                hover = { background = _mainMenuButtonTextures.Hover },
                active = { background = _mainMenuButtonTextures.Down }
            };

            color ??= Color.white;
            _mainMenuButtonGuiStyle.normal.textColor = color.Value;
            _mainMenuButtonGuiStyle.hover.textColor = color.Value;
            _mainMenuButtonGuiStyle.active.textColor = color.Value;
            _mainMenuButtonGuiStyle.fontSize = fontSize;

            return GUI.Button(rect, content, _mainMenuButtonGuiStyle);
        }

        public bool RenderIconButton(Rect rect, Texture2D icon, string tooltip = "")
        {
            _iconButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(0, 0, 0, 0),
                normal = { background = _iconButtonTextures.Idle },
                hover = { background = _iconButtonTextures.Hover },
                active = { background = _iconButtonTextures.Down }
            };

            bool clicked = GUI.Button(rect, new GUIContent(string.Empty, tooltip), _iconButtonGuiStyle);
            GUI.DrawTexture(rect.Shrink(10f), icon, ScaleMode.ScaleToFit);
            return clicked;
        }

        public bool RenderSideIconButton(Rect rect, Texture2D icon, string tooltip = "")
        {
            _sideIconButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(0, 0, 0, 0),
                normal = { background = _sideIconButtonTextures.Idle },
                hover = { background = _sideIconButtonTextures.Hover },
                active = { background = _sideIconButtonTextures.Down }
            };

            bool clicked = GUI.Button(rect, new GUIContent(string.Empty, tooltip), _sideIconButtonGuiStyle);
            GUI.DrawTexture(rect.Add(new Rect(11f, 20f, -26f, -26f)), icon, ScaleMode.ScaleToFit);
            return clicked;
        }
    }
}

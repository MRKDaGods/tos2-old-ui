using MRK.Textures;
using UnityEngine;

namespace MRK.Scenes.Home.Components
{
    public class SideButtonRenderer : UIRenderer
    {
        private SideIconButtonTextures _sideIconButtonTextures;
        private GUIStyle _sideIconButtonGuiStyle;

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            _sideIconButtonTextures = LoadButtonTextures<SideIconButtonTextures>(
                "IconButtonSpriteAtlas"
            );
            return true;
        }

        public bool RenderSideIconButton(Rect rect, Texture2D icon, string tooltip = "")
        {
            _sideIconButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(0, 0, 0, 0),
                normal = { background = _sideIconButtonTextures.Idle },
                hover = { background = _sideIconButtonTextures.Hover },
                active = { background = _sideIconButtonTextures.Down },
            };

            bool clicked = GUI.Button(
                rect,
                new GUIContent(string.Empty, tooltip),
                _sideIconButtonGuiStyle
            );
            GUI.DrawTexture(rect.Add(new Rect(11f, 20f, -26f, -26f)), icon, ScaleMode.ScaleToFit);
            return clicked;
        }

        public bool RenderSideIconButton(
            Texture2D icon,
            string tooltip = "",
            RectOffset? margin = null
        )
        {
            _sideIconButtonGuiStyle ??= new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(0, 0, 0, 0),
                normal = { background = _sideIconButtonTextures.Idle },
                hover = { background = _sideIconButtonTextures.Hover },
                active = { background = _sideIconButtonTextures.Down },
            };

            var originalMargin = _sideIconButtonGuiStyle.margin;
            if (margin != null)
                _sideIconButtonGuiStyle.margin = margin;

            bool clicked = GUILayout.Button(
                new GUIContent(string.Empty, tooltip),
                _sideIconButtonGuiStyle,
                GUILayout.Width(75f),
                GUILayout.Height(75f)
            );

            _sideIconButtonGuiStyle.margin = originalMargin;

            var rect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(rect.Add(new Rect(11f, 20f, -26f, -26f)), icon, ScaleMode.ScaleToFit);

            return clicked;
        }
    }
}

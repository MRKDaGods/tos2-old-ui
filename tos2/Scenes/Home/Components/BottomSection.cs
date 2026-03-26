using MRK.Textures;
using UnityEngine;

namespace MRK.Scenes.Home.Components
{
    /// <summary>
    /// REF: <b>Home2_1_8_7983.swf::joshua.brittain.home.BottomSection::BottomSection</b>
    /// </summary>
    public class BottomSection : UIComponent
    {
        private const float HEIGHT = 55f;
        private const float SEPARATOR_HEIGHT = 10f;
        private const float BUTTONS_WIDTH = 100f;
        private const float BUTTONS_HEIGHT = 40f;

        private Texture2D? _separatorTex;

        // Friends list
        private Texture2D? _friendListBgTex;
        private bool _showFriendsList;

        private readonly LeftSection _leftSection;

        public BottomSection(LeftSection leftSection)
        {
            _leftSection = leftSection;
        }

        public override void RefreshRect()
        {
            Position = new Vector2(_leftSection.Rect.xMax, Screen.height - HEIGHT);
            Size = new Vector2(Screen.width - _leftSection.Rect.width, HEIGHT);
        }

        public override void OnLoadTextures()
        {
            _separatorTex = TextureManager.FromResource("HomeSepSymbol");
            _friendListBgTex = TextureAtlas.Widgets.AddTextureXY(
                "FriendListBg",
                new RectInt(0, 0, 140, 321)
            );
        }

        public override void OnGUI()
        {
            // Draw separator
            var sepRect = new Rect(0f, 0f, Size.x, SEPARATOR_HEIGHT);
            if (_separatorTex != null)
                GUI.DrawTexture(sepRect, _separatorTex);

            var contentRect = new Rect(0f, sepRect.height, Size.x, Size.y - sepRect.height);
            GUILayout.BeginArea(contentRect);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            var friends = UIManager.Instance.DataProvider.Friends;
            if (
                CenterVertically(() =>
                    Renderer.RenderMainMenuButton(
                        $"Friends ({friends.Count})",
                        null,
                        FontStyle.Bold,
                        11,
                        Color.yellow,
                        null,
                        GUILayout.Width(BUTTONS_WIDTH),
                        GUILayout.Height(BUTTONS_HEIGHT)
                    )
                )
            )
            {
                Logger.Log("FRIENDS");
                _showFriendsList = !_showFriendsList;
            }

            GUILayout.Space(1f);

            if (
                CenterVertically(() =>
                    Renderer.RenderMainMenuButton(
                        "Notifications (2)",
                        null,
                        FontStyle.Bold,
                        11,
                        Color.yellow,
                        null,
                        GUILayout.Width(BUTTONS_WIDTH),
                        GUILayout.Height(BUTTONS_HEIGHT)
                    )
                )
            )
            {
                Logger.Log("NOTIFICATIONS");
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public override void OnUnconstrainedGUI()
        {
            if (_showFriendsList)
            {
                var width = 171f;
                var height = 400f;
                var rect = new Rect(Rect.xMax - width, Rect.yMin - height, width, height);

                if (_friendListBgTex != null)
                    GUI.DrawTexture(rect, _friendListBgTex);
            }
        }
    }
}

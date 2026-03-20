using Home.HomeScene;
using UnityEngine;

namespace MRK.SceneHandlers
{
    public class HomeSceneHandler : BaseSceneHandler
    {
        private Texture2D? _homeBackgroundTex;
        private Texture2D? _homeLeftSepSymbolTex;

        public override void OnSceneActivated()
        {
            base.OnSceneActivated();

            // Disable HomeSceneCustomizations
            DisableObjectWithType<HomeScenePersonalizations>();

            // Disable HomeTosMapHouses
            DisableObjectByName("HomeTosMapHouses");

            // Disable HomeUI
            DisableObjectWithType<HomeSceneController>();
        }

        public override void OnLoadTextures()
        {
            base.OnLoadTextures();

            _homeBackgroundTex = Textures.FromResource("HomeBackground");
            if (_homeBackgroundTex == null)
            {
                Logger.Log("Failed to load HomeBackground texture");
            }

            _homeLeftSepSymbolTex = Textures.FromResource("HomeLeftSepSymbol");
            if (_homeLeftSepSymbolTex == null)
            {
                Logger.Log("Failed to load HomeLeftSepSymbol texture");
            }
        }

        private void OnGUI()
        {
            // Draw home background
            if (_homeBackgroundTex != null)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _homeBackgroundTex);
            }

            // HomeScreen consists of 3 sections
            // Left, Top, Center

            // Left: 100x600
            if (_homeLeftSepSymbolTex != null)
            {
                float sepWidth = 10f;
                GUI.DrawTexture(new Rect(100f - sepWidth, 0f, sepWidth, Screen.height), _homeLeftSepSymbolTex);
            }
        }
    }
}

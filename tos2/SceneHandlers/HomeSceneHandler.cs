using Home.HomeScene;
using System.Collections;
using UnityEngine;

namespace MRK.SceneHandlers
{
    public class HomeSceneHandler : BaseSceneHandler
    {
        private Texture2D? _homeBackgroundTex;
        private Texture2D? _homeLeftSepSymbolTex;

        public override IEnumerator OnSceneActivated()
        {
            yield return base.OnSceneActivated();

            yield return new WaitUntilWithCooldown(() => DisableObjectWithType<HomeScenePersonalizations>());
            yield return new WaitUntilWithCooldown(() => DisableObjectByName("HomeTosMapHouses"));
            yield return new WaitUntilWithCooldown(() => DisableObjectWithType<HomeSceneController>());
        }

        public override void OnLoadTextures()
        {
            base.OnLoadTextures();

            _homeBackgroundTex = TextureManager.FromResource("HomeBackground");
            _homeLeftSepSymbolTex = TextureManager.FromResource("HomeLeftSepSymbol");
        }

        private void OnGUI()
        {
            // Reset
            GUI.color = Color.white;
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;

            if (_homeBackgroundTex != null)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _homeBackgroundTex);

            // Separator
            if (_homeLeftSepSymbolTex != null)
            {
                //m_seperator.x = 100 - 10;
                //m_seperator.y = 0;
                //m_seperator.width = 10;
                //m_seperator.height = 600;

                // hmm the right padding seems off
                GUI.DrawTexture(new Rect(90f, 0f, 10f, Screen.height), _homeLeftSepSymbolTex);
            }

            // Left section
            // Home2_1_8_7983.swf::joshua.brittain.home.LeftSection::LeftSection

            //m_homeBtn.x = 10;
            //m_homeBtn.y = 22;
            //m_homeBtn.width = 80;
            //m_homeBtn.height = 42;
            float homeBtnX = 10f;
            float homeBtnY = 22f;
            float homeBtnHeight = 42f;
            if (Renderer.RenderMainMenuButton(
                new Rect(homeBtnX, homeBtnY, 80f, homeBtnHeight), "HOME", 14, Color.yellow))
            {
                Logger.Log("HOMEEEEEEEE");
            }

            //m_cusBtn.x = m_homeBtn.x + 5;
            //m_cusBtn.y = m_homeBtn.y + m_homeBtn.height + 25;
            float cusBtnY = homeBtnY + homeBtnHeight + 25f;
            if (Renderer.RenderSideIconButton(
                new Rect(homeBtnX + 5f, cusBtnY, 75f, 75f), Renderer.Icons.Character, "Customization"))
            {
                Logger.Log("CUSSS");
            }

            //m_shopBtn.x = m_cusBtn.x;
            //m_shopBtn.y = m_cusBtn.y + m_cusBtn.height + 25;
            float shopBtnY = cusBtnY + 75f + 25f;
            if (Renderer.RenderSideIconButton(
                new Rect(homeBtnX + 5f, shopBtnY, 75f, 75f), Renderer.Icons.Chest, "Shop"))
            {
                Logger.Log("SHOPPP");
            }
        }
    }
}


using System.Collections;
using Home.HomeScene;
using MRK.Scenes.Home.Components;
using UnityEngine;

namespace MRK.Scenes.Home
{
    public class HomeScene : BaseScene
    {
        private Texture2D? _homeBackgroundTex;
        private readonly LeftSection _leftSection;
        private readonly TopSection _topSection;

        public HomeScene()
        {
            _leftSection = new LeftSection();
            AddUIComponent(_leftSection);

            _topSection = new TopSection(_leftSection);
            AddUIComponent(_topSection);
        }

        public override IEnumerator OnSceneActivated()
        {
            yield return base.OnSceneActivated();

            // If running a shim, skip
            if (UIManager.Instance.IsRunningShim)
            {
                Logger.Log("Running shim, skipping object disable wait");
                yield break;
            }

            yield return new WaitUntilWithCooldown(() =>
                DisableObjectWithType<HomeScenePersonalizations>()
            );
            yield return new WaitUntilWithCooldown(() => DisableObjectByName("HomeTosMapHouses"));
            yield return new WaitUntilWithCooldown(() =>
                DisableObjectWithType<HomeSceneController>()
            );
        }

        public override void OnLoadTextures()
        {
            base.OnLoadTextures();

            _homeBackgroundTex = TextureManager.FromResource("HomeBackground");
        }

        public override void OnGUI()
        {
            if (_homeBackgroundTex != null)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _homeBackgroundTex);

            base.OnGUI();
        }
    }
}

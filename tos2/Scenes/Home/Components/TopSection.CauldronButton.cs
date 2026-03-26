using UnityEngine;

namespace MRK.Scenes.Home.Components
{
    public partial class TopSection
    {
        private static class CauldronSettings
        {
            public static int NormalFontSize = 11;
            public static int LargeFontSize = 18;
            public static float NormalXOffset = 2.5f;
            public static float LargeXOffset = 2f;
            public static float NormalYOffset = 27f;
            public static float LargeYOffset = 27f;
            public static float IconShrink = 6f;
            public static Color TextColor = new Color(1f, 191f / 255f, 0f);
            public static bool TimeOverrideEnabled = false;
            public static float TimeOverride = 0f;
        }

        private void DrawCauldronButton()
        {
            var cauldronTimeLeftSec = CauldronSettings.TimeOverrideEnabled
                ? (int)CauldronSettings.TimeOverride
                : UIManager.DataProvider.User.CauldronTimeLeft;
            var cauldronNotFilled = cauldronTimeLeftSec > 0;
            if (
                CenterVertically(() =>
                    Renderer.RenderIconButton(
                        cauldronNotFilled
                            ? Renderer.Icons.CauldronEmpty
                            : Renderer.Icons.CauldronReady,
                        "Cauldron",
                        CauldronSettings.IconShrink,
                        null,
                        GUILayout.Width(ICON_BUTTON_SIZE),
                        GUILayout.Height(ICON_BUTTON_SIZE)
                    )
                )
            )
            {
                Logger.Log($"CAULDRON filled={!cauldronNotFilled}");
            }

            var cauldronBtnRect = GUILayoutUtility.GetLastRect();

            // Render status text
            var isCauldronLargeText = cauldronNotFilled && cauldronTimeLeftSec <= 60;
            var cauldronStatusText = cauldronNotFilled
                ? FormatCauldronTime(cauldronTimeLeftSec)
                : "Daily Brew";

            // Runcible when ready, Arial when on cooldown
            var cauldronFont = cauldronNotFilled ? null : UIManager.FontRuncible;
            Renderer.RenderLabelWithEffects(
                new Rect(
                    cauldronBtnRect.x
                        + (
                            isCauldronLargeText
                                ? CauldronSettings.LargeXOffset
                                : CauldronSettings.NormalXOffset
                        ),
                    cauldronBtnRect.y
                        + (
                            isCauldronLargeText
                                ? CauldronSettings.LargeYOffset
                                : CauldronSettings.NormalYOffset
                        ),
                    cauldronBtnRect.width,
                    20f
                ),
                cauldronStatusText,
                isCauldronLargeText
                    ? CauldronSettings.LargeFontSize
                    : CauldronSettings.NormalFontSize,
                CauldronSettings.TextColor,
                cauldronNotFilled ? FontStyle.Bold : FontStyle.Normal,
                glowRadius: 1f,
                shadowDist: 2f,
                font: cauldronFont
            );
        }

        private static string FormatCauldronTime(int totalSeconds)
        {
            if (totalSeconds >= 3600)
                return $"{totalSeconds / 3600}h {(totalSeconds % 3600) / 60:D2}m";

            if (totalSeconds <= 60)
                return $"{totalSeconds}s";

            return $"{totalSeconds / 60}m {totalSeconds % 60:D2}s";
        }
    }
}

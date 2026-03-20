using UnityEngine;

namespace MRK
{
    public class Entrypoint
    {
        public static bool Main()
        {
            Logger.Initialize();

            Logger.Log("Initializing UIManager...");

            // Typical init
            var go = new GameObject("mrk-tos2-old-ui");
            go.AddComponent<UIManager>();
            Object.DontDestroyOnLoad(go);

            return true;
        }
    }
}

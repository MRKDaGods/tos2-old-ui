using MRK.Data;
using UnityEngine;

namespace MRK
{
    public class Entrypoint
    {
        /// <summary>
        /// The data provider used by the UI manager.
        /// </summary>
        /// <remarks>
        /// This should be set before calling <see cref="Main"/>. <br/>
        /// I wont add this to the method sig of Main to avoid problems with mono from cpp.
        /// </remarks>
        public static IDataProvider DataProvider { get; set; }

        public static bool Main()
        {
            Logger.Initialize();

            Logger.Log("Initializing UIManager...");

            // Typical init
            var go = new GameObject("mrk-tos2-old-ui");
            var uiManager = go.AddComponent<UIManager>();
            uiManager.DataProvider = DataProvider ?? new ToSDataProvider();
            Object.DontDestroyOnLoad(go);

            return true;
        }
    }
}

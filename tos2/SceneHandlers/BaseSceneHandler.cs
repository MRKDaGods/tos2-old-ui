using UnityEngine;

namespace MRK.SceneHandlers
{
    public class BaseSceneHandler : MonoBehaviour
    {
        protected Textures Textures => UIManager.Instance.Textures;

        public virtual void OnSceneActivated()
        {
            Logger.Log($"{GetType().Name} activated");
        }

        public virtual void OnSceneDeactivated()
        {
            Logger.Log($"{GetType().Name} deactivated");
        }

        public virtual void OnLoadTextures()
        {
            Logger.Log($"{GetType().Name} loading textures");
        }

        protected bool DisableObjectWithType<T>() where T : MonoBehaviour
        {
            var obj = FindObjectOfType<T>();
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                Logger.Log($"Disabled GameObject with component '{typeof(T).Name}' in {GetType().Name}");
                return true;
            }
            else
            {
                Logger.Log($"No GameObject with component '{typeof(T).Name}' found in {GetType().Name}");
                return false;
            }
        }

        protected bool DisableObjectByName(string name)
        {
            var obj = GameObject.Find(name);
            if (obj != null)
            {
                obj.SetActive(false);
                Logger.Log($"Disabled GameObject '{name}' in {GetType().Name}");
                return true;
            }
            else
            {
                Logger.Log($"GameObject '{name}' not found in {GetType().Name}");
                return false;
            }
        }
    }
}

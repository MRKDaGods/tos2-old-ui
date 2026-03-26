using System;
using System.Collections;
using System.Collections.Generic;
using MRK.Textures;
using UnityEngine;

namespace MRK.Scenes
{
    public class WaitUntilWithCooldown : CustomYieldInstruction
    {
        private readonly Func<bool> _predicate;
        private readonly float _cooldown;
        private float _nextCheckTime;

        public WaitUntilWithCooldown(Func<bool> predicate, float cooldown = 0.1f)
        {
            _predicate = predicate;
            _cooldown = cooldown;
            _nextCheckTime = Time.time;
        }

        public override bool keepWaiting
        {
            get
            {
                if (Time.time < _nextCheckTime)
                    return true;

                _nextCheckTime = Time.time + _cooldown;
                return !_predicate();
            }
        }
    }

    public class BaseScene
    {
        public bool IsActive { get; set; }
        public List<UIComponent> UIComponents { get; private set; }

        protected UIManager UIManager => UIManager.Instance;
        protected TextureManager TextureManager => UIManager.TextureManager;
        protected UIRootRenderer Renderer => UIManager.Renderer;

        public BaseScene()
        {
            UIComponents = new List<UIComponent>();
        }

        public void AddUIComponent(UIComponent component)
        {
            UIComponents.Add(component);
        }

        public virtual void OnGUI()
        {
            foreach (var component in UIComponents)
            {
                // Make sure the rect is up to date
                component.RefreshRect();

                // Make sure we clip :P
                GUI.BeginGroup(new Rect(component.Position, component.Size));
                component.OnGUI();
                GUI.EndGroup();
            }
        }

        public virtual IEnumerator OnSceneActivated()
        {
            Logger.Log($"{GetType().Name} activated");
            yield break;
        }

        public virtual void OnSceneDeactivated()
        {
            Logger.Log($"{GetType().Name} deactivated");
        }

        public virtual void OnLoadTextures()
        {
            Logger.Log($"{GetType().Name} loading textures");

            foreach (var component in UIComponents)
            {
                component.OnLoadTextures();
            }
        }

        protected bool DisableObjectWithType<T>()
            where T : MonoBehaviour
        {
            var obj = UnityEngine.Object.FindObjectOfType<T>();
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                Logger.Log(
                    $"Disabled GameObject with component '{typeof(T).Name}' in {GetType().Name}"
                );
                return true;
            }
            else
            {
                Logger.Log(
                    $"No GameObject with component '{typeof(T).Name}' found in {GetType().Name}"
                );
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

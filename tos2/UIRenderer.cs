using System;
using MRK.Textures;

namespace MRK
{
    public abstract class UIRenderer
    {
        protected UIManager UIManager => UIManager.Instance;
        protected TextureManager TextureManager => UIManager.TextureManager;

        public bool Initialized { get; protected set; }

        public virtual bool Initialize()
        {
            if (Initialized)
            {
                Logger.Log($"ERROR {GetType().Name} is already initialized.");
                return false;
            }

            return true;
        }

        protected T LoadButtonTextures<T>(string atlasTexName)
            where T : BaseButtonTextures
        {
            var atlasTex = TextureManager.FromResource(atlasTexName);
            if (atlasTex == null)
            {
                Logger.Throw(
                    "Failed to load texture atlas '{0}' for button textures.",
                    atlasTexName
                );
            }

            return (T)Activator.CreateInstance(typeof(T), new TextureAtlas(atlasTex));
        }
    }
}

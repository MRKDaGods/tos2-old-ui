using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MRK.Textures
{
    public class TextureManager
    {
        private readonly Dictionary<string, Texture2D> _textureCache;

        public TextureManager()
        {
            _textureCache = new Dictionary<string, Texture2D>();
        }

        public Texture2D? LoadFromFile(string name, string path)
        {
            Texture2D? tex;
            if (_textureCache.TryGetValue(name, out tex))
            {
                Logger.Log($"Texture '{name}' already loaded");
                return tex;
            }

            try
            {
                var bytes = File.ReadAllBytes(path);
                tex = new Texture2D(2, 2);
                if (tex.LoadImage(bytes))
                {
                    _textureCache[name] = tex;
                    Logger.Log($"Loaded texture '{name}' from '{path}'");
                    return tex;
                }
                else
                {
                    Logger.Log($"Failed to load texture '{name}' from '{path}' - LoadImage failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception while loading texture '{name}' from '{path}': {ex.Message}");
                return null;
            }
        }

        public Texture2D? FromResource(string name)
        {
            var bytes = (byte[])TextureResources.ResourceManager.GetObject(name);
            if (bytes == null)
            {
                Logger.Log($"Texture resource '{name}' not found");
                return null;
            }

            var tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            _textureCache[name] = tex;
            Logger.Log($"Loaded texture '{name}' from embedded resources");

            return tex;
        }

        public Texture2D? this[string name] => _textureCache.TryGetValue(name, out var tex) ? tex : null;
    }
}

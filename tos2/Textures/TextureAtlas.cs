using System.Collections.Generic;
using UnityEngine;

namespace MRK.Textures
{
    public class TextureAtlas
    {
        private Texture2D _atlasTexture;
        private readonly Dictionary<string, Texture2D> _textures;

        public TextureAtlas(Texture2D atlasTexture)
        {
            _atlasTexture = atlasTexture;
            _textures = new Dictionary<string, Texture2D>();
        }

        public Texture2D AddTexture(string name, Rect uvRect)
        {
            // Fix inverted uvrect
            uvRect.y = 1f - uvRect.y - uvRect.height;

            var tex = new Texture2D((int)(uvRect.width * _atlasTexture.width), (int)(uvRect.height * _atlasTexture.height));
            var pixels = _atlasTexture.GetPixels((int)(uvRect.x * _atlasTexture.width), (int)(uvRect.y * _atlasTexture.height), tex.width, tex.height);
            tex.SetPixels(pixels);
            tex.Apply();

            _textures[name] = tex;

            return tex;
        }

        public Texture2D AddTextureXY(string name, RectInt xyRect)
        {
            var uvRect = new Rect(
                (float)xyRect.x / _atlasTexture.width,
                (float)xyRect.y / _atlasTexture.height,
                (float)xyRect.width / _atlasTexture.width,
                (float)xyRect.height / _atlasTexture.height
            );
            return AddTexture(name, uvRect);
        }

        public Texture2D? this[string name] => _textures.TryGetValue(name, out var tex) ? tex : null;
    }
}

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

        public Texture2D AddTextureUV(string name, Rect uvRect)
        {
            // Fix inverted uvrect
            uvRect.y = 1f - uvRect.y - uvRect.height;

            int x = Mathf.RoundToInt(uvRect.x * _atlasTexture.width);
            int y = Mathf.RoundToInt(uvRect.y * _atlasTexture.height);
            int w = Mathf.RoundToInt(uvRect.width * _atlasTexture.width);
            int h = Mathf.RoundToInt(uvRect.height * _atlasTexture.height);

            var tex = new Texture2D(w, h);
            tex.SetPixels(_atlasTexture.GetPixels(x, y, w, h));
            tex.Apply();

            _textures[name] = tex;
            return tex;
        }

        public Texture2D AddTextureXY(string name, RectInt xyRect)
        {
            // Work directly in pixel coordinates
            int flippedY = _atlasTexture.height - xyRect.y - xyRect.height;

            var tex = new Texture2D(xyRect.width, xyRect.height);
            tex.SetPixels(_atlasTexture.GetPixels(xyRect.x, flippedY, xyRect.width, xyRect.height));
            tex.Apply();

            _textures[name] = tex;
            return tex;
        }

        public Texture2D? this[string name] =>
            _textures.TryGetValue(name, out var tex) ? tex : null;
    }
}

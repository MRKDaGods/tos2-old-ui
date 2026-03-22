using UnityEngine;

namespace MRK.Textures
{
    public class Icons
    {
        private readonly TextureAtlas _atlas;

        public Texture2D Character => _atlas["CharacterIcon"]!;
        public Texture2D Chest => _atlas["ChestIcon"]!;

        public Icons(TextureAtlas atlas)
        {
            _atlas = atlas;

            _atlas.AddTextureXY("CharacterIcon", new RectInt(0, 0, 50, 50));
            _atlas.AddTextureXY("ChestIcon", new RectInt(50, 0, 50, 50));
        }
    }
}

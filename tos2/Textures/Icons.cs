using UnityEngine;

namespace MRK.Textures
{
    public class Icons
    {
        private readonly TextureAtlas _atlas;

        public Texture2D Character => _atlas["CharacterIcon"]!;
        public Texture2D Chest => _atlas["ChestIcon"]!;
        public Texture2D Medal => _atlas["MedalIcon"]!;
        public Texture2D WoodenWheel => _atlas["WoodenWheelIcon"]!;
        public Texture2D GoldCoin => _atlas["GoldCoinIcon"]!;
        public Texture2D CauldronReady => _atlas["CauldronReadyIcon"]!;
        public Texture2D CauldronEmpty => _atlas["CauldronEmptyIcon"]!;

        public Icons(TextureAtlas atlas)
        {
            _atlas = atlas;

            _atlas.AddTextureXY("CharacterIcon", new RectInt(0, 0, 50, 50));
            _atlas.AddTextureXY("ChestIcon", new RectInt(50, 0, 50, 50));
            _atlas.AddTextureXY("MedalIcon", new RectInt(100, 0, 30, 50));

            _atlas.AddTextureXY("WoodenWheelIcon", new RectInt(0, 50, 30, 30));
            _atlas.AddTextureXY("GoldCoinIcon", new RectInt(30, 50, 30, 27));

            _atlas.AddTextureXY("CauldronReadyIcon", new RectInt(0, 80, 43, 39));
            _atlas.AddTextureXY("CauldronEmptyIcon", new RectInt(43, 80, 43, 39));
        }
    }
}

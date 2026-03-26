using UnityEngine;

namespace MRK.Textures
{
    public abstract class BaseButtonTextures
    {
        protected readonly TextureAtlas _atlas;

        public Texture2D Idle { get; protected set; }
        public Texture2D Hover { get; protected set; }
        public Texture2D Down { get; protected set; }

        public BaseButtonTextures(TextureAtlas atlas)
        {
            _atlas = atlas;
            LoadTextures();
        }

        protected abstract void LoadTextures();
    }

    public class MainMenuButtonTextures : BaseButtonTextures
    {
        public MainMenuButtonTextures(TextureAtlas atlas)
            : base(atlas) { }

        protected override void LoadTextures()
        {
            Idle = _atlas.AddTextureXY("MainMenuButtonIdle", new RectInt(29, 202, 82, 22));
            Hover = _atlas.AddTextureXY("MainMenuButtonHover", new RectInt(29, 170, 82, 22));
            Down = _atlas.AddTextureXY("MainMenuButtonDown", new RectInt(29, 138, 82, 22));
        }
    }

    public class IconButtonTextures : BaseButtonTextures
    {
        public IconButtonTextures(TextureAtlas atlas)
            : base(atlas) { }

        protected override void LoadTextures()
        {
            Idle = _atlas.AddTextureXY("IconButtonIdle", new RectInt(50, 0, 50, 50));
            Hover = _atlas.AddTextureXY("IconButtonHover", new RectInt(100, 0, 50, 50));
            Down = _atlas.AddTextureXY("IconButtonDown", new RectInt(0, 0, 50, 50));
        }
    }

    public class SideIconButtonTextures : BaseButtonTextures
    {
        public SideIconButtonTextures(TextureAtlas atlas)
            : base(atlas) { }

        protected override void LoadTextures()
        {
            Idle = _atlas.AddTextureXY("SideIconButtonIdle", new RectInt(0, 50, 76, 76));
            Hover = _atlas.AddTextureXY("SideIconButtonHover", new RectInt(76, 50, 76, 76));
            Down = _atlas.AddTextureXY("SideIconButtonDown", new RectInt(152, 50, 76, 76));
        }
    }
}

using System;
using MRK.Textures;
using UnityEngine;

namespace MRK
{
    public class UIComponent
    {
        // Absolute UI props
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Rect Rect => new Rect(Position, Size);

        public virtual string Name => GetType().Name;
        public virtual bool HasDebugUI => false;

        protected UIManager UIManager => UIManager.Instance;
        protected TextureManager TextureManager => UIManager.TextureManager;
        protected UIRootRenderer Renderer => UIManager.Renderer;

        /// <summary>
        /// Called when the UI component is loaded to load textures.
        /// </summary>
        public virtual void OnLoadTextures() { }

        /// <summary>
        /// Called every frame to draw the UI component.
        /// </summary>
        /// <remarks>
        /// You would mostly want to use relative positioning and sizing here.
        /// </remarks>
        public virtual void OnGUI() { }

        /// <summary>
        /// Called every frame when the debug window is open.
        /// Useful for drawing debug information.
        /// </summary>
        public virtual void OnDebugWindow() { }

        /// <summary>
        /// Optionally refresh our position and size due to screen resizing.
        /// </summary>
        public virtual void RefreshRect() { }

        public Rect RelativeToAbsoluteRect(Rect relativeRect)
        {
            return relativeRect.ToAbsolute(Position);
        }

        // Non void
        protected T CenterVertically<T>(Func<T> content)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            var result = content();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            return result;
        }

        // Void
        protected void CenterVertically(Action content)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            content();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}

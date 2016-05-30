using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine
{
    [Serializable]
    public class CollisionPlane : Visual2D
    {
        public Vector3 Direction { set; get; }
        public float Offset { set; get; }
        public Material Material { set; get; }

        public CollisionPlane(int offset,
            Vector3 direction,
            Material material,
            TextureType textureIn,
            Rectangle rectArea)
            : base(rectArea, textureIn)
        {
            this.Offset = offset;
            this.Direction = direction;
            this.Material = material;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}

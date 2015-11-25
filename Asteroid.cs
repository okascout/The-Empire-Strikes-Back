using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Asteroid: GameObject
    {

        public float size = 1;
        public Vector3 vel;

        private float scaling;
        private Matrix originalWorld;

        public Asteroid(LabGame game, Vector3 pos, Vector3 vel, float size)
        {
            this.game = game;
            type = GameObjectType.Enemy; // for now
            this.vel = vel;
            this.size = size;

            model = game.assets.GetModel("asteroid", createAsteroidModel);
            collisionRadius = size / 2;

            BoundingSphere modelBounds = model.CalculateBounds();
            scaling = size / modelBounds.Radius;
            originalWorld = Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z) * Matrix.Scaling(scaling);
            this.pos = pos;

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.camera.View,
                Projection = game.camera.Projection,
                //World = Matrix.Identity
                World = originalWorld

            };
        }

        public override void Update(GameTime gameTime)
        {
            pos += vel;
            basicEffect.World = originalWorld * Matrix.Translation(pos);
        }


        public Model createAsteroidModel()
        {
            Model model = game.Content.Load<Model>("asteroid");
            BasicEffect.EnableDefaultLighting(model, true);
            return model;
        }

        public void Hit()
        {
            // do nothing for now
        }
    }
}

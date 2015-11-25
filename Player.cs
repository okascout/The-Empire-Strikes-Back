using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // Player class.
    class Player : GameObject
    {
        //private float speed = 0.006f;
        //private float projectileSpeed = 20;
        private float projectileSpeed = 300;
        private float scaling;
        private const float size = 100;
        private Matrix originalWorld;

        private double angleY;
        private double angleX;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            model = game.assets.GetModel("player", CreatePlayerModel);
            collisionRadius = 10;

            //pos = new SharpDX.Vector3(0, game.boundaryBottom + 1f, 0);
            pos = new Vector3(0, -1, 0);
            //pos = new SharpDX.Vector3(0, 10, 0);

            BoundingSphere modelBounds = model.CalculateBounds();
            scaling = size / modelBounds.Radius;
            originalWorld = Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z) 
                            * Matrix.Scaling(scaling);
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.camera.View,
                Projection = game.camera.Projection,
                //World = Matrix.Identity
                World = originalWorld
            };

            
        }

        public Model CreatePlayerModel()
        {
            //return game.assets.CreateTexturedCube("player.png", 0.7f);
            Model model = game.Content.Load<Model>("cannon");
            BasicEffect.EnableDefaultLighting(model, true);
            return model;
        }

        // Method to create projectile texture to give to newly created projectiles.
        private Model CreatePlayerProjectileModel()
        {
            //return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
            Model model = game.Content.Load<Model>("Sphere");
            BasicEffect.EnableDefaultLighting(model, true);
            return model;
        }

        // Shoot a projectile.
        private void fire()
        {
            Vector3 target = new Vector3(game.camera.View.Column3.X, 
                            game.camera.View.Column3.Y, game.camera.View.Column3.Z);
            Vector3 x_axis = new Vector3(game.camera.View.Column1.X,
                            game.camera.View.Column1.Y, game.camera.View.Column1.Z);
            Vector3 start_pos = -Vector3.Normalize(Vector3.Cross(target, x_axis));
            Vector3 end_pos = 99 * target;
            Vector3 vel = projectileSpeed * Vector3.Normalize(end_pos - start_pos);

            game.Add(new Projectile(game,
                game.assets.GetModel("player projectile", CreatePlayerProjectileModel),
                start_pos,
                vel,
                GameObjectType.Enemy
            )); 
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (game.keyboardState.IsKeyDown(Keys.Space)) { fire(); }

            // TASK 1: Determine velocity based on accelerometer reading
            //pos.X += (float)game.accelerometerReading.AccelerationX;

            // Keep within the boundaries.
            //if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            //if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }

            angleX = game.camera.angleX;
            angleY = game.camera.angleY;



            Matrix viewModel = game.camera.View;
   
 

            //basicEffect.World = originalWorld * Matrix.Translation(pos) * Matrix.Scaling(scaling)
            //                   * Matrix.RotationX((float)angleX)*Matrix.RotationY((float)angleY);

            basicEffect.World = originalWorld * Matrix.Translation(pos);
             // * Matrix.RotationX((float)-angleX)*Matrix.RotationY((float)angleY);
        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();
        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
           fire();
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
           // pos.X += (float)args.Delta.Translation.X / 100;
        }
    }
}
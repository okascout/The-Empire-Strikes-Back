using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    // Enemy class
    // Basically just shoots randomly, see EnemyController for enemy movement.
    using SharpDX.Toolkit.Graphics;
    class Enemy : GameObject
    {
        private float projectileSpeed = 10;

        float fireTimer;
        float fireWaitMin = 2000;
        float fireWaitMax = 20000;

        private const float size = 10;
        private float scaling;
        private Matrix originalWorld;

        private String debug1;
        private int move = 0;
        public Enemy(LabGame game, Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Enemy;

            model = game.assets.GetModel("ship", CreateEnemyModel);
            collisionRadius = size/2;

            BoundingSphere modelBounds = model.CalculateBounds();
            scaling = size / modelBounds.Radius;
            originalWorld = Matrix.Translation(-modelBounds.Center.X, -modelBounds.Center.Y, -modelBounds.Center.Z) * Matrix.Scaling(scaling);

            this.pos = pos;
            //setFireTimer();
            //GetParamsFromModel();

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.camera.View,
                Projection = game.camera.Projection,
                //World = Matrix.Identity
                World = originalWorld

            };

        }

        private void setFireTimer()
        {
            fireTimer = fireWaitMin + (float)game.random.NextDouble() * (fireWaitMax - fireWaitMin);
        }

        public Model CreateEnemyModel()
        {
            //return game.assets.CreateTexturedCube("enemy.png", 0.5f);
            Model model = game.Content.Load<Model>("Xwing");
            BasicEffect.EnableDefaultLighting(model, true);
            return model;
        }

        private MyModel CreateEnemyProjectileModel()
        {
            return game.assets.CreateTexturedCube("enemy projectile.png", new Vector3(0.2f, 0.2f, 0.4f));
        }

        public override void Update(GameTime gameTime)
        {
            // TASK 3: Fire projectile
            /*
            fireTimer -= gameTime.ElapsedGameTime.Milliseconds * game.difficulty;
            if (fireTimer < 0)
            {
                fire();
                setFireTimer();
            }
            basicEffect.World = Matrix.Translation(pos);
            */
            fly();

            basicEffect.World = originalWorld * Matrix.Translation(pos);
        }

        private void fly()
        {
            if (pos.Z > 20)
            {
                pos.Z -= 0.02f;
            }

            if (pos.Y > 5)
            {
                move = -1;
            }
            if (pos.Y < -5)
            {
                move = 1;
            }


            foreach (var obj in game.gameObjects)
            {
                debug1 = pos.Z.ToString();

                if (obj.type == GameObjectType.Projectile)
                {
                    double xDist = pos.X - obj.pos.X;
                    double yDist = pos.Y - obj.pos.Y;
                    double zDist = pos.Z - obj.pos.Z;
                    double distanceToBullet = Math.Sqrt((xDist * xDist) + (yDist * yDist) + (zDist * zDist));

                    if (distanceToBullet < 20)
                    {

                        if (yDist > 0)
                        {
                            move = 1;
                        }
                        else
                        {
                            move = -1;
                        }


                    }


                }
            }



            if (move == 1)
            {
                pos.Y += 0.2f;

            }
            else if (move == -1)
            {
                pos.Y -= 0.2f;
            }


        }

        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("enemy projectile", CreateEnemyProjectileModel),
                pos,
                new Vector3(0, -projectileSpeed, 0),
                GameObjectType.Player
            ));
        }

        public void Hit()
        {
            game.score += 10;
            game.Remove(this);
        }

    }
}

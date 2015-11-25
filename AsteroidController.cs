using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class AsteroidController: GameObject
    {
        private const int AMOUNT = 1;
        public Random random;

        public AsteroidController(LabGame game)
        {
            this.game = game;
            random = new Random();
            createAsteroids();
        }

        public override void Update(GameTime gameTime)
        {
            int random_number = random.Next(0, 500);
            if (random_number == 1)
                createAsteroids();
        }

        private int randomSign(Random random)
        {
            int sign = 1;
            int temp = random.Next(0, 2);
            if (temp < 1)
                sign = -1;
            return sign;

        }
        private void createAsteroids()
        {

            Vector3 pos = new Vector3();
            Vector3 vel = new Vector3();
            int size = 0;

            for (int i = 0; i < AMOUNT; i++)
            {

                pos.X = randomSign(random)*random.Next(0, 30);
                pos.Y = randomSign(random) * random.Next(10, 20);
                pos.Z =  random.Next(30, 70);

                vel.X = random.Next(-100, 100);
                vel.Y = random.Next(-100, 100);
                vel.Z = random.Next(-100, 100);
                vel = 0.3f*Vector3.Normalize(vel);

                
                size = random.Next(1, 5);
                
                game.Add(new Asteroid(game,  pos, vel, size));
            }
        }
    }
}

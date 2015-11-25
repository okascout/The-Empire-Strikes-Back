using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.Devices.Sensors;

namespace Project
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public LabGame game;
        public Vector3 pos;
        public Vector3 target;
        public Vector3 up;
        public double angleY;
        public double angleX;
        public Vector3 oldPos;
        public static  float MaxModelSize = 10.0f;

        public static Vector3 originalPos = new Vector3(0, 0, 0);
        public static Vector3 originalTarget = new Vector3(0, 0, 10);
        public static Vector3 originalUp = Vector3.UnitY;

        private const float boundaryLeft = (float) -Math.PI/4;
        private const float boundaryRight = (float)Math.PI / 4;
        private const float boundaryUp = (float) Math.PI / 4;
        private const float boundaryDown = (float)-Math.PI / 4;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(LabGame game) {
            //pos = new Vector3(0, 0, -10);
            angleX = 0;
            angleY = 0;
            pos = originalPos;
            target = originalTarget;
            up = originalUp;
            View = Matrix.LookAtLH(pos, target, up);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            //Projection = Matrix.PerspectiveFovRH(0.9f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, MaxModelSize * 10.0f);
            this.game = game;
        }

        // If the screen is resized, the projection matrix will change
        public void Update()
        {


            angleX += toRadian(game.gyrometerReading.AngularVelocityX / 60);
            angleY += toRadian(game.gyrometerReading.AngularVelocityY/30);

            if (angleX < boundaryDown) { angleX = boundaryDown; }
            if (angleX > boundaryUp) { angleX = boundaryUp; }
            if (angleY < boundaryLeft) { angleY = boundaryLeft; }
            if (angleY > boundaryRight) { angleY = boundaryRight; }

            //angle += (float)game.accelerometerReading.AccelerationX/100;
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);

            
            pos = Vector3.Normalize(rotate(originalPos, angleX, angleY));
            target = Vector3.Normalize(rotate(originalTarget, angleX, angleY));
            up = Vector3.Normalize(rotate(originalUp, angleX, angleY));
            
            //View = Matrix.LookAtLH(pos, target, up);

            View = Matrix.LookAtLH(originalPos, originalTarget, originalUp) *Matrix.RotationX((float)angleX)* Matrix.RotationY((float)angleY);
        }

        private Vector3 rotate(Vector3 vector, double angleX, double angleY)
        {
            float x, y, z;
            x = vector.X;
            // rotate X
            y = (float) (vector.Y * Math.Cos(angleX) - vector.Z * Math.Sin(angleX));
            z = (float)(vector.Y * Math.Sin(angleX) - vector.Z * Math.Cos(angleX));

            // rorate Y
            x = (float)(x * Math.Cos(angleY) + z * Math.Sin(angleY));
            z = (float)(-x * Math.Sin(angleY) + z * Math.Cos(angleY));

            return new Vector3(x, y, -z);

        }

    

        private double toRadian(double degree)
        {
            return  degree * Math.PI / 180;
        }

    }
}

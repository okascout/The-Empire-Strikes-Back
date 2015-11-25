using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace Project
{
    class Crosshair : GameObject
    {
   
        public Crosshair(LabGame game)
        {
            this.game = game;
            type = GameObjectType.None; //change later
            int screen_width =  2160;
            int screen_height = 1440;

            
            texture = game.Content.Load<Texture2D>("crosshair.png");
            
            int texture_width = texture.Width;
            int texture_height = texture.Height;

            pos = new Vector3((screen_width - texture_width) / 2, (screen_height - texture_height) / 2, 0);

        }


        public override void Update(GameTime gameTime)
        {

        }
        public virtual void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {

        }

        public virtual void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {

        }

        public virtual void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {

        }

        public virtual void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {

        }
    }
}

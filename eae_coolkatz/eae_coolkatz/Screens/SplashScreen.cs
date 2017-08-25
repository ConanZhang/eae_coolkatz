using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using eae_coolkatz.Images;

namespace eae_coolkatz.Screens
{
    public class SplashScreen : GameScreen
    {
        public Image Image;

        public SplashScreen()
        {
            Image = new Image("SplashScreen/Image");
            Image.Effects = "FadeEffect";
            Image.IsActive = true;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}

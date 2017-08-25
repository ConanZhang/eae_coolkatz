using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using eae_coolkatz.Images;
using Microsoft.Xna.Framework.Input;

namespace eae_coolkatz.Screens
{
    public class SplashScreen : GameScreen
    {
        public Image Image;

        public SplashScreen()
        {
            Image = new Image("SplashScreen/Image");
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

            if(Keyboard.GetState().IsKeyDown(Keys.Enter) && !ScreenManager.Instance.IsTransistioning)
            {
                ScreenManager.Instance.ChangeScreens("SplashScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}

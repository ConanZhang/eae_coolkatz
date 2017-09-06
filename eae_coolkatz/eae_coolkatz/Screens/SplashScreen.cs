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

        private const float _delay = 2;
        private float _remainingDelay = _delay;

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

            float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if(_remainingDelay <= 0)
            {
                ScreenManager.Instance.ChangeScreens("TitleScreen");
                _remainingDelay = _delay;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}

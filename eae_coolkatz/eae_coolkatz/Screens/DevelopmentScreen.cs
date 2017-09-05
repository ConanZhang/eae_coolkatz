using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eae_coolkatz.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eae_coolkatz.Screens
{
    public class DevelopmentScreen : GameScreen
    {
        public Image Image = new Image();

        public override void LoadContent()
        {
            base.LoadContent();
            Image.Texture = content.Load<Texture2D>("GameplayScreen/AngelVictoryCard");
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

            if (InputManager.Instance.KeyPressed(Keys.Enter))
            {

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}

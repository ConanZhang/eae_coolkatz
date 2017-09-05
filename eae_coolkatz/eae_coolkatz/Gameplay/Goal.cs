using eae_coolkatz.Images;
using eae_coolkatz.Screens;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eae_coolkatz.Gameplay
{
    public class Goal
    {
        public Image Image;
        Body goalSensor;

        public Goal()
        {
        }

        public void ActivateImage()
        {
            Image.IsActive = true;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X / 2), ScreenManager.Instance.Dimensions.Y / 3);
            ActivateImage();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}

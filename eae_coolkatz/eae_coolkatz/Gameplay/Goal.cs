using eae_coolkatz.Images;
using eae_coolkatz.Screens;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Factories;

namespace eae_coolkatz.Gameplay
{
    public class Goal
    {
        public Image Image;
        Body goalSensor;
        private bool isAngel;

        public Goal(World world, Vector2 Position, bool isAngel)
        {
            this.isAngel = isAngel;
            Image = new Image();
            Image.Effects = "SpriteSheetEffect";
            if (isAngel)
            {
                Image.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/AngelVictoryCard");
            }
            else
            {
                Image.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/DemonVictoryCard");
            }


            goalSensor = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(1080), 10f);
            goalSensor.Position = ConvertUnits.ToSimUnits(Position);
            goalSensor.IsStatic = true;
            goalSensor.IsSensor = true;
        }

        public void ActivateImage()
        {
            Image.IsActive = true;
        }

        public void LoadContent()
        {
            Image.LoadContent();
            Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X / 2), ScreenManager.Instance.Dimensions.Y / 3);
            //ActivateImage();
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
            if (Image.IsActive)
            {
                Image.Draw(spriteBatch, new Vector2( Image.SpriteSheetEffect.FrameWidth / 2, Image.SpriteSheetEffect.FrameHeight /2));
            }
        }
    }
}

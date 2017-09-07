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
using FarseerPhysics.Dynamics.Contacts;

namespace eae_coolkatz.Gameplay
{
    public class Goal
    {
        Image victoryCard;
        Image victoryGate;
        Body goalSensor;
        private bool isAngel;
        public bool activated;

        public Goal(World world, Vector2 Position, bool isAngel)
        {
            activated = false;
            this.isAngel = isAngel;
            victoryGate = new Image();
            victoryGate.Effects = "SpriteSheetEffect";
            victoryCard = new Image();
            victoryCard.Position = Position;
            victoryCard.Effects = "SpriteSheetEffect";
            if (isAngel)
            {
                victoryGate.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/AngelVictoryGate");
                victoryCard.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/DemonVictoryCard");
                victoryGate.Position= new Vector2(8150,625);
            }
            else
            {
                victoryGate.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/DemonVictoryGate");
                victoryCard.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/AngelVictoryCard");
                victoryGate.Position= new Vector2(-5000,550);
            }


            goalSensor = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(175), ConvertUnits.ToSimUnits(250), 10f);
            goalSensor.Position = ConvertUnits.ToSimUnits(Position);
            goalSensor.IsStatic = true;
            goalSensor.IsSensor = true;
            goalSensor.OnCollision += OnCollision;
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(isAngel && (string)fixtureB.Body.UserData == "demon_sacrifice")
            {
                ActivateImage();
                return true;
            }
            else if(!isAngel && (string)fixtureB.Body.UserData == "angel_sacrifice")
            {
                ActivateImage();
                return true;
            }

            return false;
        }

        public void ActivateImage()
        {
            victoryCard.IsActive = true;
            victoryGate.IsActive = true;
            activated = true;
        }

        public void LoadContent()
        {
            victoryGate.LoadContent();
            victoryGate.SpriteSheetEffect.AmountOfFrames = new Vector2(4, 3);
            victoryGate.SpriteSheetEffect.scrollThrough = true;
            victoryCard.LoadContent();
            victoryCard.Position = new Vector2(victoryCard.Position.X, ScreenManager.Instance.Dimensions.Y / 3);
        }

        public void UnloadContent()
        {
            victoryGate.UnloadContent();
            victoryCard.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            victoryGate.Update(gameTime);
            victoryCard.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (victoryCard.IsActive)
            {
                victoryCard.Draw(spriteBatch, new Vector2( victoryCard.SpriteSheetEffect.FrameWidth / 2, victoryCard.SpriteSheetEffect.FrameHeight /2));
            }

            victoryGate.Draw(spriteBatch);
            victoryGate.Draw(spriteBatch);
        }
    }
}

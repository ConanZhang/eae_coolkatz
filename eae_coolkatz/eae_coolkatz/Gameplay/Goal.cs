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

        public Goal(World world, Vector2 Position, bool isAngel)
        {
            this.isAngel = isAngel;
            victoryGate = new Image();
            victoryCard = new Image();
            victoryCard.Position = Position;
            victoryCard.Effects = "SpriteSheetEffect";
            if (isAngel)
            {
                victoryGate.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/AngelVictoryGate");
                victoryGate.flipHorizontally = true;
                victoryCard.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/DemonVictoryCard");
            }
            else
            {
                victoryGate.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/DemonVictoryGate");
                victoryGate.Effects = "SpriteSheetEffect";
                victoryCard.Texture = ScreenManager.Instance.Content.Load<Texture2D>("GameplayScreen/AngelVictoryCard");
            }


            goalSensor = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(175), ConvertUnits.ToSimUnits(250), 10f);
            goalSensor.Position = ConvertUnits.ToSimUnits(Position);
            goalSensor.IsStatic = true;
            goalSensor.IsSensor = true;
            goalSensor.OnCollision += OnCollision;
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if(isAngel && (string)fixtureB.Body.UserData == "demon")
            {
                ActivateImage();
                return true;
            }
            else if(!isAngel && (string)fixtureB.Body.UserData == "angel")
            {
                ActivateImage();
                return true;
            }

            return false;
        }

        public void ActivateImage()
        {
            victoryCard.IsActive = true;
        }

        public void LoadContent()
        {
            victoryGate.LoadContent();
            if(!isAngel)
            {
                victoryGate.SpriteSheetEffect.AmountOfFrames = new Vector2(7, 1);
                victoryGate.IsActive = true;
            }
            victoryCard.LoadContent();
            victoryCard.Position = new Vector2(victoryCard.Position.X, ScreenManager.Instance.Dimensions.Y / 3);
            //ActivateImage();
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

            if(isAngel)
            {
                victoryGate.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(goalSensor.Position), goalSensor.Rotation, true, 0f);
            }
            else
            {
                victoryGate.Draw(spriteBatch, ConvertUnits.ToDisplayUnits(goalSensor.Position.X + 9, goalSensor.Position.Y), goalSensor.Rotation, true, 0f);
            }

        }
    }
}

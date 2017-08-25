using eae_coolkatz.Images;
using eae_coolkatz.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace eae_coolkatz.Screens
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        XmlManager<GameScreen> xmlGameScreenManager;
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        [XmlIgnore]
        public Vector2 Dimensions { private set; get; }

        GameScreen currentScreen, newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransistioning { get; private set; }

        public static ScreenManager Instance
        {
            get
            {
                if(instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Load/ScreenManager.xml");
                }

                return instance;
            }
        }

        public ScreenManager()
        {
            Dimensions = new Vector2(1920, 1080);
            currentScreen = new GameScreen();
            //currentScreen = new SplashScreen();
            xmlGameScreenManager = new XmlManager<GameScreen>();
            xmlGameScreenManager.Type = currentScreen.Type;
            //currentScreen = xmlGameScreenManager.Load("Load/SplashScreen.xml");
        }

        public void ChangeScreens(string screenName)
        {
            newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("eae_coolkatz.Screens." + screenName));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransistioning = true;
        }

        void Transition(GameTime gameTime)
        {
            if(IsTransistioning)
            {
                Image.Update(gameTime);
                if(Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    xmlGameScreenManager.Type = currentScreen.Type;
                    if(File.Exists(currentScreen.XmlPath))
                    {
                        currentScreen = xmlGameScreenManager.Load(currentScreen.XmlPath);
                    }
                    currentScreen.LoadContent();
                }
                else if(Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransistioning = false;
                }
            }

        }

        public void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }


        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if(IsTransistioning)
            {
                Image.Draw(spriteBatch);
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eae_coolkatz
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;

        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public Texture2D Texture;
        Vector2 _origin;
        ContentManager content;

        public Image()
        {
            Path = Text = string.Empty;
            FontName = "Image";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;

        }

        public void LoadContent()
        {
//            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            if(Path != string.Empty)
            {
                Texture = content.Load<Texture2D>(Path);
            }
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

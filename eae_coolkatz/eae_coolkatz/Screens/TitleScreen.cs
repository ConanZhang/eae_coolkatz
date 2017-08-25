using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using eae_coolkatz.Menu;

namespace eae_coolkatz.Screens
{
    public class TitleScreen : GameScreen
    {
        MenuManager menuManager;

        public TitleScreen()
        {
            menuManager = new MenuManager();
        }
        public override void LoadContent()
        {
            base.LoadContent();
            menuManager.LoadContent("Load/Menus/TitleMenu.xml");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }
    }
}

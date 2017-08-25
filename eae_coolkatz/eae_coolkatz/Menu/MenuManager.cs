using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eae_coolkatz.Menu
{
    public class MenuManager
    {
        Menu menu;
        public MenuManager()
        {
            menu = new Menu();
            menu.OnMenuChange += Menu_OnMenuChange;
        }

        private void Menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            menu.UnloadContent();
            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
        }

        public void LoadContent(string menuPath)
        {
            if(menuPath != string.Empty)
            {
                menu.ID = menuPath;
            }
        }

        public void UnloadContent()
        {
            menu.UnloadContent();

        }

        public void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}

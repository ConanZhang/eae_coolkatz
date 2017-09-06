using eae_coolkatz.Images;
using eae_coolkatz.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace eae_coolkatz.Menu
{
    public class Menu
    {
        public event EventHandler OnMenuChange;
        public string Axis;
        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;
        int itemNumber;
        string id;

        public Image Image;

        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        public int ItemNumber
        {
            get { return itemNumber; }
        }
        public Menu()
        {
            id = string.Empty;
            itemNumber = 0;
            Effects = string.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void Transition(float alpha)
        {
            foreach(MenuItem item in Items)
            {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                if(alpha == 0.0f)
                {
                    item.Image.FadeEffect.Increase = true;
                }
                else
                {
                    item.Image.FadeEffect.Increase = false;
                }
            }
        }
        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            foreach(MenuItem item in Items)
            {
                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }

            dimensions = new Vector2((ScreenManager.Instance.Dimensions.X - dimensions.X) / 2, ((ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2) + 350) ;

            foreach(MenuItem item in Items)
            {
                if(Axis == "X")
                {
                    item.Image.Position = new Vector2(dimensions.X, (ScreenManager.Instance.Dimensions.Y - item.Image.SourceRect.Height) / 2);
                }
                else if(Axis == "Y")
                {
                    item.Image.Position = new Vector2((ScreenManager.Instance.Dimensions.X - item.Image.SourceRect.Width) / 2, dimensions.Y);
                }

                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }
        }

        public void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach(MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach(string s in split)
                {
                    item.Image.ActivateEffect(s);
                }
            }
            AlignMenuItems();

            if(Image != null)
            {
                Image.LoadContent();
            }
        }

        public void UnloadContent()
        {
            foreach(MenuItem item in Items)
            {
                item.Image.UnloadContent();
            }

            if(Image != null)
            {
                Image.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if(Axis == "X")
            {
                if(InputManager.Instance.KeyPressed(Keys.Right))
                {
                    itemNumber++;
                }
                else if(InputManager.Instance.KeyPressed(Keys.Left))
                {
                    itemNumber--;
                }
            }
            else if(Axis == "Y")
            {
                if(InputManager.Instance.KeyPressed(Keys.Down))
                {
                    itemNumber++;
                }
                else if(InputManager.Instance.KeyPressed(Keys.Up))
                {
                    itemNumber--;
                }
            }

            if(itemNumber < 0)
            {
                itemNumber = 0;
            }
            else if(itemNumber > Items.Count - 1)
            {
                itemNumber = Items.Count - 1;
            }

            for(int i = 0; i < Items.Count; i++)
            {
                if(i == itemNumber)
                {
                    Items[i].Image.IsActive = true;
                }
                else
                {
                    Items[i].Image.IsActive = false;
                }

                Items[i].Image.Update(gameTime);
            }

            if(Image != null)
            {
                Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Image != null)
            {
                Image.Draw(spriteBatch);
            }
            foreach(MenuItem item in Items)
            {
                item.Image.Draw(spriteBatch);
            }
        }
    }
}


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eae_coolkatz.Screens;

namespace eae_coolkatz.Screens
{
    class AudioManager
    {
        private static AudioManager audioManager = null;
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public void LoadContent()
        {
            //ScreenManager.Instance.LoadContent(ScreenManager.Instance.Content);
            SoundEffect angelVictoryTheme = ScreenManager.Instance.Content.Load<SoundEffect>("Audio/angel_victory_theme");
            sounds.Add("angel_victory", angelVictoryTheme);

            SoundEffect devilVictoryTheme = ScreenManager.Instance.Content.Load<SoundEffect>("Audio/devil_victory_theme");
            sounds.Add("devil_victory", devilVictoryTheme);

            SoundEffect explosion = ScreenManager.Instance.Content.Load<SoundEffect>("Audio/explosion");
            sounds.Add("explosion", explosion);

            SoundEffect sacrificePickup = ScreenManager.Instance.Content.Load<SoundEffect>("Audio/sacrifice_pickup");
            sounds.Add("sacrifice_pickup", sacrificePickup);

            SoundEffect crash1 = ScreenManager.Instance.Content.Load<SoundEffect>("Audio/crash_1");
            sounds.Add("crash1", crash1);
        }

        public void PlaySound(string soundName)
        {

            if (sounds.ContainsKey(soundName))
            {
                try
                {
                    sounds[soundName].Play();
                }
                catch (InstancePlayLimitException)
                {
                    //if this is thrown, too many sound effects are currently playing
                }
            }
        }

        public void UnloadContent()
        {
            //pass
        }

    }
}
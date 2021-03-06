﻿using eae_coolkatz.Screens;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eae_coolkatz
{
    public class InputManager
    {
        KeyboardState currentKeyState, prevKeyState;
        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }


        public void Update()
        {
            prevKeyState = currentKeyState;
            if (!ScreenManager.Instance.IsTransistioning)
            {
                currentKeyState = Keyboard.GetState();
            }
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if(currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if(currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool KeyPressed(object keys)
        {
            throw new NotImplementedException();
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if(currentKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

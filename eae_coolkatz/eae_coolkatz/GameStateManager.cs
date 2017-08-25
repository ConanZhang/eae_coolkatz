using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eae_coolkatz
{
    public enum GameStates
    {
        Menu,
        Playing,
        Paused,
        End
    }

    class GameStateManager
    {

        GameStates _gameState;

        public GameStateManager()
        {
            _gameState = GameStates.Menu;
        }

        public GameStates GameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                _gameState = value;
            }
        }
    }
}

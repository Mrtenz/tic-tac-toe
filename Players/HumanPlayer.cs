using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Players
{
    public class HumanPlayer : Player
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public PlayerType Type { get; } = PlayerType.Human;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyPropertyChanged("Score");
            }
        }

        private int _score = 0;
        private Game _game;

        public HumanPlayer(Game game)
        {
            _game = game;
        }

        public void GiveTurn()
        {
            // Not relevant for the player
        }

        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Players
{
    public class ComputerPlayer : Player
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public PlayerType Type { get; } = PlayerType.Computer;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyPropertyChanged("Score");
            }
        }

        private Game _game;
        private int _score = 0;

        public ComputerPlayer(Game game)
        {
            _game = game;
        }

        public void GiveTurn()
        {
            List<FieldItem> items = _game.GetFreeItems();
            FieldItem item = items.OrderBy(i => Guid.NewGuid()).FirstOrDefault();
            item?.Select(this);
        }

        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

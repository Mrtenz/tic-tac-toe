using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TicTacToe.Players;

namespace TicTacToe
{
    public class FieldItem
    {
        public Game Field { get; private set; }
        public Point Point { get; private set; }
        public bool Selected { get; private set; }
        public Player Selector { get; private set; }

        private readonly Button _button;

        public FieldItem(Game field, Point point, Button button)
        {
            this.Field = field;
            this.Point = point;
            this._button = button;

            button.Click += Button_Click;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!Selected && Field.Turn.Type == PlayerType.Human)
            {
                Select(Field.Human);
            }
        }

        public void Select(Player player, bool nextTurn = true)
        {
            Selected = true;
            Selector = player;

            if (player.Type == PlayerType.Human)
            {
                _button.Content = "✕";
            }
            else
            {
                _button.Content = "O";
            }

            if (nextTurn)
            {
                Field.NextTurn();
            }
        }

        public void Reset()
        {
            _button.Content = "";
            Selected = false;
            Selector = null;
        }
    }
}

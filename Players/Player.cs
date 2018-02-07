using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Players
{
    public interface Player : INotifyPropertyChanged
    {
        int Score { get; set; }

        void GiveTurn();

        PlayerType Type { get; }
    }
}

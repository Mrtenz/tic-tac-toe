using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        public Game Game { get; private set; }

        public MainWindow()
        {
            Game = new Game(this);

            DataContext = this;
            InitializeComponent();
            InitializeGame(Game);

            ResetButton.Click += ResetButton_Click;
            SaveButton.Click += SaveButton_Click;
            LoadButton.Click += LoadButton_Click;
            ExitButton.Click += ExitButton_Click;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Reset();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Load();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Game.Save();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InitializeGame(Game field)
        {
            int x = 0;
            int y = 0;
            foreach (Button button in buttonsGrid.Children.OfType<Button>())
            {
                if (x > 2)
                {
                    x = 0;
                    y++;
                }

                Point point = new Point(x, y);
                FieldItem item = new FieldItem(field, point, button);
                field.AddField(item);

                x++;
            }
        }
    }
}

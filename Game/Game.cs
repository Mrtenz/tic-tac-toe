using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicTacToe.Players;

namespace TicTacToe
{
    public class Game
    {
        public readonly Dictionary<Point, FieldItem> Field = new Dictionary<Point, FieldItem>();
        public Player Turn { get; set; }
        public DatabaseConnection DatabaseConnection { get; private set; }
        public Player Human { get; private set; }
        public Player Computer { get; private set; }

        private readonly MainWindow _window;

        private List<List<Point>> winConditions = new List<List<Point>>()
        {
            new List<Point>() {new Point(0, 0), new Point(1, 0), new Point(2, 0)},
            new List<Point>() {new Point(0, 1), new Point(1, 1), new Point(2, 1)},
            new List<Point>() {new Point(0, 2), new Point(1, 2), new Point(2, 2)},
            new List<Point>() {new Point(0, 0), new Point(0, 1), new Point(0, 2)},
            new List<Point>() {new Point(1, 0), new Point(1, 1), new Point(1, 2)},
            new List<Point>() {new Point(2, 0), new Point(2, 1), new Point(2, 2)},
            new List<Point>() {new Point(0, 0), new Point(1, 1), new Point(2, 2)},
            new List<Point>() {new Point(2, 0), new Point(1, 1), new Point(0, 2)}
        };

        public Game(MainWindow window)
        {
            _window = window;

            DatabaseConnection = new DatabaseConnection("./Resources/Database.accdb");
            Human = new HumanPlayer(this);
            Computer = new ComputerPlayer(this);
            Turn = Human;
        }

        public FieldItem GetItem(int x, int y)
        {
            if (x < 0 || x > 2 || y < 0 || y > 2)
            {
                throw new ArgumentException();
            }

            Point point = new Point(x, y);
            return GetItem(point);
        }

        public FieldItem GetItem(Point point)
        {
            return Field[point];
        }

        public void Select(Player player, FieldItem item)
        {
            item.Select(player);
        }

        public void NextTurn()
        {
            if (CheckWin())
            {
                Win(Turn);
                return;
            }

            if (Turn.Equals(Human))
            {
                Turn = Computer;
            }
            else
            {
                Turn = Human;
            }

            if (GetFreeItems().Count > 0)
            {
                Turn.GiveTurn();
            }
            else
            {
                End();
            }
        }

        public void Win(Player player)
        {
            if (player.Type == PlayerType.Human)
            {
                MessageBox.Show("Congratulations!");
            }
            else
            {
                MessageBox.Show("You lost!");
            }

            player.Score++;
            Reset();
        }

        public void End()
        {
            MessageBox.Show("There is no winner!");
            Reset();
        }

        public void Reset()
        {
            Turn = Human;
            Field.Values.ToList().ForEach(item => item.Reset());
        }

        public void Save()
        {
            DatabaseConnection.ExecuteUpdate("DELETE * FROM Field");
            DatabaseConnection.ExecuteUpdate("DELETE * FROM Players");

            DatabaseConnection.ExecuteUpdate("INSERT INTO Players (PlayerType, Score) VALUES (?, ?)",
                Human.Type.ToString(), Human.Score);
            DatabaseConnection.ExecuteUpdate("INSERT INTO Players (PlayerType, Score) VALUES (?, ?)",
                Computer.Type.ToString(), Computer.Score);

            foreach (FieldItem item in Field.Values)
            {
                DatabaseConnection.ExecuteUpdate("INSERT INTO Field (X, Y, PlayerType) VALUES (?, ?, ?)", item.Point.X,
                    item.Point.Y,
                    item.Selected ? item.Selector.Type.ToString() : "");
            }
        }

        public void Load()
        {
            DataTable table = DatabaseConnection.ExecuteQuery("SELECT * FROM Players");
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("There is nothing to load.");
                return;
            }

            foreach (DataRow row in table.Rows)
            {
                string playerType = (string) row["PlayerType"];
                int score = Convert.ToInt32(row["Score"]);

                if (playerType == "Human")
                {
                    Human.Score = score;
                }
                else if (playerType == "Computer")
                {
                    Computer.Score = score;
                }
            }

            DataTable fieldTable = DatabaseConnection.ExecuteQuery("SELECT * FROM Field");
            foreach (DataRow row in fieldTable.Rows)
            {
                int x = Convert.ToInt32(row["X"]);
                int y = Convert.ToInt32(row["Y"]);
                string playerType = (string) row["PlayerType"];

                FieldItem item = GetItem(new Point(x, y));
                if (!String.IsNullOrEmpty(playerType))
                {
                    if (playerType == "Human")
                    {
                        item.Select(Human, false);
                    }
                    else if (playerType == "Computer")
                    {
                        item.Select(Computer, false);
                    }
                }
                else
                {
                    item.Reset();
                }
            }
        }

        public List<FieldItem> GetFreeItems()
        {
            return Field.Values.Where(item => !item.Selected).ToList();
        }

        public void AddField(FieldItem item)
        {
            if (Field.Count >= 9)
            {
                throw new ArgumentException();
            }

            Field.Add(item.Point, item);
        }

        private bool CheckWin()
        {
            foreach (List<Point> points in winConditions)
            {
                if (AllSelected(points))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AllSelected(List<Point> points)
        {
            Player player = null;
            foreach (Point point in points)
            {
                FieldItem item = GetItem(point);
                if (!item.Selected)
                {
                    return false;
                }

                if (player == null)
                {
                    player = item.Selector;
                }
                else if (!player.Equals(item.Selector))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

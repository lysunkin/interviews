using System.Text;

namespace Life.Board
{
    public class LifeBoard
    {
        bool[,] _board;

        public LifeBoard(int x, int y)
        {
            var rand = new Random();

            _board = new bool[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var r = rand.Next();
                    _board[i, j] = r % 2 == 0;
                }
            }
        }

        public void SetBoard(bool[,] board)
        {
            _board = board;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i <= _board.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= _board.GetUpperBound(1); j++)
                {
                    var cell = _board[i, j] ? "X" : ".";
                    sb.Append(cell);
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public void Tick()
        {
            var newBoard = new bool[_board.GetUpperBound(0) + 1, _board.GetUpperBound(1) + 1];

            for (int i = 0; i <= _board.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= _board.GetUpperBound(1); j++)
                {
                    var n = countNeighbors(i, j);

                    if (_board[i, j])
                    {
                        if (n == 2 || n == 3)
                        {
                            newBoard[i, j] = true;
                        }
                    }
                    else
                    {
                        if (n == 3)
                        {
                            newBoard[i, j] = true;
                        }
                    }
                }
            }

            _board = newBoard;
        }

        private int countNeighbors(int x, int y)
        {
            int result = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i <= _board.GetUpperBound(0)
                    && j >= 0 && j <= _board.GetUpperBound(1)
                    && !(i == x && j == y)
                    && _board[i, j])
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}

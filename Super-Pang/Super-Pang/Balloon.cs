namespace SuperPang
{
    using System;

    public class Balloon
    {
        private char[,] balloon;
        private char emptySymbols = ' ';
        private char balloonSymbols = '*';

        public Balloon(int radius, int x, int y, bool isFalling, bool isGoingRight)
        {
            this.Radius = radius;
            this.IsFalling = isFalling;
            this.IsGoingRight = isGoingRight;
            this.CurrentX = x;
            this.CurrentY = y;
            this.balloon = new char[this.Radius * 2, this.Radius * 2];
            DrawBallInArray();
        }

        public int Radius { get; set; }

        public int CurrentX { get; set; }

        public int CurrentY { get; set; }

        public bool IsFalling { get; set; }

        public bool IsGoingRight { get; set; }

        private void DrawBallInArray()
        {
            for (int row = 0; row < balloon.GetLength(0); row++)
            {
                for (int col = 0; col < balloon.GetLength(1); col++)
                {
                    if (balloon.GetLength(0) > 2)
                    {
                        if ((row == 0 || row == balloon.GetLength(0) - 1) &&
                            (col == 0 || col == balloon.GetLength(0) - 1))
                        {
                            balloon[row, col] = emptySymbols;
                        }
                        else
                        {
                            balloon[row, col] = balloonSymbols;
                        }
                    }
                    else
                    {
                        balloon[row, col] = balloonSymbols;
                    }
                }
            }
        }

        public void Draw(char[,] playGround)
        {
            for (int i = this.CurrentY; i < this.CurrentY + this.balloon.GetLength(0); i++)
            {
                for (int j = this.CurrentX; j < this.CurrentX + this.balloon.GetLength(1); j++)
                {
                    playGround[i, j] = this.balloon[i - this.CurrentY, j - this.CurrentX];
                }
            }
        }
    }
}

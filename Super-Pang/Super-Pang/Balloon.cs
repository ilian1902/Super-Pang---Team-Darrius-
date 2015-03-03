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
            // TODO: Implement logic

            for (int row = 0; row < balloon.GetLength(0); row++)
            {
                for (int col = 0; col < balloon.GetLength(1); col++)
                {
                    if (balloon.GetLength(0) > 2)
                    {
                        if ((row == 0 || row == balloon.GetLength(0) - 1) &&
                            (col == 0 || col == balloon.GetLength(0) - 1))
                        {
                            Console.Write(balloon[row, col] = emptySymbols);
                        }
                        else
                        {
                            Console.Write(balloon[row, col] = balloonSymbols);
                        }
                    }
                    else
                    {
                        Console.Write(balloon[row, col] = balloonSymbols);
                    }
                }
                Console.WriteLine();
            }

            //this.balloon[0, 0] = ' ';
            //this.balloon[0, 1] = '*';
            //this.balloon[0, 2] = '*';
            //this.balloon[0, 3] = ' ';
            //this.balloon[1, 0] = '*';
            //this.balloon[1, 1] = ' ';
            //this.balloon[1, 2] = ' ';
            //this.balloon[1, 3] = '*';
            //this.balloon[2, 0] = '*';
            //this.balloon[2, 1] = ' ';
            //this.balloon[2, 2] = ' ';
            //this.balloon[2, 3] = '*';
            //this.balloon[3, 0] = ' ';
            //this.balloon[3, 1] = '*';
            //this.balloon[3, 2] = '*';
            //this.balloon[3, 3] = ' ';
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

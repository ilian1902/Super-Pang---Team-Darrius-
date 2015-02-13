namespace SuperPang
{
    using System;

    public class Balloon
    {
        private char[,] balloon;

        public Balloon(int radius)
        {
            this.Radius = radius;
            this.balloon = new char[this.Radius * 2, this.Radius * 2];
            DrawBallInArray();
        }

        public int Radius { get; set; }

        private void DrawBallInArray()
        {
            this.balloon[0, 1] = '*';
            this.balloon[0, 2] = '*';
            this.balloon[1, 0] = '*';
            this.balloon[1, 3] = '*';
            this.balloon[2, 0] = '*';
            this.balloon[2, 3] = '*';
            this.balloon[3, 1] = '*';
            this.balloon[3, 2] = '*';
        }

        public void Draw(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            for (int i = 0; i < this.balloon.GetLength(0); i++)
            {
                for (int j = 0; j < this.balloon.GetLength(1); j++)
                {
                    Console.Write(this.balloon[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}

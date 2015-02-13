namespace SuperPang
{
    using System;

    public class Balloon
    {
        private char[,] balloon;

        public Balloon(int radius)
        {
            this.Radius = radius;
            this.IsFalling = true;
            this.IsGoingRight = true;
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
            this.balloon[0, 1] = '*';
            this.balloon[0, 2] = '*';
            this.balloon[1, 0] = '*';
            this.balloon[1, 3] = '*';
            this.balloon[2, 0] = '*';
            this.balloon[2, 3] = '*';
            this.balloon[3, 1] = '*';
            this.balloon[3, 2] = '*';
        }

        public void Draw()
        {
            Console.SetCursorPosition(this.CurrentX, this.CurrentY);
            for (int i = 0; i < this.balloon.GetLength(0); i++)
            {
                for (int j = 0; j < this.balloon.GetLength(1); j++)
                {
                    Console.Write(this.balloon[i, j]);
                }
                
                if (i != (this.Radius * 2) - 1)
                {
                    Console.WriteLine();
                    Console.SetCursorPosition(this.CurrentX, this.CurrentY + i + 1);
                }
            }
        }
    }
}

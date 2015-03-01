using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SuperPang
{
    class SuperPang
    {
        public const int InitialLives = 10;

        public static string playerHead = "(..)";
        public static string playerTorso = "<][>";
        public static string playerLegs = " /\\";
        public static string playerLegsTogether = " ||";
        public static ConsoleColor playerColor = ConsoleColor.Green;
        public static ConsoleColor sceneColor = ConsoleColor.Green;

        public static Thread timing;
        public static Thread music;

        public const int FirstBallonRadius = 2;
        public static List<Balloon> balloons = new List<Balloon>();

        public static int playerPositionX;
        public static int playerPositionY;
        public static int lives = 10;
        public static int playerScore = 0;
        public static int timeLeft = 100;
        public static int bonusPositionY = 1;
        //static string shotSymbol = "^\n|";

        // Generate random numbers for bonus.
        public static Random bonusRandom = new Random();

        public static int bonusPositionX;
        public static int bonusCurrentPositionX;
        public static int bonusCurrentPositionY;
        public static char bonusChar;
        public static ConsoleColor bonusColor;

        public static void Main()
        {
            Console.Title = "Super Pang";
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 60;
            Menu();

            timing = new Thread(new ThreadStart(Timer.Start));
            timing.Start();

            music = new Thread(new ThreadStart(Music.PlaySound));
            music.Start();

            playerPositionX = Console.BufferWidth / 2 - 3;
            playerPositionY = Console.BufferHeight - 3;

            var firstBalloon = new Balloon(FirstBallonRadius);
            balloons.Add(firstBalloon);

            Task.Run(() =>
            {
                while (true)
                {
                    Console.Clear();
                    DetectCollisionsBalloons();
                    DetectCollisionsBonus();
                    MoveAllBalloons();
                    Draw();
                    //SetBonus();
                    DrawBonus();
                    Thread.Sleep(250);
                }
            });

            while (lives > 0)
            {
                if (Console.KeyAvailable)
                {
                    Console.Clear();
                    MovePlayer();
                    DetectCollisionsBalloons();
                    DetectCollisionsBonus();
                    Draw();
                }
            }
        }

        static void Menu()
        {
            Console.SetCursorPosition(2, 2);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ***  *   * ****  ***** ****    ****    *   *   *  *** ");
            Console.SetCursorPosition(2, 3);
            Console.WriteLine("*   * *   * *   * *     *   *   *   *   *   *   * *   *");
            Console.SetCursorPosition(2, 4);
            Console.WriteLine("*   * *   * *   * *     *   *   *   *  * *  *   * *   *");
            Console.SetCursorPosition(2, 5);
            Console.WriteLine("*     *   * *   * *     *   *   *   *  * *  * * * *    ");
            Console.SetCursorPosition(2, 6);
            Console.WriteLine("*     *   * *   * *     *   *   *   * *   * *  ** *    ");
            Console.SetCursorPosition(2, 7);
            Console.WriteLine(" ***  *   * ****  ****  ****    ****  ***** *   * *  **");
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("    * *   * *     *     * *     *     *   * *   * *   *");
            Console.SetCursorPosition(2, 9);
            Console.WriteLine("    * *   * *     *     *  *    *     *   * *   * *   *");
            Console.SetCursorPosition(2, 10);
            Console.WriteLine("*   * *   * *     *     *   *   *     *   * *   * *   *");
            Console.SetCursorPosition(2, 11);
            Console.WriteLine("*   * *   * *     *     *   *   *     *   * *   * *   *");
            Console.SetCursorPosition(2, 12);
            Console.WriteLine(" ***   ***  *     ***** *   *   *     *   * *   *  ***");
            Console.ResetColor();
            Console.SetCursorPosition(18, 14);
            Console.WriteLine("For start enter - start ");
            Console.SetCursorPosition(19, 16);
            Console.WriteLine("For exit enter - exit ");
            Console.SetCursorPosition(29, 18);
            Console.ForegroundColor = ConsoleColor.Green;
            string command = Console.ReadLine();

            while (true)
            {
                if (command == "start")
                {
                    Console.Clear();
                    break;
                }
                else if (command == "exit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.SetCursorPosition(20, 18);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Command!");
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(20, 18);
                    Console.WriteLine("                                ");
                    Console.SetCursorPosition(29, 18);
                    Console.ForegroundColor = ConsoleColor.Green;
                    command = Console.ReadLine();
                }
            }
        }

        private static void MoveAllBalloons()
        {
            foreach (var balloon in balloons)
            {
                if (balloon.IsFalling)
                {
                    if (balloon.CurrentY + (balloon.Radius * 2) + 1 < Console.BufferHeight) balloon.CurrentY++;
                    else
                    {
                        balloon.CurrentY--;
                        balloon.IsFalling = false;
                    }
                }
                else
                {
                    if (balloon.CurrentY > 1) balloon.CurrentY--;
                    else
                    {
                        balloon.CurrentY++;
                        balloon.IsFalling = true;
                    }
                }

                if (balloon.IsGoingRight)
                {
                    if (balloon.CurrentX + (balloon.Radius * 2) + 1 < Console.BufferWidth) balloon.CurrentX++;
                    else
                    {
                        balloon.CurrentX--;
                        balloon.IsGoingRight = false;
                    }
                }
                else
                {
                    if (balloon.CurrentX > 0) balloon.CurrentX--;
                    else
                    {
                        balloon.CurrentX++;
                        balloon.IsGoingRight = true;
                    }
                }
            }
        }

        private static void MovePlayer()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (playerPositionX - 1 > 0)
                    {
                        playerPositionX--;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (playerPositionX + 1 < Console.WindowWidth - 4)
                    {
                        playerPositionX++;
                    }
                }
            }
        }

        private static void Draw()
        {
            DrawPlayer();
            DrawBalloons();
            DrawGameInfo();
        }

        private static void DrawPlayer()
        {
            Console.ForegroundColor = playerColor;
            Console.SetCursorPosition(playerPositionX, playerPositionY);
            Console.WriteLine(playerHead);

            Console.SetCursorPosition(playerPositionX, playerPositionY + 1);
            Console.WriteLine(playerTorso);

            Console.SetCursorPosition(playerPositionX, playerPositionY + 2);
            if (playerPositionX % 2 == 0) Console.WriteLine(playerLegs);
            else Console.WriteLine(playerLegsTogether);

            Console.ForegroundColor = sceneColor;
        }

        private static void DrawBalloons()
        {
            foreach (var balloon in balloons)
            {
                balloon.Draw();
            }
        }

        private static void DrawGameInfo()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 5, 0);
            Console.Write("Lives: {0}", lives);

            Console.SetCursorPosition((Console.WindowWidth - Console.WindowWidth) + 1, 0);
            Console.Write("Score: {0}", playerScore);

            Console.SetCursorPosition(Console.WindowWidth - 10, 0);
            Console.Write("Time: {0}", Timer.GetRemainingTime());

            if (Timer.GetRemainingTime() <= 0)
            {
                if (lives <= 0) EndGame();
                else
                {
                    Thread.Sleep(500);
                    lives--;
                    RestartGame();
                }
            }
        }

        static void EndGame()
        {
            Console.Clear();
            Console.SetCursorPosition(4, 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ***    *   *     * *****    ***  *   * ***** **** ");
            Console.SetCursorPosition(4, 3);
            Console.WriteLine("*   *   *   **   ** *       *   * *   * *     *   *");
            Console.SetCursorPosition(4, 4);
            Console.WriteLine("*   *  * *  * * * * *       *   * *   * *     *   *");
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("*      * *  *  *  * *       *   * *   * *     *   *");
            Console.SetCursorPosition(4, 6);
            Console.WriteLine("*     *   * *     * *       *   * *   * *     *   *");
            Console.SetCursorPosition(4, 7);
            Console.WriteLine("*  ** ***** *     * ****    *   * *   * ****  **** ");
            Console.SetCursorPosition(4, 8);
            Console.WriteLine("*   * *   * *     * *       *   * *   * *     * *  ");
            Console.SetCursorPosition(4, 9);
            Console.WriteLine("*   * *   * *     * *       *   *  * *  *     *  * ");
            Console.SetCursorPosition(4, 10);
            Console.WriteLine("*   * *   * *     * *       *   *  * *  *     *   *");
            Console.SetCursorPosition(4, 11);
            Console.WriteLine("*   * *   * *     * *       *   *   *   *     *   *");
            Console.SetCursorPosition(4, 12);
            Console.WriteLine(" ***  *   * *     * *****    ***    *   ***** *   *");
            Console.ResetColor();

            Console.SetCursorPosition(18, 14);
            Console.WriteLine("Your score is: {0}", playerScore);
            Console.SetCursorPosition(15, 16);
            Console.WriteLine("Do you want to play again? y/n");
            Console.SetCursorPosition(25, 18);
            Console.ForegroundColor = ConsoleColor.Green;
            string command = Console.ReadLine();

            while (true)
            {
                if (command == "y")
                {
                    Console.Clear();
                    break;
                }
                else if (command == "n")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.SetCursorPosition(20, 18);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Command!");
                    Thread.Sleep(1000);

                    Console.SetCursorPosition(20, 18);
                    Console.WriteLine("                                  ");
                    Console.SetCursorPosition(25, 18);
                    Console.ForegroundColor = ConsoleColor.Green;
                    command = Console.ReadLine();
                }
            }

            lives = 10;
            playerScore = 0;
            RestartGame();
        }

        private static void DetectCollisionsBalloons()
        {
            foreach (var balloon in balloons)
            {
                if ((balloon.CurrentX + (balloon.Radius * 2) + 1 >= playerPositionX) && balloon.CurrentX <= playerPositionX + 4)
                {
                    if (balloon.CurrentY + (balloon.Radius * 2) >= 16)
                    {
                        EndGame();
                        Thread.Sleep(500);
                        lives--;
                        if (lives <= 0) EndGame();
                        
                        RestartGame();
                        break;
                    }
                }
            }
        }

        private static void RestartGame()
        {
            Timer.Restart();

            balloons.Clear();
            balloons.Add(new Balloon(FirstBallonRadius));

            playerPositionX = Console.BufferWidth / 2 - 3;
            playerPositionY = Console.BufferHeight - 3;
        }

        private static void SetBonus()
        {
            int bonusChance = bonusRandom.Next(0, 101);
            if (bonusChance <= 3)
            {
                bonusChar = '@';
                bonusColor = ConsoleColor.Red;
                bonusPositionX = bonusRandom.Next(0, Console.BufferWidth - 1);
                bonusPositionY = 1;
            }
            else if (bonusChance > 3 && bonusChance <= 13)
            {
                bonusChar = '$';
                bonusColor = ConsoleColor.Yellow;
                bonusPositionX = bonusRandom.Next(0, Console.BufferWidth - 1);
                bonusPositionY = 1;
            }
            else if (bonusChance > 13 && bonusChance <= 50)
            {
                bonusChar = '#';
                bonusColor = ConsoleColor.Magenta;
                bonusPositionX = bonusRandom.Next(0, Console.BufferWidth - 1);
                bonusPositionY = 1;
            }
            else
            {
                bonusPositionY = Console.BufferHeight;
            }
        }

        private static void DrawBonus()
        {
            if (bonusPositionY == 1 || bonusPositionY >= Console.BufferHeight - 2)
            {
                SetBonus();
            }

            if (bonusPositionY < Console.BufferHeight - 2)
            {
                Console.SetCursorPosition(bonusPositionX, bonusPositionY);
                Console.ForegroundColor = bonusColor;
                Console.WriteLine(bonusChar);
                bonusCurrentPositionX = bonusPositionX;
                bonusCurrentPositionY = bonusPositionY;
                Console.ForegroundColor = playerColor;
                bonusPositionY++;
            }
        }

        private static void DetectCollisionsBonus()
        {
            if (bonusCurrentPositionX >= playerPositionX && bonusCurrentPositionX + -3 <= playerPositionX)
            {
                if (bonusCurrentPositionY + 2 >= playerPositionY)
                {
                    switch (bonusChar)
                    {
                        case '#':
                            playerScore += 50;
                            break;
                        case '$':
                            playerScore += 100;
                            break;
                        case '@':
                            playerScore += 200;
                            lives++;
                            break;
                    }

                    playerColor = bonusColor;
                }
            }
        }
    }
}

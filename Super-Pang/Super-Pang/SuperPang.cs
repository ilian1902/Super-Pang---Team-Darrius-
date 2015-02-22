using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SuperPang
{
    class SuperPang
    {
        public static string playerHead = "(..)";
        public static string playerTorso = "<][>";
        public static string playerLegs = " /\\";
        public static string playerLegsTogether = " ||";
        public static ConsoleColor playerColor = ConsoleColor.Green;
        public static ConsoleColor sceneColor = ConsoleColor.Green;

        public const int FirstBallonRadius = 2;
        public static List<Balloon> balloons = new List<Balloon>();

        public static int playerPositionX;
        public static int playerPositionY;
        public static int lives;
        public static int playerScore = 0;
        public static int timeLeft = 100;
        //static string shotSymbol = "^\n|";

        // Generate random numbers for bonus.
        public static Random bonusRandom = new Random();
       
        public static int bonusPositionX;
        public static int bonusCurrentPositionX;
        public static int bonusCurrentPositionY;
        public static int bonusPositionY;
        public static char bonusChar;
        public static ConsoleColor bonusColor;

        public static void Main()
        {
            Console.Title = "Super Pang";
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 60;

            playerPositionX = Console.BufferWidth / 2 - 3;
            playerPositionY = Console.BufferHeight - 3;

            lives = 10;
            var firstBalloon = new Balloon(FirstBallonRadius);
            balloons.Add(firstBalloon);

            //First bonus position
            bonusPositionY = 1;

            Thread timing = new Thread(new ThreadStart(TimeCounter));
            Thread music = new Thread(new ThreadStart(Music.PlaySound));
            timing.Start();
            music.Start();

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
                    Draw();

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
            Console.Write("Time: {0}", timeLeft);
        }

        private static void TimeCounter()
        {
            while (timeLeft > 0)
            {
                timeLeft--;
                Thread.Sleep(1000);
            }

            lives--;
        }

        private static void DetectCollisionsBalloons()
        {
            foreach (var balloon in balloons)
            {
                if ((balloon.CurrentX + (balloon.Radius * 2) + 1 >= playerPositionX) && balloon.CurrentX <= playerPositionX + 4)
                {
                    if (balloon.CurrentY + (balloon.Radius * 2) >= 16)
                    {
                        lives--;
                        //TODO restart the game.
                    }
                }
            }
        }

        // Bonuses

        private static void SetBonus()
        {
            int bonus = bonusRandom.Next(0, 100);
            if (bonus <= 3)
            {
                bonusChar = '@';
                bonusColor = ConsoleColor.Red;
                bonusPositionX = bonusRandom.Next(0, Console.BufferWidth - 1);
                bonusPositionY = 1;
            }
            else if (bonus > 3 && bonus <= 13)
            {
                bonusChar = '$';
                bonusColor = ConsoleColor.Yellow;
                bonusPositionX = bonusRandom.Next(0, Console.BufferWidth - 1);
                bonusPositionY = 1;
            }
            else if (bonus > 13 && bonus <= 50)
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
            if (bonusChar == '#' && (bonusCurrentPositionX >= playerPositionX) && (bonusCurrentPositionY == playerPositionY))
            {
                    playerScore+=50;
            }
            if (bonusChar == '$' && (bonusCurrentPositionX >= playerPositionX) && (bonusCurrentPositionY == playerPositionY))
            {
                playerScore += 100;
                playerColor = ConsoleColor.Yellow;
                // strelbata da inishtozhava Big Balloon
            }
            if (bonusChar == '@' && (bonusCurrentPositionX >= playerPositionX) && (bonusCurrentPositionY == playerPositionY))
            {
                playerScore += 200;
                lives++;
                playerColor = ConsoleColor.Red;
                // strelbata da inishtozhava Big Ballon
            }
        }
    }
}

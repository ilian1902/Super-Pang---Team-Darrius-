using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SuperPang
{
    class Program
    {
        public static string playerHead = "(..)";
        public static string playerTorso = "<][>";
        public static string playerLegs = " /\\";
        public static string playerLegsTogether = " ||";
        public const ConsoleColor playerColor = ConsoleColor.Green;
        
        public const int FirstBallonRadius = 2;
        public static List<Balloon> balloons = new List<Balloon>();

        public static int playerPositionX;
        public static int playerPositionY;
        public static int lives;
        //static string shotSymbol = "^\n|";

        public static void Main()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Music.PlaySound();
                }
            });

            Console.Title = "Super Pang";
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 60;

            playerPositionX = Console.BufferWidth / 2;
            playerPositionY = Console.BufferHeight - 3;

            lives = 2;
            var firstBalloon = new Balloon(FirstBallonRadius);
            balloons.Add(firstBalloon);

            Draw();
            Task.Run(() =>
            {
                while (true)
                {
                    Console.Clear();
                    DetectCollisions();
                    MoveAllBalloons();
                    Draw();
                    Thread.Sleep(200);
                }
            });

            while (lives > 0)
            {
                if (Console.KeyAvailable)
                {
                    Console.Clear();
                    DetectCollisions();
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
                    if (balloon.CurrentY > 0) balloon.CurrentY--;
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
            ConsoleKeyInfo pressedKey = Console.ReadKey(false);

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

        private static void Draw()
        {
            DrawPlayer();
            DrawBalloons();
        }

        private static void DrawPlayer()
        {
            Console.SetCursorPosition(playerPositionX, playerPositionY);
            Console.WriteLine(playerHead);

            Console.SetCursorPosition(playerPositionX, playerPositionY + 1);
            Console.WriteLine(playerTorso);

            Console.SetCursorPosition(playerPositionX, playerPositionY + 2);
            if (playerPositionX % 2 == 0) Console.WriteLine(playerLegs);
            else Console.WriteLine(playerLegsTogether);

            Console.ForegroundColor = playerColor;
        }

        private static void DrawBalloons()
        {
            foreach (var balloon in balloons)
            {
                balloon.Draw();
            }
        }

        private static void DetectCollisions()
        {
            foreach (var balloon in balloons)
            {
                if ((balloon.CurrentX + (balloon.Radius * 2) + 1 >= playerPositionX) && balloon.CurrentX <= playerPositionX + 4)
                {
                    if (balloon.CurrentY + (balloon.Radius * 2) >= 16)
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}

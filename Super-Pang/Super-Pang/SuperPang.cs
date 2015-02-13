using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SuperPang
{
    class Program
    {
        public const int FirstBallonRadius = 2;

        static List<Balloon> balloons = new List<Balloon>();
        static List<int> playerCoordinates;

        static int playerPosition = 0;
        static string playerHead = "(..)";
        static string playerTorso = "<][>";
        static string playerLegs = " /\\";
        static string playerLegsTogether = " ||";
        //static string printBaloon = " ### \n#####\n ###";
        //static string shotSymbol = "^\n|";

        static void Main()
        {
            //Music run
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        Music.PlaySound();
            //    }
            //});

            //Console title and boundaries
            Console.Title = "Super Pang";
            Console.BufferHeight = Console.WindowHeight = 20;
            Console.BufferWidth = Console.WindowWidth = 60;

            playerPosition = Console.BufferWidth / 2;

            int livesCount = 2;

            var firstBalloon = new Balloon(FirstBallonRadius);
            balloons.Add(firstBalloon);

            while (livesCount > 0)
            {
                Draw();
                DetectCollisions();
                MovePlayer();
                MoveAllBalloons();

                Thread.Sleep(200);
                Console.Clear();
            }
        }
        private static void Draw()
        {
            DrawPlayer();
            foreach (var balloon in balloons)
            {
                balloon.Draw();
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
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (playerPosition - 1 > 0)
                    {
                        playerPosition--;
                    }
                }

                if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (playerPosition + 1 < Console.WindowWidth - 4)
                    {
                        playerPosition++;
                    }
                }
            }
        }

        private static void DrawPlayer()
        {
            playerCoordinates = new List<int>() { 
                playerPosition, Console.WindowHeight - 1};

            ConsoleColor playerColor = ConsoleColor.Green;

            DrawSymbolAtCoordinates(playerCoordinates, playerHead, playerColor);
        }

        private static void DrawSymbolAtCoordinates(List<int> playerCoordinates, string playerSymbol, ConsoleColor playerColor)
        {
            //Drawing the player simbols int same position
            Console.SetCursorPosition(playerCoordinates[0], playerCoordinates[1]);
            Console.WriteLine(playerSymbol);

            Console.SetCursorPosition(playerCoordinates[0], playerCoordinates[1]);
            Console.WriteLine(playerTorso);

            if (playerPosition % 2 == 0)
            {
                Console.SetCursorPosition(playerCoordinates[0], playerCoordinates[1]);
                Console.WriteLine(playerLegs);
            }
            else
            {
                Console.SetCursorPosition(playerCoordinates[0], playerCoordinates[1]);
                Console.WriteLine(playerLegsTogether);
            }

            Console.ForegroundColor = playerColor;
        }

        private static void DetectCollisions()
        {
            foreach (var balloon in balloons)
            {
                // balloon.CurrentY + (balloon.Radius * 2) + 1 >= 18
                //balloon.CurrentX == playerCoordinates[0] &&
                if ((balloon.CurrentX + (balloon.Radius * 2) + 1 >= playerCoordinates[0]) && balloon.CurrentX <= playerCoordinates[0] + 4)
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

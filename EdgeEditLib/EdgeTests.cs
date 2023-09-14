using System;
using EdgeEditLib;
using EdgeEditLib.Blocks;

namespace EdgeEditLib
{
    public static class EdgeTests
    {

        private static char[] tilePalette = new char[] { ' ', '░', '█', '▓' };

        /// <summary>
        /// A simple method to render entire level to the console for debug purposes
        /// </summary>
        public static void ConsoleDraw(Level level)
        {
            int[,] tileMap = new int[level.Map.Width, level.Map.Length];

            for (int x = 0; x < level.Map.Width; x++)
            {
                for (int y = 0; y < level.Map.Length; y++)
                {
                    int height = level.Map.GetHeightAt(x, y);
                    tileMap[x, y] = level.Map[x, y, height] ? 1 : 0;
                }
            }

            for (int i = 0; i < level.moving_platforms.Count; i++)
            {
                int x = level.moving_platforms[i].GetWaypointAt(0).position.X;
                int y = level.moving_platforms[i].GetWaypointAt(0).position.Y;

                // Moving platforms unlike static blocks, can be outside the level
                if (level.Map.IsInBounds(x, y, 0))
                {
                    tileMap[x, y] = 2;
                }
            }

            for (int i = 0; i < level.bumpers.Count; i++)
            {
                int x = level.bumpers[i].position.X;
                int y = level.bumpers[i].position.Y;

                if (level.Map.IsInBounds(x, y, 0))
                {
                    tileMap[x, y] = 3;
                }
            }

            for (int i = 0; i < level.falling_platforms.Count; i++)
            {
                int x = level.falling_platforms[i].position.X;
                int y = level.falling_platforms[i].position.Y;

                if (level.Map.IsInBounds(x, y, 0))
                {
                    tileMap[x, y] = 4;
                }
            }

            for (int i = 0; i < level.prisms.Count; i++)
            {
                int x = level.prisms[i].position.X;
                int y = level.prisms[i].position.Y;

                if (level.Map.IsInBounds(x, y, 0))
                {
                    tileMap[x, y] = 5;
                }
            }

            for (int i = 0; i < level.buttons.Count; i++)
            {
                int x = level.buttons[i].position.X;
                int y = level.buttons[i].position.Y;

                if (level.Map.IsInBounds(x, y, 0))
                {
                    tileMap[x, y] = 6;
                }
            }
            for (int i = 0; i < level.camera_triggers.Count; i++)
            {
                int x = level.camera_triggers[i].position.X;
                int y = level.camera_triggers[i].position.Y;
                if (level.Map.IsInBounds(x, y, 0))
                {
                    tileMap[x, y] = 7;
                }

            }


            for (int j = 0; j < level.Map.Length; j++)
            {
                for (int i = 0; i < level.Map.Width; i++)
                {
                    int x = i;
                    int y = j;
                    //Console.WriteLine(x + " " + y);
                    if (tileMap[x, y] == 1) // Static block
                    {
                        int height = level.Map.GetHeightAt(x, y);
                        switch (height)
                        {
                            case 0:
                                DrawTile(1, ConsoleColor.DarkGray);
                                break;
                            case 1:
                                DrawTile(2, ConsoleColor.DarkGray);
                                break;
                            case 2:
                                DrawTile(2, ConsoleColor.Gray);
                                break;
                            case 3:
                                DrawTile(2, ConsoleColor.White);
                                break;
                            case 4:
                                DrawTile(2, ConsoleColor.White);
                                break;
                        }
                    }
                    else if (tileMap[x, y] == 2) // Moving platform
                    {
                        DrawTile(2, ConsoleColor.Blue);
                    }
                    else if (tileMap[x, y] == 3) // Bumper
                    {
                        DrawTile(3, ConsoleColor.DarkYellow);
                    }
                    else if (tileMap[x, y] == 4) // Falling platform
                    {
                        DrawTile(1, ConsoleColor.Red);
                    }
                    else if (tileMap[x, y] == 5) // Prism
                    {
                        DrawTile(3, ConsoleColor.Magenta);
                    }
                    else if (tileMap[x, y] == 6) // Button
                    {
                        DrawTile(3, ConsoleColor.Red);
                    }
                    else if (tileMap[x, y] == 7) // Button
                    {
                        DrawTile(2, ConsoleColor.Green);
                    }
                    else
                    {
                        DrawTile(0);
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public static void WriteAboutBlocks()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(tilePalette[2] + " = static block");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(tilePalette[2] + " = moving platform");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(tilePalette[1] + " = bumper");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(tilePalette[1] + " = falling platform");
        }

        private static void DrawTile(int tile, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            char c = tilePalette[0];
            if (tile == 0) // Air
            {
                c = tilePalette[0];
            }
            else if (tile == 1) // Ground
            {
                c = tilePalette[1];
            }
            else if (tile == 2) // Solid block
            {
                c = tilePalette[2];
            }
            else if (tile == 3) // Point
            {
                c = tilePalette[3];
            }
            Console.Write(c);
            Console.Write(c);
        }
    }
}

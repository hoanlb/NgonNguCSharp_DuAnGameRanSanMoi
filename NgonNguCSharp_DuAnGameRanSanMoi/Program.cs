using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NgonNguCSharp_DuAnGameRanSanMoi
{
    internal class Program
    {
        static readonly int width = 20;
        static readonly int height = 10;
        static int score = 0;
        static bool gameOver = false;
        static Direction direction = Direction.Right;
        static Random random = new Random();
        static List<Position> snake = new List<Position>();
        static Position food;

        enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        struct Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 1, height + 1);
            Console.SetBufferSize(width + 1, height + 1);

            InitializeGame();

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangeDirection(key);
                }

                MoveSnake();
                CheckCollision();
                Draw();
                Thread.Sleep(100);
            }

            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.WriteLine("Game Over! Score: " + score);

            Console.ReadKey();
        }

        static void InitializeGame()
        {
            snake.Clear();
            snake.Add(new Position { X = width / 2, Y = height / 2 });

            food = new Position { X = random.Next(0, width), Y = random.Next(0, height) };
        }

        static void Draw()
        {
            Console.Clear();

            Console.SetCursorPosition(food.X, food.Y);
            Console.Write("*");

            foreach (var segment in snake)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write("O");
            }
        }

        static void MoveSnake()
        {
            Position head = snake.First();
            Position newHead = new Position();

            switch (direction)
            {
                case Direction.Left:
                    newHead.X = head.X - 1;
                    newHead.Y = head.Y;
                    break;
                case Direction.Right:
                    newHead.X = head.X + 1;
                    newHead.Y = head.Y;
                    break;
                case Direction.Up:
                    newHead.X = head.X;
                    newHead.Y = head.Y - 1;
                    break;
                case Direction.Down:
                    newHead.X = head.X;
                    newHead.Y = head.Y + 1;
                    break;
            }

            snake.Insert(0, newHead);

            if (snake.First().X == food.X && snake.First().Y == food.Y)
            {
                score++;
                food = new Position { X = random.Next(0, width), Y = random.Next(0, height) };
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void CheckCollision()
        {
            Position head = snake.First();

            if (head.X < 0 || head.X >= width || head.Y < 0 || head.Y >= height || snake.Skip(1).Any(s => s.X == head.X && s.Y == head.Y))
            {
                gameOver = true;
            }
        }

        static void ChangeDirection(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (direction != Direction.Right)
                        direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (direction != Direction.Left)
                        direction = Direction.Right;
                    break;
                case ConsoleKey.UpArrow:
                    if (direction != Direction.Down)
                        direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != Direction.Up)
                        direction = Direction.Down;
                    break;
            }
        }
    }
}

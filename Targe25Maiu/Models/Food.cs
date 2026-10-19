
using System;
using System.Collections.Generic;

namespace Targe25Maiu.SnakeGame
{
    public class Food
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Symbol { get; private set; }
        public int Points { get; private set; }

        private readonly Random random = new Random();

        private readonly string[] fruits =
        {
            "🍎", "🍌", "🍓", "🍇"
        };

        public Food()
        {
            Symbol = "🍎";
            Points = 10;
        }

        // Loome toidu juhuslikku vabasse kohta
        public bool Generate(
            int width,
            int height,
            List<(int X, int Y)> snakeBody)
        {
            List<(int X, int Y)> freeCells = new();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!snakeBody.Contains((x, y)))
                    {
                        freeCells.Add((x, y));
                    }
                }
            }

            if (freeCells.Count == 0)
                return false;

            var position = freeCells[random.Next(freeCells.Count)];

            X = position.X;
            Y = position.Y;

            Symbol = fruits[random.Next(fruits.Length)];
            Points = 10;

            return true;
        }
    }
}

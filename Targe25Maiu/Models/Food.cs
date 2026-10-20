
using System;
using System.Collections.Generic;

namespace Targe25Maiu.SnakeGame
{
    public class Food
    {
        private readonly Random random = new Random();

        private readonly string[] crystals =
        {
            "💎", "🔮", "💠"
        };

        private readonly string[] bonusCrystals =
        {
            "🌟", "✨", "💛"
        };

        public int X { get; private set; } = -1;
        public int Y { get; private set; } = -1;

        public string Symbol { get; private set; } = "💎";

        public int Points { get; private set; } = 100;

        public bool IsBonus { get; private set; }

        public SnakePoint Position
        {
            get
            {
                return new SnakePoint(X, Y);
            }
        }

        public Food()
        {
            MakeNormal();
        }

        // ÜHILDUVUS VANA SNAKE KLASSIGA
        public bool Generate(
            int width,
            int height,
            List<(int X, int Y)> snakeBody)
        {
            List<SnakePoint> body = new List<SnakePoint>();

            foreach (var part in snakeBody)
            {
                body.Add(new SnakePoint(part.X, part.Y));
            }

            return Generate(
                width,
                height,
                body,
                new List<SnakePoint>(),
                new List<SnakePoint>(),
                false);
        }

        // ÜHILDUVUS VANA GENERATE MEETODIGA
        public bool Generate(
            int width,
            int height,
            List<(int X, int Y)> snakeBody,
            List<(int X, int Y)> rocks,
            List<(int X, int Y)> otherCrystals,
            bool bonus = false)
        {
            List<SnakePoint> body = ConvertPoints(snakeBody);
            List<SnakePoint> obstacles = ConvertPoints(rocks);
            List<SnakePoint> crystalsOnBoard =
                ConvertPoints(otherCrystals);

            return Generate(
                width,
                height,
                body,
                obstacles,
                crystalsOnBoard,
                bonus);
        }

        // UUE DRAAKONIMÄNGU KRISTALLI LOOMINE
        public bool Generate(
            int width,
            int height,
            List<SnakePoint> snakeBody,
            List<SnakePoint> rocks,
            List<SnakePoint> otherCrystals,
            bool bonus = false)
        {
            if (width <= 0 || height <= 0)
                return false;

            List<SnakePoint> freeCells =
                new List<SnakePoint>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SnakePoint position =
                        new SnakePoint(x, y);

                    if (snakeBody.Contains(position))
                        continue;

                    if (rocks.Contains(position))
                        continue;

                    if (otherCrystals.Contains(position))
                        continue;

                    freeCells.Add(position);
                }
            }

            if (freeCells.Count == 0)
            {
                X = -1;
                Y = -1;
                return false;
            }

            SnakePoint selected =
                freeCells[random.Next(freeCells.Count)];

            X = selected.X;
            Y = selected.Y;

            if (bonus)
            {
                MakeBonus();
            }
            else
            {
                MakeNormal();
            }

            return true;
        }

        // BOONUSKRISTALL: +200
        public void MakeBonus()
        {
            IsBonus = true;
            Points = 200;

            Symbol = bonusCrystals[
                random.Next(bonusCrystals.Length)];
        }

        // TAVALINE KRISTALL: +100
        public void MakeNormal()
        {
            IsBonus = false;
            Points = 100;

            Symbol = crystals[
                random.Next(crystals.Length)];
        }

        // MUUDAME VANA KOORDINAADITÜÜBI UUEKS
        private static List<SnakePoint> ConvertPoints(
            List<(int X, int Y)> points)
        {
            List<SnakePoint> result =
                new List<SnakePoint>();

            foreach (var point in points)
            {
                result.Add(
                    new SnakePoint(point.X, point.Y));
            }

            return result;
        }
    }
}

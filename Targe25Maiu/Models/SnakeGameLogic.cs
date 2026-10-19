
using System;
using System.Collections.Generic;
using System.Linq;

namespace Targe25Maiu.SnakeGame
{
    public class SnakeGameLogic
    {
        private readonly Random random = new Random();

        public Snake Snake { get; private set; }
        public Food Food { get; private set; }

        public int Width { get; } = 15;
        public int Height { get; } = 15;

        public int Score { get; private set; }
        public int BerriesCollected { get; private set; }
        public int BonusProgress => BerriesCollected % 5;

        public int LastPoints { get; private set; }
        public int EatSequence { get; private set; }

        public bool IsGameOver { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsWon { get; private set; }

        // Kaks boonusmarja
        public List<(int X, int Y)> BonusBerries { get; }
            = new List<(int X, int Y)>();

        public SnakeGameLogic()
        {
            Snake = new Snake();
            Food = new Food();

            Start();
        }

        // Uue mängu alustamine
        public void Start()
        {
            Snake = new Snake();
            Food = new Food();

            Score = 0;
            BerriesCollected = 0;
            LastPoints = 0;
            EatSequence = 0;

            IsGameOver = false;
            IsPaused = false;
            IsWon = false;

            BonusBerries.Clear();

            GenerateFood();
        }

        // Mängu peatamine
        public void Pause()
        {
            if (!IsGameOver)
                IsPaused = true;
        }

        // Mängu jätkamine
        public void Resume()
        {
            if (!IsGameOver)
                IsPaused = false;
        }

        // Ussi liikumissuund
        public void ChangeDirection(int x, int y)
        {
            if (IsGameOver || IsPaused)
                return;

            Snake.ChangeDirection(x, y);
        }

        // Tavalise marja loomine
        private bool GenerateFood()
        {
            var blocked = Snake.Body
                .Concat(BonusBerries)
                .ToList();

            return Food.Generate(
                Width,
                Height,
                blocked
            );
        }

        // Kahe boonusmarja loomine
        private void GenerateBonusBerries()
        {
            BonusBerries.Clear();

            List<(int X, int Y)> freeCells =
                new List<(int X, int Y)>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var position = (X: x, Y: y);

                    // Ei teki ussi keha sisse
                    if (Snake.Body.Contains(position))
                        continue;

                    // Ei teki tavalise marja peale
                    if (position == (Food.X, Food.Y))
                        continue;

                    freeCells.Add(position);
                }
            }

            // Loome kaks boonusmarja
            for (int i = 0; i < 2 && freeCells.Count > 0; i++)
            {
                int index = random.Next(freeCells.Count);

                BonusBerries.Add(freeCells[index]);

                freeCells.RemoveAt(index);
            }
        }

        // Üks mängusamm
        // Tagastab söödud marja punktid
        public int Update()
        {
            LastPoints = 0;

            if (IsGameOver || IsPaused)
                return 0;

            var head = Snake.Body[0];

            int nextX = head.X + Snake.DirectionX;
            int nextY = head.Y + Snake.DirectionY;

            // Seina kontroll
            if (nextX < 0 || nextX >= Width ||
                nextY < 0 || nextY >= Height)
            {
                IsGameOver = true;
                return 0;
            }

            var nextPosition = (X: nextX, Y: nextY);

            // Tavalise marja kontroll
            bool normalBerry =
                nextX == Food.X &&
                nextY == Food.Y;

            // Boonusmarja kontroll
            bool bonusBerry =
                BonusBerries.Contains(nextPosition);

            bool eating = normalBerry || bonusBerry;

            // Kontrollime kokkupõrget enda kehaga
            int bodyCount = eating
                ? Snake.Body.Count
                : Snake.Body.Count - 1;

            if (Snake.Body
                .Take(bodyCount)
                .Contains(nextPosition))
            {
                IsGameOver = true;
                return 0;
            }

            // Liigutame ussi
            Snake.Move(eating);

            // Tavaline mari
            if (normalBerry)
            {
                LastPoints = 100;
                BerriesCollected++;

                bool hasSpace = GenerateFood();

                if (!hasSpace)
                {
                    IsWon = true;
                    IsGameOver = true;
                }

                // Iga 5 marja järel kaks boonusmarja
                if (BerriesCollected % 5 == 0 && !IsGameOver)
                {
                    GenerateBonusBerries();
                }
            }
            // Boonusmari
            else if (bonusBerry)
            {
                LastPoints = 200;

                BonusBerries.Remove(nextPosition);
            }

            // Punktide lisamine
            if (LastPoints > 0)
            {
                Score += LastPoints;
                EatSequence++;
            }

            return LastPoints;
        }
    }
}

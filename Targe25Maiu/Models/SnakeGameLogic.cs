
using System;
using System.Collections.Generic;
using System.Linq;

namespace Targe25Maiu.SnakeGame
{
    // Mängu maailmad
    public enum DragonWorld
    {
        Forest,
        Night,
        Candy
    }

    // Raskusastmed
    public enum DragonDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    // Ühe ruudu koordinaadid
    public readonly record struct SnakePoint(int X, int Y);

    public class SnakeGameLogic
    {
        private readonly Random random = new Random();

        // Mänguväljak
        public int Width => 15;
        public int Height => 15;

        // Draakon
        public Snake Snake { get; } = new Snake();

        // Tavaline kristall
        public SnakePoint Food { get; private set; }

        // Boonuskristallid
        public List<SnakePoint> BonusBerries { get; } =
            new List<SnakePoint>();

        // Kivid
        public List<SnakePoint> Rocks { get; } =
            new List<SnakePoint>();

        // Punktid
        public int Score { get; private set; }

        public int BerriesCollected { get; private set; }

        public int BonusesCollected { get; private set; }

        public int BonusProgress => BerriesCollected % 5;

        // Mängu olek
        public bool IsPaused { get; private set; }

        public bool IsGameOver { get; private set; }

        public bool IsWon { get; private set; }

        // Raskusaste
        public DragonDifficulty Difficulty { get; private set; }
            = DragonDifficulty.Normal;

        public SnakeGameLogic()
        {
            Start();
        }

        // UUS MÄNG
        public void Start()
        {
            Score = 0;
            BerriesCollected = 0;
            BonusesCollected = 0;

            IsPaused = false;
            IsGameOver = false;
            IsWon = false;

            Snake.Reset();

            BonusBerries.Clear();
            Rocks.Clear();

            Food = new SnakePoint(-1, -1);

            GenerateRocks();
            GenerateFood();
        }

        // RASKUSASTE
        public void SetDifficulty(DragonDifficulty selected)
        {
            Difficulty = selected;
        }

        // PAUS
        public void Pause()
        {
            if (!IsGameOver)
            {
                IsPaused = true;
            }
        }

        // JÄTKA
        public void Resume()
        {
            if (!IsGameOver)
            {
                IsPaused = false;
            }
        }

        // SUUNA MUUTMINE
        public void ChangeDirection(int dx, int dy)
        {
            if (IsPaused || IsGameOver)
                return;

            Snake.ChangeDirection(dx, dy);
        }

        // MÄNGU ÜKS SAMM
        public int Update()
        {
            if (IsPaused || IsGameOver)
                return 0;

            Snake.ApplyDirection();

            SnakePoint head = Snake.Body[0];

            SnakePoint next = new SnakePoint(
                head.X + Snake.DirectionX,
                head.Y + Snake.DirectionY);

            // Seinaga kokkupõrge
            if (next.X < 0 ||
                next.X >= Width ||
                next.Y < 0 ||
                next.Y >= Height)
            {
                EndGame();
                return 0;
            }

            // Kiviga kokkupõrge
            if (Rocks.Contains(next))
            {
                EndGame();
                return 0;
            }

            bool normalCrystal = next == Food;

            bool bonusCrystal =
                BonusBerries.Contains(next);

            bool growing =
                normalCrystal || bonusCrystal;

            // Kontrollime kokkupõrget oma kehaga
            int bodyCount = growing
                ? Snake.Body.Count
                : Snake.Body.Count - 1;

            for (int i = 0; i < bodyCount; i++)
            {
                if (Snake.Body[i] == next)
                {
                    EndGame();
                    return 0;
                }
            }

            // Liigutame draakonit
            Snake.Body.Insert(0, next);

            if (!growing)
            {
                Snake.Body.RemoveAt(
                    Snake.Body.Count - 1);
            }

            int gainedPoints = 0;

            // TAVALINE KRISTALL +100
            if (normalCrystal)
            {
                Score += 100;
                gainedPoints += 100;

                BerriesCollected++;

                // Eemaldame vana kristalli
                Food = new SnakePoint(-1, -1);

                // Loome esmalt uue tavalise kristalli
                GenerateFood();

                // Iga viie kristalli järel kaks boonust
                if (!IsGameOver &&
                    BerriesCollected % 5 == 0)
                {
                    GenerateBonus();
                    GenerateBonus();
                }

                UpdateRocks();
            }

            // BOONUSKRISTALL +200
            if (bonusCrystal)
            {
                BonusBerries.Remove(next);

                Score += 200;
                gainedPoints += 200;

                BonusesCollected++;

                UpdateRocks();
            }

            // Võit, kui kõik mängitavad ruudud
            // on draakoni kehaga täidetud
            int availableCells =
                Width * Height - Rocks.Count;

            if (Snake.Body.Count >= availableCells)
            {
                IsWon = true;
                IsGameOver = true;
            }

            return gainedPoints;
        }

        // MÄNG LÄBI
        private void EndGame()
        {
            IsGameOver = true;
            IsPaused = false;
        }

        // KIVIDE ALGARV
        private int GetStartingRocks()
        {
            switch (Difficulty)
            {
                case DragonDifficulty.Easy:
                    return 0;

                case DragonDifficulty.Normal:
                    return 2;

                case DragonDifficulty.Hard:
                    return 4;

                default:
                    return 2;
            }
        }

        // ALGSETE KIVIDE LOOMINE
        private void GenerateRocks()
        {
            Rocks.Clear();

            int count = GetStartingRocks();

            for (int i = 0; i < count; i++)
            {
                if (!TryFindFreePoint(out SnakePoint point))
                    break;

                Rocks.Add(point);
            }
        }

        // IGA 1000 PUNKTI JÄREL +2 KIVI
        private void UpdateRocks()
        {
            int extraRocks = (Score / 1000) * 2;

            int targetCount = Math.Min(
                30,
                GetStartingRocks() + extraRocks);

            while (Rocks.Count < targetCount)
            {
                if (!TryFindFreePoint(out SnakePoint point))
                    break;

                Rocks.Add(point);
            }
        }

        // UUS TAVALINE KRISTALL
        private void GenerateFood()
        {
            Food = new SnakePoint(-1, -1);

            if (TryFindFreePoint(out SnakePoint point))
            {
                Food = point;
            }
            else
            {
                // Kui boonused on veel alles,
                // saab draakon neid edasi koguda
                if (BonusBerries.Count == 0)
                {
                    IsWon = true;
                    IsGameOver = true;
                }
            }
        }

        // BOONUSKRISTALL
        private void GenerateBonus()
        {
            if (TryFindFreePoint(out SnakePoint point))
            {
                BonusBerries.Add(point);
            }
        }

        // VABA RUUDU LEIDMINE
        private bool TryFindFreePoint(out SnakePoint point)
        {
            List<SnakePoint> available =
                new List<SnakePoint>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SnakePoint candidate =
                        new SnakePoint(x, y);

                    if (Snake.Body.Contains(candidate))
                        continue;

                    if (Rocks.Contains(candidate))
                        continue;

                    if (BonusBerries.Contains(candidate))
                        continue;

                    if (candidate == Food)
                        continue;

                    available.Add(candidate);
                }
            }

            if (available.Count == 0)
            {
                point = default;
                return false;
            }

            point = available[
                random.Next(available.Count)];

            return true;
        }
    }
}

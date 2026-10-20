
using System;
using System.Collections.Generic;

namespace Targe25Maiu.SnakeGame
{
    public class Snake
    {
        // Draakoni keha
        public List<SnakePoint> Body { get; private set; }

        // Praegune liikumissuund
        public int DirectionX { get; private set; }
        public int DirectionY { get; private set; }

        // Järgmised suunamuutused
        private readonly Queue<SnakePoint> directionQueue =
            new Queue<SnakePoint>();

        public Snake()
        {
            Body = new List<SnakePoint>();
            Reset();
        }

        // DRAAKONI ALGASEND
        public void Reset()
        {
            Body.Clear();

            Body.Add(new SnakePoint(7, 7));
            Body.Add(new SnakePoint(6, 7));
            Body.Add(new SnakePoint(5, 7));

            DirectionX = 1;
            DirectionY = 0;

            directionQueue.Clear();
        }

        // SUUNA MUUTMINE
        public void ChangeDirection(int x, int y)
        {
            // Lubatud on ainult neli suunda
            if (Math.Abs(x) + Math.Abs(y) != 1)
                return;

            int lastX = DirectionX;
            int lastY = DirectionY;

            // Kontrollime viimast järjekorras olevat suunda
            if (directionQueue.Count > 0)
            {
                SnakePoint last = new SnakePoint(
                    DirectionX, DirectionY);

                foreach (SnakePoint direction in directionQueue)
                {
                    last = direction;
                }

                lastX = last.X;
                lastY = last.Y;
            }

            // Draakon ei saa otse tagasi pöörata
            if (x == -lastX && y == -lastY)
                return;

            // Sama suunda ei lisa uuesti
            if (x == lastX && y == lastY)
                return;

            // Salvestame kuni kaks järgmist pööret
            if (directionQueue.Count < 2)
            {
                directionQueue.Enqueue(
                    new SnakePoint(x, y));
            }
        }

        // RAKENDAME JÄRGMISE SUUNA
        public void ApplyDirection()
        {
            if (directionQueue.Count == 0)
                return;

            SnakePoint nextDirection =
                directionQueue.Dequeue();

            DirectionX = nextDirection.X;
            DirectionY = nextDirection.Y;
        }

        // DRAAKONI LIIKUMINE
        public void Move(bool grow = false)
        {
            ApplyDirection();

            SnakePoint head = Body[0];

            SnakePoint newHead = new SnakePoint(
                head.X + DirectionX,
                head.Y + DirectionY);

            Body.Insert(0, newHead);

            // Kui kristalli ei korjatud, eemaldame saba
            if (!grow)
            {
                Body.RemoveAt(Body.Count - 1);
            }
        }
    }
}

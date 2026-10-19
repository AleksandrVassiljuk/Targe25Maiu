
using System.Collections.Generic;

namespace Targe25Maiu.SnakeGame
{
    public class Snake
    {
        public List<(int X, int Y)> Body { get; private set; }

        public int DirectionX { get; private set; }
        public int DirectionY { get; private set; }

        public Snake()
        {
            Body = new List<(int X, int Y)>
            {
                (5, 5),
                (4, 5),
                (3, 5)
            };

            DirectionX = 1;
            DirectionY = 0;
        }

        public void ChangeDirection(int x, int y)
        {
            // Uss ei saa otse tagasi pöörata
            if (x == -DirectionX && y == -DirectionY)
                return;

            if (Math.Abs(x) + Math.Abs(y) != 1)
                return;

            DirectionX = x;
            DirectionY = y;
        }

        public void Move(bool grow = false)
        {
            var head = Body[0];

            var newHead = (
                X: head.X + DirectionX,
                Y: head.Y + DirectionY
            );

            Body.Insert(0, newHead);

            if (!grow)
            {
                Body.RemoveAt(Body.Count - 1);
            }
        }
    }
}

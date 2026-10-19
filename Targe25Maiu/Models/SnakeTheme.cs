
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Targe25Maiu.SnakeGame
{
    public class SnakeTheme
    {
        public string Name { get; private set; }
        public Color BackgroundColor { get; private set; }
        public Color BoardColor { get; private set; }
        public Color AlternateBoardColor { get; private set; }
        public Color SnakeColor { get; private set; }
        public Color TextColor { get; private set; }
        public string FontFamily { get; private set; }

        public SnakeTheme(
            string name,
            Color backgroundColor,
            Color boardColor,
            Color alternateBoardColor,
            Color snakeColor,
            Color textColor,
            string fontFamily)
        {
            Name = name;
            BackgroundColor = backgroundColor;
            BoardColor = boardColor;
            AlternateBoardColor = alternateBoardColor;
            SnakeColor = snakeColor;
            TextColor = textColor;
            FontFamily = fontFamily;
        }

        // Rakendab teema lehele
        public void Apply(ContentPage page)
        {
            page.BackgroundColor = BackgroundColor;

            page.Resources["SnakeTextColor"] = TextColor;
            page.Resources["SnakeBackgroundColor"] = BackgroundColor;
            page.Resources["SnakeBoardColor"] = BoardColor;
            page.Resources["SnakeAlternateBoardColor"] =
                AlternateBoardColor;
            page.Resources["SnakeBodyColor"] = SnakeColor;
            page.Resources["SnakeFontFamily"] = FontFamily;
        }

        // Roheline teema
        public static SnakeTheme Grass()
        {
            return new SnakeTheme(
                "Grass",
                Color.FromArgb("#E8F5E9"),
                Color.FromArgb("#A5D66D"),
                Color.FromArgb("#8BC85A"),
                Color.FromArgb("#43A047"),
                Color.FromArgb("#234B25"),
                "Arial"
            );
        }

        // Tume teema
        public static SnakeTheme Night()
        {
            return new SnakeTheme(
                "Night",
                Color.FromArgb("#101827"),
                Color.FromArgb("#24364B"),
                Color.FromArgb("#33475C"),
                Color.FromArgb("#2DD4BF"),
                Colors.White,
                "Courier New"
            );
        }

        // Värviline teema
        public static SnakeTheme Candy()
        {
            return new SnakeTheme(
                "Candy",
                Color.FromArgb("#FCE7F3"),
                Color.FromArgb("#FBCFE8"),
                Color.FromArgb("#F9A8D4"),
                Color.FromArgb("#A855F7"),
                Color.FromArgb("#831843"),
                "Georgia"
            );
        }
    }
}

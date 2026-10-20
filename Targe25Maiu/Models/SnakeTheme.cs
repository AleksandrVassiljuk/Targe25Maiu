
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

        public Color BorderColor { get; private set; }

        public Color CrystalColor { get; private set; }

        public Color BonusColor { get; private set; }

        public Color PanelColor { get; private set; }

        // VANA KONSTRUKTOR - 7 PARAMEETRIT
        public SnakeTheme(
            string name,
            Color backgroundColor,
            Color boardColor,
            Color alternateBoardColor,
            Color snakeColor,
            Color textColor,
            string fontFamily)
            : this(
                name,
                backgroundColor,
                boardColor,
                alternateBoardColor,
                snakeColor,
                textColor,
                fontFamily,
                Color.FromArgb("#B28B4C"),
                Color.FromArgb("#3BBED6"),
                Color.FromArgb("#FFD166"),
                Color.FromArgb("#29213F"))
        {
        }

        // UUS KONSTRUKTOR - 11 PARAMEETRIT
        public SnakeTheme(
            string name,
            Color backgroundColor,
            Color boardColor,
            Color alternateBoardColor,
            Color snakeColor,
            Color textColor,
            string fontFamily,
            Color borderColor,
            Color crystalColor,
            Color bonusColor,
            Color panelColor)
        {
            Name = name;
            BackgroundColor = backgroundColor;
            BoardColor = boardColor;
            AlternateBoardColor = alternateBoardColor;
            SnakeColor = snakeColor;
            TextColor = textColor;
            FontFamily = fontFamily;

            BorderColor = borderColor;
            CrystalColor = crystalColor;
            BonusColor = bonusColor;
            PanelColor = panelColor;
        }

        // TEEMA RAKENDAMINE
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

            page.Resources["SnakeBorderColor"] = BorderColor;

            page.Resources["SnakeCrystalColor"] = CrystalColor;

            page.Resources["SnakeBonusColor"] = BonusColor;

            page.Resources["SnakePanelColor"] = PanelColor;
        }

        // FOREST
        public static SnakeTheme Forest()
        {
            return new SnakeTheme(
                "Forest",
                Color.FromArgb("#171329"),
                Color.FromArgb("#243F3B"),
                Color.FromArgb("#2B4942"),
                Color.FromArgb("#43B96C"),
                Color.FromArgb("#E9D5FF"),
                "Arial",
                Color.FromArgb("#B28B4C"),
                Color.FromArgb("#3BBED6"),
                Color.FromArgb("#FFD166"),
                Color.FromArgb("#29213F")
            );
        }

        // VANA GRASS NIME TOETUS
        public static SnakeTheme Grass()
        {
            return Forest();
        }

        // NIGHT
        public static SnakeTheme Night()
        {
            return new SnakeTheme(
                "Night",
                Color.FromArgb("#171329"),
                Color.FromArgb("#172538"),
                Color.FromArgb("#203149"),
                Color.FromArgb("#43B96C"),
                Color.FromArgb("#E9D5FF"),
                "Arial",
                Color.FromArgb("#8C83D6"),
                Color.FromArgb("#70E5FF"),
                Color.FromArgb("#FFD166"),
                Color.FromArgb("#29213F")
            );
        }

        // CANDY
        public static SnakeTheme Candy()
        {
            return new SnakeTheme(
                "Candy",
                Color.FromArgb("#171329"),
                Color.FromArgb("#594070"),
                Color.FromArgb("#694B81"),
                Color.FromArgb("#43B96C"),
                Color.FromArgb("#FFE1F4"),
                "Arial",
                Color.FromArgb("#F3A5DA"),
                Color.FromArgb("#70E5FF"),
                Color.FromArgb("#FFD166"),
                Color.FromArgb("#382646")
            );
        }
    }
}

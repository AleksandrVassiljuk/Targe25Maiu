
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using Targe25Maiu.SnakeGame;

namespace Targe25Maiu
{
    public partial class SnakePage : ContentPage
    {
        private readonly SnakeGameLogic game;
        private SnakeTheme currentTheme;

        private readonly IDispatcherTimer gameTimer;
        private readonly SnakeBoardDrawable boardDrawable;

        private int bestScore;
        private bool gameOverShown;
        private int popupVersion;
        private bool pageVisible;

        public SnakePage()
        {
            InitializeComponent();

            game = new SnakeGameLogic();
            currentTheme = SnakeTheme.Grass();

            boardDrawable = new SnakeBoardDrawable
            {
                Game = game,
                Theme = currentTheme
            };

            GameCanvas.Drawable = boardDrawable;

            gameTimer = Dispatcher.CreateTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameTimer_Tick;

            bestScore = Preferences.Default.Get(
                "DragonSnakeBestScore", 0);

            ThemePicker.SelectedIndex = 0;
            SpeedPicker.SelectedIndex = 1;

            ApplyTheme();
            UpdateStats();

            StatusLabel.Text =
                "🐉 Juhi draakon maagiliste kristallideni!";

            GameCanvas.Invalidate();
        }

        // Käivitame mängu siis, kui leht avaneb
        protected override void OnAppearing()
        {
            base.OnAppearing();

            pageVisible = true;

            GameCanvas.Invalidate();

            if (!game.IsGameOver && !game.IsPaused)
            {
                gameTimer.Start();
            }
        }

        // Peatame mängu lehelt lahkudes
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            pageVisible = false;
            gameTimer.Stop();

            popupVersion++;
            PointsPopup.CancelAnimations();
            PointsPopup.IsVisible = false;

            if (!game.IsGameOver)
            {
                game.Pause();

                PauseButton.Text = "▶ Jätka";
                StatusLabel.Text = "⏸ Mäng on pausil";
            }
        }

        // Mängu liikumine
        private async void GameTimer_Tick(
            object? sender, EventArgs e)
        {
            if (!pageVisible ||
                game.IsPaused ||
                game.IsGameOver)
            {
                return;
            }

            int points = game.Update();

            if (game.Score > bestScore)
            {
                bestScore = game.Score;

                Preferences.Default.Set(
                    "DragonSnakeBestScore", bestScore);
            }

            UpdateStats();
            GameCanvas.Invalidate();

            if (points > 0)
            {
                _ = ShowPointsPopup(points);
            }

            if (game.IsGameOver && !gameOverShown)
            {
                gameOverShown = true;
                gameTimer.Stop();

                StatusLabel.Text = game.IsWon
                    ? "🏆 Võitsid mängu!"
                    : "💀 Draakoni seiklus lõppes!";

                string message = game.IsWon
                    ? $"Võitsid!\nPunktid: {game.Score}"
                    : $"Mäng läbi!\n\n" +
                      $"Punktid: {game.Score}\n" +
                      $"Kristallid: {game.BerriesCollected}\n" +
                      $"Rekord: {bestScore}";

                await DisplayAlert(
                    "Fantasy Dragon Adventure",
                    message,
                    "OK");
            }
        }

        // Punktide animatsioon
        private async Task ShowPointsPopup(int points)
        {
            int version = ++popupVersion;

            PointsPopup.CancelAnimations();

            PointsPopup.Text = $"+{points}";

            PointsPopup.TextColor = points >= 200
                ? Color.FromArgb("#FFD166")
                : Color.FromArgb("#70E5FF");

            PointsPopup.Opacity = 1;
            PointsPopup.Scale = 0.8;
            PointsPopup.TranslationY = 10;
            PointsPopup.IsVisible = true;

            try
            {
                await Task.WhenAll(
                    PointsPopup.ScaleTo(1.3, 250),
                    PointsPopup.TranslateTo(0, -35, 500),
                    PointsPopup.FadeTo(0, 650)
                );
            }
            finally
            {
                if (version == popupVersion)
                {
                    PointsPopup.IsVisible = false;
                }
            }
        }

        // Punktid ja boonused
        private void UpdateStats()
        {
            ScoreLabel.Text = game.Score.ToString();
            BestLabel.Text = bestScore.ToString();

            BerriesLabel.Text =
                game.BerriesCollected.ToString();

            BonusLabel.Text =
                $"{game.BonusProgress} / 5";

            BonusProgress.Progress =
                game.BonusProgress / 5.0;

            if (game.BonusBerries.Count > 0)
            {
                BonusInfoLabel.Text =
                    $"🎁 Aardekirste väljakul: " +
                    $"{game.BonusBerries.Count} (+200)";
            }
            else
            {
                BonusInfoLabel.Text =
                    $"💎 Kogu veel " +
                    $"{5 - game.BonusProgress} " +
                    $"kristalli ja ava aarded!";
            }
        }

        // Uus mäng
        private void NewGameButton_Clicked(
            object sender, EventArgs e)
        {
            gameTimer.Stop();

            popupVersion++;
            PointsPopup.CancelAnimations();
            PointsPopup.IsVisible = false;

            game.Start();
            gameOverShown = false;

            PauseButton.Text = "⏸ Paus";

            StatusLabel.Text =
                "🐉 Uus maagiline seiklus algab!";

            UpdateStats();
            GameCanvas.Invalidate();

            if (pageVisible)
            {
                gameTimer.Start();
            }
        }

        // Paus ja jätkamine
        private void PauseButton_Clicked(
            object sender, EventArgs e)
        {
            if (game.IsGameOver)
                return;

            if (game.IsPaused)
            {
                game.Resume();

                PauseButton.Text = "⏸ Paus";
                StatusLabel.Text = "🐉 Seiklus jätkub!";

                if (pageVisible)
                {
                    gameTimer.Start();
                }
            }
            else
            {
                game.Pause();
                gameTimer.Stop();

                PauseButton.Text = "▶ Jätka";
                StatusLabel.Text = "⏸ Mäng on pausil";
            }

            GameCanvas.Invalidate();
        }

        // Draakoni juhtimine
        private void UpButton_Clicked(
            object sender, EventArgs e)
        {
            game.ChangeDirection(0, -1);
        }

        private void DownButton_Clicked(
            object sender, EventArgs e)
        {
            game.ChangeDirection(0, 1);
        }

        private void LeftButton_Clicked(
            object sender, EventArgs e)
        {
            game.ChangeDirection(-1, 0);
        }

        private void RightButton_Clicked(
            object sender, EventArgs e)
        {
            game.ChangeDirection(1, 0);
        }

        // Teema valimine
        private void ThemePicker_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (boardDrawable == null)
                return;

            currentTheme = ThemePicker.SelectedIndex switch
            {
                1 => SnakeTheme.Night(),
                2 => SnakeTheme.Candy(),
                _ => SnakeTheme.Grass()
            };

            ApplyTheme();
        }

        private void ApplyTheme()
        {
            // Säilitame fantaasiastiilis lehe tausta
            BackgroundColor = Color.FromArgb("#171329");

            boardDrawable.Theme = currentTheme;

            BoardBorder.BackgroundColor =
                Color.FromArgb("#261E3D");

            BoardBorder.Stroke =
                Color.FromArgb("#B28B4C");

            GameCanvas.BackgroundColor =
                Color.FromArgb("#243F3B");

            ThemeTitleLabel.TextColor =
                Color.FromArgb("#E9D5FF");

            SpeedTitleLabel.TextColor =
                Color.FromArgb("#E9D5FF");

            StatusLabel.TextColor =
                Color.FromArgb("#E9D5FF");

            GameCanvas.Invalidate();
        }

        // Kiiruse valimine
        private void SpeedPicker_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (gameTimer == null)
                return;

            bool wasRunning = gameTimer.IsRunning;

            gameTimer.Stop();

            gameTimer.Interval =
                SpeedPicker.SelectedIndex switch
                {
                    0 => TimeSpan.FromMilliseconds(300),
                    2 => TimeSpan.FromMilliseconds(110),
                    _ => TimeSpan.FromMilliseconds(200)
                };

            if (wasRunning &&
                !game.IsPaused &&
                !game.IsGameOver)
            {
                gameTimer.Start();
            }
        }
    }

    // Fantaasiametsa ja draakoni joonistamine
    public class SnakeBoardDrawable : IDrawable
    {
        public SnakeGameLogic? Game { get; set; }
        public SnakeTheme? Theme { get; set; }

        public void Draw(ICanvas canvas, RectF rect)
        {
            if (Game == null || Theme == null)
                return;

            float cell = Math.Min(
                rect.Width / Game.Width,
                rect.Height / Game.Height);

            if (cell <= 0)
                return;

            float ox =
                (rect.Width - cell * Game.Width) / 2;

            float oy =
                (rect.Height - cell * Game.Height) / 2;

            DrawForest(canvas, ox, oy, cell);

            // Kristall
            if (!Game.IsWon)
            {
                DrawCrystal(
                    canvas,
                    ox + Game.Food.X * cell,
                    oy + Game.Food.Y * cell,
                    cell);
            }

            // Aardekirstud
            foreach (var bonus in Game.BonusBerries)
            {
                DrawTreasure(
                    canvas,
                    ox + bonus.X * cell,
                    oy + bonus.Y * cell,
                    cell);
            }

            // Draakoni keha ja pea
            for (int i = Game.Snake.Body.Count - 1;
                 i >= 0; i--)
            {
                var part = Game.Snake.Body[i];

                float x = ox + part.X * cell;
                float y = oy + part.Y * cell;

                if (i == 0)
                {
                    DrawDragonHead(
                        canvas,
                        x,
                        y,
                        cell,
                        Game.Snake.DirectionX,
                        Game.Snake.DirectionY);
                }
                else
                {
                    DrawDragonBody(
                        canvas, x, y, cell, i);
                }
            }
        }

        // Maagiline metsapõrand
        private void DrawForest(
            ICanvas canvas,
            float ox,
            float oy,
            float cell)
        {
            if (Game == null)
                return;

            for (int y = 0; y < Game.Height; y++)
            {
                for (int x = 0; x < Game.Width; x++)
                {
                    float px = ox + x * cell;
                    float py = oy + y * cell;

                    canvas.FillColor =
                        Theme?.Name switch
                        {
                            "Night" => (x + y) % 2 == 0
                                ? Color.FromArgb("#172538")
                                : Color.FromArgb("#203149"),

                            "Candy" => (x + y) % 2 == 0
                                ? Color.FromArgb("#594070")
                                : Color.FromArgb("#694B81"),

                            _ => (x + y) % 2 == 0
                                ? Color.FromArgb("#243F3B")
                                : Color.FromArgb("#2B4942")
                        };

                    canvas.FillRectangle(
                        px, py, cell, cell);

                    // Maagilised sädemed
                    if ((x * 7 + y * 11) % 19 == 0)
                    {
                        canvas.FillColor =
                            Color.FromArgb("#7ADCC6");

                        canvas.FillEllipse(
                            px + cell * 0.45f,
                            py + cell * 0.45f,
                            cell * 0.1f,
                            cell * 0.1f);
                    }

                    // Lilled
                    if ((x * 13 + y * 5) % 31 == 0)
                    {
                        canvas.FillColor =
                            Color.FromArgb("#9B79D9");

                        canvas.FillEllipse(
                            px + cell * 0.3f,
                            py + cell * 0.3f,
                            cell * 0.2f,
                            cell * 0.2f);
                    }
                }
            }
        }

        // Maagiline kristall
        private void DrawCrystal(
            ICanvas canvas,
            float x,
            float y,
            float size)
        {
            canvas.FillColor =
                Color.FromArgb("#3BBED6");

            var crystal = new PathF();

            crystal.MoveTo(
                x + size * 0.5f,
                y + size * 0.05f);

            crystal.LineTo(
                x + size * 0.85f,
                y + size * 0.4f);

            crystal.LineTo(
                x + size * 0.5f,
                y + size * 0.95f);

            crystal.LineTo(
                x + size * 0.15f,
                y + size * 0.4f);

            crystal.Close();
            canvas.FillPath(crystal);

            canvas.FillColor =
                Color.FromArgb("#B6F5FF");

            var shine = new PathF();

            shine.MoveTo(
                x + size * 0.5f,
                y + size * 0.1f);

            shine.LineTo(
                x + size * 0.5f,
                y + size * 0.85f);

            shine.LineTo(
                x + size * 0.2f,
                y + size * 0.4f);

            shine.Close();
            canvas.FillPath(shine);
        }

        // Kuldne aardekirst
        private void DrawTreasure(
            ICanvas canvas,
            float x,
            float y,
            float size)
        {
            canvas.FillColor =
                Color.FromArgb("#FACC15");

            canvas.FillRoundedRectangle(
                x + size * 0.1f,
                y + size * 0.25f,
                size * 0.8f,
                size * 0.6f,
                size * 0.12f);

            canvas.FillColor =
                Color.FromArgb("#8B4513");

            canvas.FillRoundedRectangle(
                x + size * 0.17f,
                y + size * 0.35f,
                size * 0.66f,
                size * 0.42f,
                size * 0.08f);

            canvas.FillColor =
                Color.FromArgb("#FFE88B");

            canvas.FillRectangle(
                x + size * 0.43f,
                y + size * 0.4f,
                size * 0.14f,
                size * 0.28f);

            canvas.FillColor =
                Color.FromArgb("#FFF7C2");

            canvas.FillEllipse(
                x + size * 0.46f,
                y + size * 0.48f,
                size * 0.08f,
                size * 0.08f);
        }

        // Draakoni keha
        private void DrawDragonBody(
            ICanvas canvas,
            float x,
            float y,
            float size,
            int index)
        {
            // Seljaogad
            if (index % 2 == 0)
            {
                canvas.FillColor =
                    Color.FromArgb("#1B7D4D");

                var spike = new PathF();

                spike.MoveTo(
                    x + size * 0.3f,
                    y + size * 0.2f);

                spike.LineTo(
                    x + size * 0.5f,
                    y);

                spike.LineTo(
                    x + size * 0.7f,
                    y + size * 0.2f);

                spike.Close();
                canvas.FillPath(spike);
            }

            // Roheline keha
            canvas.FillColor =
                Color.FromArgb("#43B96C");

            canvas.FillEllipse(
                x + size * 0.02f,
                y + size * 0.02f,
                size * 0.96f,
                size * 0.96f);

            // Kuldne kõht
            canvas.FillColor =
                Color.FromArgb("#F6D47B");

            canvas.FillEllipse(
                x + size * 0.28f,
                y + size * 0.37f,
                size * 0.44f,
                size * 0.5f);

            // Läige
            canvas.FillColor =
                Color.FromArgb("#A1F3B5");

            canvas.FillEllipse(
                x + size * 0.16f,
                y + size * 0.12f,
                size * 0.24f,
                size * 0.16f);
        }

        // Draakoni pea
        private void DrawDragonHead(
            ICanvas canvas,
            float x,
            float y,
            float size,
            int dx,
            int dy)
        {
            canvas.SaveState();

            canvas.Translate(
                x + size / 2,
                y + size / 2);

            float angle = 0;

            if (dx == 1)
                angle = 90;
            else if (dy == 1)
                angle = 180;
            else if (dx == -1)
                angle = 270;

            canvas.Rotate(angle);

            canvas.Translate(
                -size / 2,
                -size / 2);

            // Sarved
            canvas.FillColor =
                Color.FromArgb("#F7D78A");

            var leftHorn = new PathF();

            leftHorn.MoveTo(
                size * 0.15f,
                size * 0.35f);

            leftHorn.LineTo(
                size * 0.03f,
                size * 0.02f);

            leftHorn.LineTo(
                size * 0.4f,
                size * 0.2f);

            leftHorn.Close();
            canvas.FillPath(leftHorn);

            var rightHorn = new PathF();

            rightHorn.MoveTo(
                size * 0.85f,
                size * 0.35f);

            rightHorn.LineTo(
                size * 0.97f,
                size * 0.02f);

            rightHorn.LineTo(
                size * 0.6f,
                size * 0.2f);

            rightHorn.Close();
            canvas.FillPath(rightHorn);

            // Roheline pea
            canvas.FillColor =
                Color.FromArgb("#70DD86");

            canvas.FillEllipse(
                -size * 0.05f,
                size * 0.05f,
                size * 1.1f,
                size * 0.95f);

            // Kuldne koon
            canvas.FillColor =
                Color.FromArgb("#F6D47B");

            canvas.FillRoundedRectangle(
                size * 0.22f,
                size * 0.58f,
                size * 0.56f,
                size * 0.32f,
                size * 0.15f);

            // Silmad
            canvas.FillColor = Colors.White;

            canvas.FillEllipse(
                size * 0.15f,
                size * 0.3f,
                size * 0.27f,
                size * 0.3f);

            canvas.FillEllipse(
                size * 0.58f,
                size * 0.3f,
                size * 0.27f,
                size * 0.3f);

            // Pupillid
            canvas.FillColor =
                Color.FromArgb("#243447");

            canvas.FillEllipse(
                size * 0.25f,
                size * 0.38f,
                size * 0.12f,
                size * 0.15f);

            canvas.FillEllipse(
                size * 0.63f,
                size * 0.38f,
                size * 0.12f,
                size * 0.15f);

            // Ninasõõrmed
            canvas.FillColor =
                Color.FromArgb("#388B50");

            canvas.FillEllipse(
                size * 0.37f,
                size * 0.69f,
                size * 0.06f,
                size * 0.05f);

            canvas.FillEllipse(
                size * 0.57f,
                size * 0.69f,
                size * 0.06f,
                size * 0.05f);

            // Naeratus
            canvas.StrokeColor =
                Color.FromArgb("#357A46");

            canvas.StrokeSize = size * 0.035f;

            canvas.DrawArc(
                size * 0.32f,
                size * 0.65f,
                size * 0.68f,
                size * 0.86f,
                0,
                180,
                false,
                false);

            canvas.RestoreState();
        }
    }
}

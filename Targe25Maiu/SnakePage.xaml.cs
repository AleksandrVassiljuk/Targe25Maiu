
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using Targe25Maiu.SnakeGame;

namespace Targe25Maiu
{
    public partial class SnakePage : ContentPage
    {
        private readonly SnakeGameLogic game;
        private readonly IDispatcherTimer gameTimer;
        private readonly DragonBoardDrawable board;
        private readonly Random random = new Random();

        private DragonWorld world = DragonWorld.Forest;
        private DragonDifficulty difficulty = DragonDifficulty.Normal;

        private readonly List<DragonQuest> quests =
            new List<DragonQuest>();

        private readonly int[] worldCrystals = new int[3];

        private int hardCrystals;
        private int fastCrystals;
        private int bestScore;

        private bool pageVisible;
        private bool gameOverShown;

        public SnakePage()
        {
            InitializeComponent();

            game = new SnakeGameLogic();

            board = new DragonBoardDrawable
            {
                Game = game,
                World = world
            };

            GameCanvas.Drawable = board;

            gameTimer = Dispatcher.CreateTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameTimer_Tick;

            bestScore = Preferences.Default.Get(
                "DragonSnakeBestScore", 0);

            SelectWorld(DragonWorld.Forest);
            SelectDifficulty(DragonDifficulty.Normal);

            GenerateQuests();
            UpdateStats();

            GameCanvas.Invalidate();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            pageVisible = true;

            if (!game.IsPaused && !game.IsGameOver)
            {
                gameTimer.Start();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            pageVisible = false;
            gameTimer.Stop();

            PointsPopup.IsVisible = false;

            if (!game.IsGameOver)
            {
                game.Pause();
                PauseButton.Text = "▶ Jätka";
            }
        }

        // MENÜÜ
        private void MenuButton_Clicked(
            object sender, EventArgs e)
        {
            SettingsPanel.IsVisible =
                !SettingsPanel.IsVisible;

            MenuButton.Text =
                SettingsPanel.IsVisible ? "✕" : "☰";
        }

        // MAAILMADE VALIMINE
        private void ForestCard_Tapped(
            object sender, TappedEventArgs e)
        {
            SelectWorld(DragonWorld.Forest);
        }

        private void NightCard_Tapped(
            object sender, TappedEventArgs e)
        {
            SelectWorld(DragonWorld.Night);
        }

        private void CandyCard_Tapped(
            object sender, TappedEventArgs e)
        {
            SelectWorld(DragonWorld.Candy);
        }

        // RASKUSASTMETE VALIMINE
        private void EasyCard_Tapped(
            object sender, TappedEventArgs e)
        {
            SelectDifficulty(DragonDifficulty.Easy);
        }

        private void NormalCard_Tapped(
            object sender, TappedEventArgs e)
        {
            SelectDifficulty(DragonDifficulty.Normal);
        }

        private void HardCard_Tapped(
            object sender, TappedEventArgs e)
        {
            SelectDifficulty(DragonDifficulty.Hard);
        }

        private void SelectWorld(DragonWorld selected)
        {
            world = selected;
            board.World = selected;

            StyleCard(
                ForestCard,
                selected == DragonWorld.Forest,
                "#225D42",
                "#70E8A2");

            StyleCard(
                NightCard,
                selected == DragonWorld.Night,
                "#343D79",
                "#9CA9FF");

            StyleCard(
                CandyCard,
                selected == DragonWorld.Candy,
                "#793B70",
                "#FFA1E5");

            ForestSelected.IsVisible =
                selected == DragonWorld.Forest;

            NightSelected.IsVisible =
                selected == DragonWorld.Night;

            CandySelected.IsVisible =
                selected == DragonWorld.Candy;

            switch (selected)
            {
                case DragonWorld.Forest:
                    WorldDescriptionLabel.Text =
                        "🌲 Maagiline roheline mets.";
                    break;

                case DragonWorld.Night:
                    WorldDescriptionLabel.Text =
                        "🌙 Maagiline öö ja kristallid.";
                    break;

                case DragonWorld.Candy:
                    WorldDescriptionLabel.Text =
                        "🍬 Värviline kommimaa.";
                    break;
            }

            GameCanvas.Invalidate();
        }

        private void SelectDifficulty(
            DragonDifficulty selected)
        {
            difficulty = selected;

            game.SetDifficulty(selected);

            StyleCard(
                EasyCard,
                selected == DragonDifficulty.Easy,
                "#423457",
                "#4ADE80");

            StyleCard(
                NormalCard,
                selected == DragonDifficulty.Normal,
                "#423457",
                "#FACC15");

            StyleCard(
                HardCard,
                selected == DragonDifficulty.Hard,
                "#423457",
                "#FB7185");

            EasySelected.IsVisible =
                selected == DragonDifficulty.Easy;

            NormalSelected.IsVisible =
                selected == DragonDifficulty.Normal;

            HardSelected.IsVisible =
                selected == DragonDifficulty.Hard;

            bool wasRunning = gameTimer.IsRunning;

            gameTimer.Stop();

            switch (selected)
            {
                case DragonDifficulty.Easy:
                    gameTimer.Interval =
                        TimeSpan.FromMilliseconds(300);
                    break;

                case DragonDifficulty.Normal:
                    gameTimer.Interval =
                        TimeSpan.FromMilliseconds(200);
                    break;

                case DragonDifficulty.Hard:
                    gameTimer.Interval =
                        TimeSpan.FromMilliseconds(110);
                    break;
            }

            if (wasRunning &&
                !game.IsPaused &&
                !game.IsGameOver)
            {
                gameTimer.Start();
            }
        }

        private void StyleCard(
            Border card,
            bool selected,
            string background,
            string stroke)
        {
            card.BackgroundColor = Color.FromArgb(
                selected ? background : "#322744");

            card.Stroke = new SolidColorBrush(
                Color.FromArgb(
                    selected ? stroke : "#65547F"));

            card.StrokeThickness = selected ? 2 : 1;
        }

        // QUESTIDE LOOMINE
        private void GenerateQuests()
        {
            quests.Clear();
            QuestListLayout.Children.Clear();

            Array.Clear(
                worldCrystals,
                0,
                worldCrystals.Length);

            hardCrystals = 0;
            fastCrystals = 0;

            List<DragonQuest> pool = new List<DragonQuest>
            {
                new DragonQuest(
                    "💎", "First Crystal",
                    "Kogu 1 kristall",
                    QuestKind.Crystals, 1),

                new DragonQuest(
                    "🐉", "Crystal Hunter",
                    "Kogu 5 kristalli",
                    QuestKind.Crystals, 5),

                new DragonQuest(
                    "👑", "Crystal Master",
                    "Kogu 12 kristalli",
                    QuestKind.Crystals, 12),

                new DragonQuest(
                    "⭐", "Rising Dragon",
                    "Saavuta 1000 punkti",
                    QuestKind.Score, 1000),

                new DragonQuest(
                    "🏆", "Dragon Legend",
                    "Saavuta 3000 punkti",
                    QuestKind.Score, 3000),

                new DragonQuest(
                    "🎁", "Bonus Hunter",
                    "Korja 2 boonuskristalli",
                    QuestKind.Bonuses, 2),

                new DragonQuest(
                    "🌲", "Forest Guardian",
                    "Kogu metsamaailmas 5 kristalli",
                    QuestKind.Forest, 5),

                new DragonQuest(
                    "🌙", "Night Explorer",
                    "Kogu öömaailmas 5 kristalli",
                    QuestKind.Night, 5),

                new DragonQuest(
                    "🍬", "Candy Explorer",
                    "Kogu kommimaal 5 kristalli",
                    QuestKind.Candy, 5),

                new DragonQuest(
                    "🔥", "Hard Challenge",
                    "Kogu Hard-raskusega 4 kristalli",
                    QuestKind.Hard, 4),

                new DragonQuest(
                    "⚡", "Speed Master",
                    "Kogu Normal või Hard raskusega 8 kristalli",
                    QuestKind.Fast, 8),

                new DragonQuest(
                    "✨", "Treasure Seeker",
                    "Saavuta 2000 punkti",
                    QuestKind.Score, 2000)
            };

            // Segame questid juhuslikult.
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                DragonQuest temp = pool[i];
                pool[i] = pool[j];
                pool[j] = temp;
            }

            // Valime viis questi.
            foreach (DragonQuest quest in pool.Take(5))
            {
                CreateQuestCard(quest);
                quests.Add(quest);
            }

            UpdateQuests();
        }

        // QUESTI KAART
        private void CreateQuestCard(DragonQuest quest)
        {
            quest.TitleLabel = new Label
            {
                Text = quest.Icon + " " + quest.Title,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White,
                LineBreakMode = LineBreakMode.WordWrap
            };

            quest.StateLabel = new Label
            {
                Text = "IN PROGRESS",
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#D8C4F0"),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            quest.ProgressLabel = new Label
            {
                Text = "0 / " + quest.Target,
                FontSize = 12,
                TextColor = Color.FromArgb("#D8C4F0")
            };

            quest.ProgressBar = new ProgressBar
            {
                Progress = 0,
                ProgressColor = Color.FromArgb("#F6C65B"),
                BackgroundColor = Color.FromArgb("#514366"),
                HorizontalOptions = LayoutOptions.Fill
            };

            Grid heading = new Grid
            {
                ColumnSpacing = 10
            };

            heading.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = GridLength.Star
                });

            heading.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(125)
                });

            heading.Children.Add(quest.TitleLabel);
            Grid.SetColumn(quest.TitleLabel, 0);

            heading.Children.Add(quest.StateLabel);
            Grid.SetColumn(quest.StateLabel, 1);

            Label description = new Label
            {
                Text = quest.Description,
                FontSize = 12,
                TextColor = Color.FromArgb("#D8C4F0"),
                LineBreakMode = LineBreakMode.WordWrap
            };

            VerticalStackLayout content =
                new VerticalStackLayout
                {
                    Spacing = 9,
                    HorizontalOptions = LayoutOptions.Fill
                };

            content.Children.Add(heading);
            content.Children.Add(description);
            content.Children.Add(quest.ProgressBar);
            content.Children.Add(quest.ProgressLabel);

            quest.Card = new Border
            {
                BackgroundColor =
                    Color.FromArgb("#322744"),

                Stroke = new SolidColorBrush(
                    Color.FromArgb("#65547F")),

                StrokeThickness = 1,

                StrokeShape =
                    new Microsoft.Maui.Controls.Shapes.RoundRectangle
                    {
                        CornerRadius = new CornerRadius(12)
                    },

                Padding = new Thickness(14),

                HorizontalOptions = LayoutOptions.Fill,

                Content = content
            };

            QuestListLayout.Children.Add(quest.Card);
        }

        // QUESTIDE PROGRESS
        private void UpdateQuests()
        {
            int completedCount = 0;

            foreach (DragonQuest quest in quests)
            {
                int progress = 0;

                switch (quest.Kind)
                {
                    case QuestKind.Crystals:
                        progress = game.BerriesCollected;
                        break;

                    case QuestKind.Score:
                        progress = game.Score;
                        break;

                    case QuestKind.Bonuses:
                        progress = game.BonusesCollected;
                        break;

                    case QuestKind.Forest:
                        progress = worldCrystals[0];
                        break;

                    case QuestKind.Night:
                        progress = worldCrystals[1];
                        break;

                    case QuestKind.Candy:
                        progress = worldCrystals[2];
                        break;

                    case QuestKind.Hard:
                        progress = hardCrystals;
                        break;

                    case QuestKind.Fast:
                        progress = fastCrystals;
                        break;
                }

                progress = Math.Min(
                    progress,
                    quest.Target);

                quest.ProgressLabel.Text =
                    progress + " / " + quest.Target;

                quest.ProgressBar.Progress =
                    (double)progress / quest.Target;

                if (progress >= quest.Target &&
                    !quest.Completed)
                {
                    quest.Completed = true;

                    quest.Card.BackgroundColor =
                        Color.FromArgb("#183D30");

                    quest.Card.Stroke =
                        new SolidColorBrush(
                            Color.FromArgb("#39AD74"));

                    quest.StateLabel.Text =
                        "✓ COMPLETED";

                    quest.StateLabel.TextColor =
                        Color.FromArgb("#8AF0B2");

                    quest.ProgressBar.ProgressColor =
                        Color.FromArgb("#4ADE80");
                }

                if (quest.Completed)
                {
                    completedCount++;
                }
            }

            QuestCountLabel.Text =
                completedCount + " / " + quests.Count;
        }

        // MÄNGU TAIMER
        private void GameTimer_Tick(
            object sender, EventArgs e)
        {
            if (!pageVisible ||
                game.IsPaused ||
                game.IsGameOver)
            {
                return;
            }

            int previousCrystals =
                game.BerriesCollected;

            int points = game.Update();

            int collected =
                game.BerriesCollected - previousCrystals;

            if (collected > 0)
            {
                worldCrystals[(int)world] += collected;

                if (difficulty == DragonDifficulty.Hard)
                {
                    hardCrystals += collected;
                }

                if (difficulty != DragonDifficulty.Easy)
                {
                    fastCrystals += collected;
                }
            }

            if (game.Score > bestScore)
            {
                bestScore = game.Score;

                Preferences.Default.Set(
                    "DragonSnakeBestScore",
                    bestScore);
            }

            if (points > 0 || game.IsGameOver)
            {
                UpdateStats();
                UpdateQuests();
            }

            GameCanvas.Invalidate();

            if (points > 0 && !game.IsGameOver)
            {
                ShowPoints(points);
            }

            if (game.IsGameOver && !gameOverShown)
            {
                gameOverShown = true;
                gameTimer.Stop();

                PointsPopup.IsVisible = false;

                StatusLabel.Text = game.IsWon
                    ? "🏆 Võitsid mängu!"
                    : "💀 Mäng läbi!";

                GameOverScoreLabel.Text =
                    "Punktid: " + game.Score +
                    "\nKristallid: " + game.BerriesCollected +
                    "\nRekord: " + bestScore;

                GameOverOverlay.IsVisible = true;
            }
        }

        // PUNKTIDE JA BOONUSE INFO
        private void UpdateStats()
        {
            ScoreLabel.Text = game.Score.ToString();
            BestLabel.Text = bestScore.ToString();
            BerriesLabel.Text =
                game.BerriesCollected.ToString();

            BonusProgress.Progress =
                game.BonusProgress / 5.0;

            int remaining = 5 - game.BonusProgress;

            BonusLabel.Text =
                "🌟 Järgmine boonus";

            BonusInfoLabel.Text =
                "Veel " + remaining +
                " kristalli ja saad 2 boonuskristalli!";
        }

        // +100 / +200 TEADE
        private async void ShowPoints(int points)
        {
            PointsPopup.CancelAnimations();

            PointsPopup.Text = "+" + points;
            PointsPopup.TextColor = Color.FromArgb(
                points >= 200 ? "#FFD166" : "#70E5FF");

            PointsPopup.Opacity = 1;
            PointsPopup.Scale = 1;
            PointsPopup.TranslationY = 0;
            PointsPopup.IsVisible = true;

            await PointsPopup.TranslateTo(0, -35, 250);
            await PointsPopup.FadeTo(0, 250);

            PointsPopup.IsVisible = false;
        }

        // UUS MÄNG
        private void NewGameButton_Clicked(
            object sender, EventArgs e)
        {
            gameTimer.Stop();

            game.SetDifficulty(difficulty);
            game.Start();

            gameOverShown = false;

            GameOverOverlay.IsVisible = false;
            PointsPopup.IsVisible = false;

            PauseButton.Text = "⏸ Paus";
            StatusLabel.Text = "🐉 Mäng käib!";

            GenerateQuests();
            UpdateStats();
            GameCanvas.Invalidate();

            if (pageVisible)
            {
                gameTimer.Start();
            }
        }

        // PAUS / JÄTKA
        private void PauseButton_Clicked(
            object sender, EventArgs e)
        {
            if (game.IsGameOver)
                return;

            if (game.IsPaused)
            {
                game.Resume();

                PauseButton.Text = "⏸ Paus";
                StatusLabel.Text = "🐉 Mäng käib!";

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
        }

        // JUHTNUPUD
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
    }

    // QUESTIDE TÜÜBID
    public enum QuestKind
    {
        Crystals,
        Score,
        Bonuses,
        Forest,
        Night,
        Candy,
        Hard,
        Fast
    }

    // QUESTI ANDMED
    public class DragonQuest
    {
        public string Icon { get; }
        public string Title { get; }
        public string Description { get; }
        public QuestKind Kind { get; }
        public int Target { get; }

        public bool Completed { get; set; }

        public Border Card { get; set; } = null!;
        public Label TitleLabel { get; set; } = null!;
        public Label StateLabel { get; set; } = null!;
        public Label ProgressLabel { get; set; } = null!;
        public ProgressBar ProgressBar { get; set; } = null!;

        public DragonQuest(
            string icon,
            string title,
            string description,
            QuestKind kind,
            int target)
        {
            Icon = icon;
            Title = title;
            Description = description;
            Kind = kind;
            Target = target;
        }
    }

    // DRAAKONI MÄNGUVÄLJAK
    public class DragonBoardDrawable : IDrawable
    {
        public SnakeGameLogic? Game { get; set; }

        public DragonWorld World { get; set; } =
            DragonWorld.Forest;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Game == null)
                return;

            float cell = Math.Min(
                dirtyRect.Width / Game.Width,
                dirtyRect.Height / Game.Height);

            float boardWidth = cell * Game.Width;
            float boardHeight = cell * Game.Height;

            float offsetX =
                (dirtyRect.Width - boardWidth) / 2;

            float offsetY =
                (dirtyRect.Height - boardHeight) / 2;

            DrawBackground(
                canvas,
                Game,
                cell,
                offsetX,
                offsetY);

            // KIVID
            foreach (SnakePoint rock in Game.Rocks)
            {
                float x = offsetX + rock.X * cell;
                float y = offsetY + rock.Y * cell;

                canvas.FillColor =
                    Color.FromArgb("#77748A");

                canvas.FillRoundedRectangle(
                    x + 2,
                    y + 2,
                    cell - 4,
                    cell - 4,
                    5);

                canvas.FillColor =
                    Color.FromArgb("#B8B3C5");

                canvas.FillCircle(
                    x + cell * 0.36f,
                    y + cell * 0.34f,
                    cell * 0.11f);
            }

            // TAVALINE KRISTALL
            if (Game.Food.X >= 0 &&
                Game.Food.Y >= 0)
            {
                DrawCrystal(
                    canvas,
                    offsetX + Game.Food.X * cell,
                    offsetY + Game.Food.Y * cell,
                    cell,
                    false);
            }

            // BOONUSKRISTALLID
            foreach (SnakePoint bonus in Game.BonusBerries)
            {
                DrawCrystal(
                    canvas,
                    offsetX + bonus.X * cell,
                    offsetY + bonus.Y * cell,
                    cell,
                    true);
            }

            // DRAAKONI KEHA
            for (int i = Game.Snake.Body.Count - 1;
                 i >= 1;
                 i--)
            {
                SnakePoint part = Game.Snake.Body[i];

                float x = offsetX + part.X * cell;
                float y = offsetY + part.Y * cell;

                DrawBody(canvas, x, y, cell, i);
            }

            // DRAAKONI PEA
            if (Game.Snake.Body.Count > 0)
            {
                SnakePoint head = Game.Snake.Body[0];

                DrawHead(
                    canvas,
                    offsetX + head.X * cell,
                    offsetY + head.Y * cell,
                    cell,
                    Game.Snake.DirectionX,
                    Game.Snake.DirectionY);
            }
        }

        // MAAILMA TAUST
        private void DrawBackground(
            ICanvas canvas,
            SnakeGameLogic game,
            float cell,
            float offsetX,
            float offsetY)
        {
            Color first;
            Color second;

            switch (World)
            {
                case DragonWorld.Night:
                    first = Color.FromArgb("#172538");
                    second = Color.FromArgb("#203149");
                    break;

                case DragonWorld.Candy:
                    first = Color.FromArgb("#594070");
                    second = Color.FromArgb("#694B81");
                    break;

                default:
                    first = Color.FromArgb("#243F3B");
                    second = Color.FromArgb("#2B4942");
                    break;
            }

            for (int y = 0; y < game.Height; y++)
            {
                for (int x = 0; x < game.Width; x++)
                {
                    canvas.FillColor =
                        (x + y) % 2 == 0 ? first : second;

                    canvas.FillRectangle(
                        offsetX + x * cell,
                        offsetY + y * cell,
                        cell,
                        cell);
                }
            }
        }

        // KRISTALLI JOONISTAMINE
        private void DrawCrystal(
            ICanvas canvas,
            float x,
            float y,
            float cell,
            bool bonus)
        {
            float cx = x + cell / 2;
            float cy = y + cell / 2;

            canvas.FillColor = bonus
                ? Color.FromArgb("#FFD166")
                : Color.FromArgb("#70E5FF");

            PathF crystal = new PathF();

            crystal.MoveTo(cx, y + cell * 0.12f);
            crystal.LineTo(x + cell * 0.82f, cy);
            crystal.LineTo(cx, y + cell * 0.88f);
            crystal.LineTo(x + cell * 0.18f, cy);
            crystal.Close();

            canvas.FillPath(crystal);

            canvas.StrokeColor = bonus
                ? Color.FromArgb("#FFF2AE")
                : Color.FromArgb("#D6FAFF");

            canvas.StrokeSize = 1.5f;
            canvas.DrawPath(crystal);

            canvas.FillColor = Colors.White;

            canvas.FillCircle(
                cx - cell * 0.09f,
                cy - cell * 0.14f,
                cell * 0.045f);
        }

        // DRAAKONI KEHA JOONISTAMINE
        private void DrawBody(
            ICanvas canvas,
            float x,
            float y,
            float cell,
            int index)
        {
            canvas.FillColor =
                index % 2 == 0
                    ? Color.FromArgb("#43B96C")
                    : Color.FromArgb("#35A65C");

            canvas.FillRoundedRectangle(
                x + 1,
                y + 1,
                cell - 2,
                cell - 2,
                cell * 0.30f);

            canvas.FillColor =
                Color.FromArgb("#7BE39B");

            canvas.FillCircle(
                x + cell * 0.35f,
                y + cell * 0.30f,
                cell * 0.12f);
        }

        // DRAAKONI PEA JOONISTAMINE
        private void DrawHead(
            ICanvas canvas,
            float x,
            float y,
            float cell,
            int directionX,
            int directionY)
        {
            float cx = x + cell / 2;
            float cy = y + cell / 2;

            // Väikesed sarved
            canvas.FillColor =
                Color.FromArgb("#F6D48D");

            canvas.FillCircle(
                x + cell * 0.24f,
                y + cell * 0.20f,
                cell * 0.13f);

            canvas.FillCircle(
                x + cell * 0.76f,
                y + cell * 0.20f,
                cell * 0.13f);

            // Pea
            canvas.FillColor =
                Color.FromArgb("#55CE76");

            canvas.FillRoundedRectangle(
                x + 1,
                y + 1,
                cell - 2,
                cell - 2,
                cell * 0.35f);

            // Silmad
            canvas.FillColor = Colors.White;

            canvas.FillCircle(
                x + cell * 0.32f,
                y + cell * 0.38f,
                cell * 0.16f);

            canvas.FillCircle(
                x + cell * 0.68f,
                y + cell * 0.38f,
                cell * 0.16f);

            canvas.FillColor =
                Color.FromArgb("#182A24");

            canvas.FillCircle(
                x + cell * 0.35f,
                y + cell * 0.40f,
                cell * 0.075f);

            canvas.FillCircle(
                x + cell * 0.71f,
                y + cell * 0.40f,
                cell * 0.075f);

            // Nina
            canvas.FillColor =
                Color.FromArgb("#91EBA4");

            canvas.FillRoundedRectangle(
                x + cell * 0.28f,
                y + cell * 0.62f,
                cell * 0.44f,
                cell * 0.22f,
                cell * 0.10f);

            // Ninasõõrmed
            canvas.FillColor =
                Color.FromArgb("#2E8550");

            canvas.FillCircle(
                x + cell * 0.42f,
                y + cell * 0.72f,
                cell * 0.035f);

            canvas.FillCircle(
                x + cell * 0.60f,
                y + cell * 0.72f,
                cell * 0.035f);
        }
    }
}

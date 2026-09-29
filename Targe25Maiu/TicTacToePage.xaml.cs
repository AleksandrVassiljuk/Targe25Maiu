namespace Targe25Maiu;

public partial class TicTacToePage : ContentPage
{
    private string currentPlayer = "X";

    private int xWins = 0;
    private int oWins = 0;
    private int draws = 0;
    private int round = 1;
    private int moves = 0;

    private bool gameOver = false;
    private bool botMode = false;
    private bool matchFinished = false;

    private readonly Random random = new();

    private const int WinsNeeded = 2;

    public TicTacToePage()
    {
        InitializeComponent();

        GameModePicker.SelectedIndex = 0;

        UpdateGameInfo();
        UpdateStatistics();
        UpdateMatchScore();
    }

    private async void GameButton_Clicked(object sender, EventArgs e)
    {
        if (gameOver || matchFinished)
            return;

        if (botMode && currentPlayer == "O")
            return;

        if (sender is not Button button)
            return;

        if (!string.IsNullOrEmpty(button.Text))
        {
            await DisplayAlert(
                "Ruut on hõivatud",
                "Vali teine ruut.",
                "OK");

            return;
        }

        MakeMove(button, currentPlayer);

        if (await CheckGameEnd())
            return;

        SwitchPlayer();

        if (botMode && currentPlayer == "O")
            await BotMove();
    }

    private void MakeMove(Button button, string player)
    {
        button.Text = player;

        if (player == "X")
            button.TextColor = Colors.Red;
        else
            button.TextColor = Colors.Blue;

        moves++;

        UpdateGameInfo();
    }

    private void SwitchPlayer()
    {
        currentPlayer =
            currentPlayer == "X" ? "O" : "X";

        UpdateGameInfo();
    }

    private async Task BotMove()
    {
        await Task.Delay(500);

        if (gameOver || matchFinished)
            return;

        Button? selectedButton = FindWinningMove("O");

        selectedButton ??= FindWinningMove("X");

        if (selectedButton == null &&
            string.IsNullOrEmpty(Button11.Text))
        {
            selectedButton = Button11;
        }

        if (selectedButton == null)
        {
            Button[] corners =
            {
                Button00,
                Button02,
                Button20,
                Button22
            };

            List<Button> freeCorners =
                corners
                    .Where(x => string.IsNullOrEmpty(x.Text))
                    .ToList();

            if (freeCorners.Count > 0)
            {
                selectedButton =
                    freeCorners[random.Next(freeCorners.Count)];
            }
        }

        if (selectedButton == null)
        {
            List<Button> freeButtons =
                GetButtons()
                    .Where(x => string.IsNullOrEmpty(x.Text))
                    .ToList();

            if (freeButtons.Count == 0)
                return;

            selectedButton =
                freeButtons[random.Next(freeButtons.Count)];
        }

        MakeMove(selectedButton, "O");

        if (await CheckGameEnd())
            return;

        currentPlayer = "X";

        UpdateGameInfo();
    }

    private Button? FindWinningMove(string player)
    {
        foreach (Button[] line in GetWinningLines())
        {
            int playerCount =
                line.Count(x => x.Text == player);

            int emptyCount =
                line.Count(x => string.IsNullOrEmpty(x.Text));

            if (playerCount == 2 && emptyCount == 1)
            {
                return line.First(
                    x => string.IsNullOrEmpty(x.Text));
            }
        }

        return null;
    }

    private async Task<bool> CheckGameEnd()
    {
        foreach (Button[] line in GetWinningLines())
        {
            if (!string.IsNullOrEmpty(line[0].Text) &&
                line[0].Text == line[1].Text &&
                line[1].Text == line[2].Text)
            {
                gameOver = true;

                string winner = line[0].Text;

                HighlightWinningLine(line);

                if (winner == "X")
                {
                    xWins++;
                    LastWinnerLabel.Text =
                        "Viimane võitja: Mängija X";
                }
                else
                {
                    oWins++;

                    LastWinnerLabel.Text =
                        botMode
                            ? "Viimane võitja: Bot"
                            : "Viimane võitja: Mängija O";
                }

                UpdateStatistics();
                UpdateMatchScore();

                string winnerName =
                    GetWinnerName(winner);

                PlayerLabel.Text =
                    $"{winnerName} võitis vooru";

                if (await CheckBestOfThree())
                    return true;

                await DisplayAlert(
                    "Voor võidetud",
                    $"{winnerName} võitis vooru.\n\n" +
                    $"Matši seis: {xWins} : {oWins}",
                    "OK");

                return true;
            }
        }

        if (moves >= 9)
        {
            gameOver = true;
            draws++;

            LastWinnerLabel.Text =
                "Viimane mäng: viik";

            PlayerLabel.Text =
                "Voor jäi viiki";

            UpdateStatistics();

            await DisplayAlert(
                "Viik",
                "Voor jäi viiki.",
                "OK");

            return true;
        }

        return false;
    }

    private async Task<bool> CheckBestOfThree()
    {
        if (xWins >= WinsNeeded)
        {
            matchFinished = true;

            await ShowFireworks("Mängija X");

            await DisplayAlert(
                "Matš võidetud",
                $"Mängija X võitis matši {xWins} : {oWins}",
                "OK");

            return true;
        }

        if (oWins >= WinsNeeded)
        {
            matchFinished = true;

            string winner =
                botMode ? "Bot" : "Mängija O";

            await ShowFireworks(winner);

            await DisplayAlert(
                "Matš võidetud",
                $"{winner} võitis matši {oWins} : {xWins}",
                "OK");

            return true;
        }

        return false;
    }

    private async Task ShowFireworks(string winner)
    {
        ChampionLabel.Text =
            $"{winner} võitis";

        FireworksLayer.IsVisible = true;
        FireworksLayer.Opacity = 0;

        Firework1.Scale = 0.2;
        Firework2.Scale = 0.2;
        Firework3.Scale = 0.2;
        Firework4.Scale = 0.2;
        Firework5.Scale = 0.2;
        Firework6.Scale = 0.2;
        Firework7.Scale = 0.2;
        Firework8.Scale = 0.2;

        await FireworksLayer.FadeTo(1, 300);

        await Task.WhenAll(
            Firework1.ScaleTo(1.4, 600),
            Firework2.ScaleTo(1.5, 700),
            Firework3.ScaleTo(1.4, 650),
            Firework4.ScaleTo(1.3, 600),
            Firework5.ScaleTo(1.3, 700),
            Firework6.ScaleTo(1.4, 650),
            Firework7.ScaleTo(1.5, 700),
            Firework8.ScaleTo(1.4, 600)
        );

        for (int i = 0; i < 3; i++)
        {
            await FireworksLayer.FadeTo(0.7, 150);
            await FireworksLayer.FadeTo(1, 150);
        }

        await Task.Delay(1500);

        await FireworksLayer.FadeTo(0, 400);

        FireworksLayer.IsVisible = false;
    }

    private string GetWinnerName(string winner)
    {
        if (winner == "X")
            return "Mängija X";

        if (botMode)
            return "Bot";

        return "Mängija O";
    }

    private void HighlightWinningLine(Button[] line)
    {
        foreach (Button button in line)
            button.BackgroundColor = Colors.LightGreen;
    }

    private void NewGame_Clicked(object sender, EventArgs e)
    {
        if (matchFinished)
            return;

        round++;

        ResetBoard();

        currentPlayer = "X";

        UpdateGameInfo();
    }

    private async void WhoStarts_Clicked(
        object sender,
        EventArgs e)
    {
        if (matchFinished)
        {
            await DisplayAlert(
                "Matš on lõppenud",
                "Alusta uut matši.",
                "OK");

            return;
        }

        ResetBoard();

        currentPlayer =
            random.Next(2) == 0 ? "X" : "O";

        UpdateGameInfo();

        string starter =
            GetWinnerName(currentPlayer);

        await DisplayAlert(
            "Alustaja",
            $"Mängu alustab {starter}.",
            "OK");

        if (botMode && currentPlayer == "O")
            await BotMove();
    }

    private void ResetBoard()
    {
        foreach (Button button in GetButtons())
        {
            button.Text = "";
            button.TextColor = Colors.Black;
            button.BackgroundColor = Colors.White;
        }

        moves = 0;
        gameOver = false;

        UpdateGameInfo();
    }

    private void GameModePicker_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        botMode =
            GameModePicker.SelectedIndex == 1;

        ResetMatch();

        MatchInfoLabel.Text =
            botMode
                ? "Sina mängid X-ga. Esimene 2 võiduni võidab."
                : "Esimene, kes saab 2 võitu, võidab matši.";

        UpdateMatchScore();
    }

    private async void ResetMatch_Clicked(
        object sender,
        EventArgs e)
    {
        bool answer =
            await DisplayAlert(
                "Uus matš",
                "Kas soovid alustada uut matši?",
                "Jah",
                "Ei");

        if (answer)
            ResetMatch();
    }

    private void ResetMatch()
    {
        xWins = 0;
        oWins = 0;
        draws = 0;

        round = 1;
        moves = 0;

        currentPlayer = "X";

        gameOver = false;
        matchFinished = false;

        LastWinnerLabel.Text =
            "Viimane võitja: -";

        ResetBoard();

        UpdateStatistics();
        UpdateMatchScore();
        UpdateGameInfo();
    }

    private void UpdateGameInfo()
    {
        if (!gameOver && !matchFinished)
        {
            if (currentPlayer == "X")
            {
                PlayerLabel.Text =
                    botMode
                        ? "Sinu kord - X"
                        : "Mängija X kord";
            }
            else
            {
                PlayerLabel.Text =
                    botMode
                        ? "Boti kord - O"
                        : "Mängija O kord";
            }
        }

        RoundLabel.Text =
            $"Voor: {round}";

        MovesLabel.Text =
            $"Käike: {moves}";
    }

    private void UpdateStatistics()
    {
        XWinsLabel.Text =
            $"Võite: {xWins}";

        OWinsLabel.Text =
            botMode
                ? $"Bot: {oWins}"
                : $"Võite: {oWins}";

        DrawsLabel.Text =
            $"Viike: {draws}";
    }

    private void UpdateMatchScore()
    {
        MatchScoreLabel.Text =
            botMode
                ? $"X  {xWins} : {oWins}  Bot"
                : $"X  {xWins} : {oWins}  O";
    }

    private Button[] GetButtons()
    {
        return new[]
        {
            Button00,
            Button01,
            Button02,
            Button10,
            Button11,
            Button12,
            Button20,
            Button21,
            Button22
        };
    }

    private Button[][] GetWinningLines()
    {
        return new[]
        {
            new[] { Button00, Button01, Button02 },
            new[] { Button10, Button11, Button12 },
            new[] { Button20, Button21, Button22 },

            new[] { Button00, Button10, Button20 },
            new[] { Button01, Button11, Button21 },
            new[] { Button02, Button12, Button22 },

            new[] { Button00, Button11, Button22 },
            new[] { Button02, Button11, Button20 }
        };
    }

    private async void Rules_Clicked(
        object sender,
        EventArgs e)
    {
        await DisplayAlert(
            "Mängureeglid",
            "Trips-Traps-Trull on mäng 3x3 ruudustikul.\n\n" +
            "X ja O teevad käike kordamööda.\n\n" +
            "Võidab mängija, kes saab kolm märki järjest.\n\n" +
            "Võita saab horisontaalselt, vertikaalselt või diagonaalselt.\n\n" +
            "Mäng toimub Best of 3 süsteemis. " +
            "Esimene mängija, kes saab 2 vooruvõitu, võidab matši.\n\n" +
            "Botirežiimis mängid sina X-ga ja bot O-ga.",
            "OK");
    }
}
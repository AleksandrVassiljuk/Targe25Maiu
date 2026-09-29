namespace Targe25Maiu;

public partial class TicTacToePage : ContentPage
{
    private string currentPlayer = "X";

    private int xWins = 0;
    private int oWins = 0;
    private int draws = 0;

    public TicTacToePage()
    {
        InitializeComponent();

        LoadStatistics();
        UpdateStatistics();
    }

    private async void GameButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        // Kui ruut on juba täidetud, siis midagi ei tee
        if (!string.IsNullOrWhiteSpace(button.Text))
            return;

        button.Text = currentPlayer;

        if (currentPlayer == "X")
        {
            button.TextColor = Colors.Red;
        }
        else
        {
            button.TextColor = Colors.Blue;
        }

        // Kontrollime võitu
        if (CheckWinner())
        {
            string winner = currentPlayer;

            if (winner == "X")
            {
                xWins++;
            }
            else
            {
                oWins++;
            }

            SaveStatistics();
            UpdateStatistics();

            bool newGame = await DisplayAlert(
                "Mäng läbi!",
                $"{winner} võitis! Kas soovid veel mängida?",
                "Jah",
                "Ei");

            if (newGame)
            {
                ResetBoard();
            }

            return;
        }

        // Kontrollime viiki
        if (IsDraw())
        {
            draws++;

            SaveStatistics();
            UpdateStatistics();

            bool newGame = await DisplayAlert(
                "Viik!",
                "Mäng jäi viiki. Kas soovid veel mängida?",
                "Jah",
                "Ei");

            if (newGame)
            {
                ResetBoard();
            }

            return;
        }

        // Vahetame mängijat
        currentPlayer = currentPlayer == "X" ? "O" : "X";

        PlayerLabel.Text = $"Mängija {currentPlayer} kord";
    }

    private bool CheckWinner()
    {
        string b00 = Button00.Text ?? "";
        string b01 = Button01.Text ?? "";
        string b02 = Button02.Text ?? "";

        string b10 = Button10.Text ?? "";
        string b11 = Button11.Text ?? "";
        string b12 = Button12.Text ?? "";

        string b20 = Button20.Text ?? "";
        string b21 = Button21.Text ?? "";
        string b22 = Button22.Text ?? "";

        // Read
        if (Same(b00, b01, b02)) return true;
        if (Same(b10, b11, b12)) return true;
        if (Same(b20, b21, b22)) return true;

        // Veerud
        if (Same(b00, b10, b20)) return true;
        if (Same(b01, b11, b21)) return true;
        if (Same(b02, b12, b22)) return true;

        // Diagonaalid
        if (Same(b00, b11, b22)) return true;
        if (Same(b02, b11, b20)) return true;

        return false;
    }

    private bool Same(string a, string b, string c)
    {
        return !string.IsNullOrWhiteSpace(a) &&
               a == b &&
               b == c;
    }

    private bool IsDraw()
    {
        Button[] buttons =
        {
            Button00, Button01, Button02,
            Button10, Button11, Button12,
            Button20, Button21, Button22
        };

        foreach (Button button in buttons)
        {
            if (string.IsNullOrWhiteSpace(button.Text))
            {
                return false;
            }
        }

        return true;
    }

    private void NewGame_Clicked(object sender, EventArgs e)
    {
        ResetBoard();
    }

    private void ResetBoard()
    {
        ClearBoard();

        currentPlayer = "X";

        PlayerLabel.Text = "Mängija X kord";
    }

    private void ClearBoard()
    {
        Button[] buttons =
        {
            Button00, Button01, Button02,
            Button10, Button11, Button12,
            Button20, Button21, Button22
        };

        foreach (Button button in buttons)
        {
            button.Text = "";
            button.TextColor = Colors.White;
        }
    }

    private async void WhoStarts_Clicked(object sender, EventArgs e)
    {
        ClearBoard();

        Random random = new Random();

        if (random.Next(0, 2) == 0)
        {
            currentPlayer = "X";
        }
        else
        {
            currentPlayer = "O";
        }

        PlayerLabel.Text = $"Mängija {currentPlayer} kord";

        await DisplayAlert(
            "Kes alustab?",
            $"Mängija {currentPlayer} alustab!",
            "OK");
    }

    private async void Rules_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Mängureeglid",
            "1. Mängija X ja mängija O käivad kordamööda.\n\n" +
            "2. Vajuta vabale ruudule, et panna sinna oma sümbol.\n\n" +
            "3. Võidab mängija, kes saab kolm oma sümbolit järjest.\n\n" +
            "4. Rida võib olla horisontaalne, vertikaalne või diagonaalne.\n\n" +
            "5. Kui kõik ruudud on täis ja keegi ei võida, jääb mäng viiki.",
            "Sulge");
    }

    private void SaveStatistics()
    {
        Preferences.Default.Set("TicTacToe_XWins", xWins);
        Preferences.Default.Set("TicTacToe_OWins", oWins);
        Preferences.Default.Set("TicTacToe_Draws", draws);
    }

    private void LoadStatistics()
    {
        xWins = Preferences.Default.Get("TicTacToe_XWins", 0);
        oWins = Preferences.Default.Get("TicTacToe_OWins", 0);
        draws = Preferences.Default.Get("TicTacToe_Draws", 0);
    }

    private void UpdateStatistics()
    {
        XWinsLabel.Text = $"X võite: {xWins}";
        OWinsLabel.Text = $"O võite: {oWins}";
        DrawsLabel.Text = $"Viike: {draws}";
    }
}
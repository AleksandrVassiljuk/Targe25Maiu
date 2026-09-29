namespace Targe25Maiu;

public partial class PopUpPage : ContentPage
{
    private int score = 0;
    private int maxNumber = 5;

    public PopUpPage()
    {
        InitializeComponent();
    }

    // DisplayPromptAsync
    // Küsib kasutajalt nime
    private async void NameButton_Clicked(object sender, EventArgs e)
    {
        string nimi = await DisplayPromptAsync(
            "Tere!",
            "Mis on sinu nimi?",
            "Salvesta",
            "Loobu");

        if (!string.IsNullOrWhiteSpace(nimi))
        {
            NameLabel.Text = nimi;

            await DisplayAlert(
                "Tere!",
                $"Tere tulemast, {nimi}!",
                "OK");
        }
    }

    // DisplayActionSheet
    // Kasutaja saab valida raskusastme
    private async void DifficultyButton_Clicked(object sender, EventArgs e)
    {
        string valik = await DisplayActionSheet(
            "Vali raskusaste",
            "Loobu",
            null,
            "Lihtne",
            "Keskmine",
            "Raske");

        if (valik == "Lihtne")
        {
            maxNumber = 5;
            DifficultyLabel.Text = "Lihtne";
        }
        else if (valik == "Keskmine")
        {
            maxNumber = 10;
            DifficultyLabel.Text = "Keskmine";
        }
        else if (valik == "Raske")
        {
            maxNumber = 15;
            DifficultyLabel.Text = "Raske";
        }
    }

    // Korrutustabeli küsimus
    private async void StartButton_Clicked(object sender, EventArgs e)
    {
        Random random = new Random();

        int arv1 = random.Next(1, maxNumber + 1);
        int arv2 = random.Next(1, maxNumber + 1);

        int oigeVastus = arv1 * arv2;

        string vastus = await DisplayPromptAsync(
            "Korrutustabel",
            $"Kui palju on {arv1} × {arv2}?",
            "Vasta",
            "Loobu",
            keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(vastus))
        {
            return;
        }

        if (int.TryParse(vastus, out int kasutajaVastus))
        {
            if (kasutajaVastus == oigeVastus)
            {
                score++;
                ScoreLabel.Text = score.ToString();

                await DisplayAlert(
                    "Õige! 🎉",
                    $"{arv1} × {arv2} = {oigeVastus}",
                    "OK");
            }
            else
            {
                await DisplayAlert(
                    "Vale vastus ❌",
                    $"Õige vastus on {oigeVastus}.",
                    "OK");
            }
        }
        else
        {
            await DisplayAlert(
                "Viga",
                "Palun sisesta number.",
                "OK");
        }

        // Yes / No DisplayAlert
        bool uuesti = await DisplayAlert(
            "Järgmine küsimus",
            "Kas soovid veel ühe küsimuse?",
            "Jah",
            "Ei");

        if (uuesti)
        {
            StartButton_Clicked(sender, e);
        }
    }

    // Punktide nullimine
    private async void ResetButton_Clicked(object sender, EventArgs e)
    {
        bool vasta = await DisplayAlert(
            "Nulli punktid",
            "Kas oled kindel, et soovid punktid nullida?",
            "Jah",
            "Ei");

        if (vasta)
        {
            score = 0;
            ScoreLabel.Text = "0";

            await DisplayAlert(
                "Valmis",
                "Punktid on nullitud.",
                "OK");
        }
    }
}
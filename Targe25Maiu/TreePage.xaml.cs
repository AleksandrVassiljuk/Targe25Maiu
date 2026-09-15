namespace Targe25Maiu;

public partial class TreePage : ContentPage
{
    public TreePage()
    {
        InitializeComponent();
        Algseis();
    }

    private async void Käivita_Clicked(object sender, EventArgs e)
    {
        string tegevus = ActionPicker.SelectedItem?.ToString() ?? "";

        if (tegevus == "")
        {
            InfoLabel.Text = "Vali kõigepealt tegevus!";
            return;
        }

        uint kiirus = (uint)SpeedStepper.Value;

        // KASVA
        if (tegevus == "Kasva")
        {
            InfoLabel.Text = "🌱 Puu kasvab!";

            Flowers.IsVisible = false;

            // Alustab väiksemana
            Tree.Scale = 0.55;

            // Kasvab sujuvalt normaalse suuruseni
            await Tree.ScaleTo(1.0, kiirus);
        }

        // ÕITSE
        else if (tegevus == "Õitse")
        {
            InfoLabel.Text = "🌸 Puu õitseb!";

            Flowers.IsVisible = true;
            Flowers.Opacity = 0;

            await Flowers.FadeTo(1, kiirus);
        }

        // VÄRISE
        else if (tegevus == "Värise")
        {
            InfoLabel.Text = "🌬️ Puu väriseb tuules!";

            await Tree.TranslateTo(-10, 0, 150);
            await Tree.TranslateTo(10, 0, 150);
            await Tree.TranslateTo(-8, 0, 150);
            await Tree.TranslateTo(8, 0, 150);
            await Tree.TranslateTo(0, 0, 150);
        }

        // LANGETA
        else if (tegevus == "Langeta")
        {
            int kuu = 0;

            if (DatePickerControl.Date.HasValue)
            {
                kuu = DatePickerControl.Date.Value.Month;
            }

            bool onTalv =
                kuu == 12 ||
                kuu == 1 ||
                kuu == 2;

            if (!onTalv)
            {
                InfoLabel.Text =
                    "❌ Puud saab langetada ainult talvel!";
                return;
            }

            InfoLabel.Text = "🪓 Puu hakkab kukkuma!";

            // Kõigepealt hakkab puu aeglaselt külili minema
            await Tree.RotateTo(25, 300);

            // Siis kukub rohkem külili
            await Tree.RotateTo(70, 300);

            // Lõplik kukkumine
            await Tree.RotateTo(90, 200);

            // Puu liigub alla
            await Tree.TranslateTo(0, 130, 500);

            // Kogu ekraan must
            BlackScreen.IsVisible = true;

            // Must ekraan 3 sekundit
            await Task.Delay(3000);

            // Must ekraan ära
            BlackScreen.IsVisible = false;

            // Puu tagasi algasendisse
            Tree.Scale = 1;
            Tree.Rotation = 0;
            Tree.TranslationX = 0;
            Tree.TranslationY = 0;

            Flowers.IsVisible = false;

            InfoLabel.Text = "🌳 Puu on jälle püsti!";
        }
    }

    private void Opacity_Changed(
        object sender,
        ValueChangedEventArgs e)
    {
        Foliage.Opacity = e.NewValue;
    }

    private void Restart_Clicked(
        object sender,
        EventArgs e)
    {
        BlackScreen.IsVisible = false;

        Tree.Scale = 1;
        Tree.Rotation = 0;
        Tree.TranslationX = 0;
        Tree.TranslationY = 0;

        Foliage.Opacity = 1;

        Flowers.IsVisible = false;
        Flowers.Opacity = 1;

        ActionPicker.SelectedIndex = -1;

        OpacitySlider.Value = 1;

        SpeedStepper.Value = 1000;

        DatePickerControl.Date = DateTime.Today;

        TimePickerControl.Time =
            new TimeSpan(12, 0, 0);

        InfoLabel.Text =
            "🌳 Puu on algseisus!";
    }

    private void Algseis()
    {
        BlackScreen.IsVisible = false;

        Tree.Scale = 1;
        Tree.Rotation = 0;
        Tree.TranslationX = 0;
        Tree.TranslationY = 0;

        Foliage.Opacity = 1;

        Flowers.IsVisible = false;
        Flowers.Opacity = 1;

        ActionPicker.SelectedIndex = -1;

        OpacitySlider.Value = 1;

        SpeedStepper.Value = 1000;

        DatePickerControl.Date = DateTime.Today;

        TimePickerControl.Time =
            new TimeSpan(12, 0, 0);

        InfoLabel.Text =
            "Vali tegevus";
    }
}
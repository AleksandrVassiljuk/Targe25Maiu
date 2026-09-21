namespace Targe25Maiu;

public partial class TreePage : ContentPage
{
    private bool isGrown = false;
    private bool isFallen = false;
    private bool isAnimating = false;

    public TreePage()
    {
        InitializeComponent();

        // ALGSEIS
        Flowers.IsVisible = false;
        InfoLabel.Text = "Vali tegevus";

        OpacitySlider.Value = 1;
        SpeedStepper.Value = 1000;

        DatePickerControl.Date = DateTime.Today;
        TimePickerControl.Time = DateTime.Now.TimeOfDay;

        // Puu pöörlemispunkt on all keskel
        Tree.AnchorX = 0.5;
        Tree.AnchorY = 1.0;

        // Kukutamise nupp ühendatakse C# kaudu
        FallTreeButton.Clicked += FallTreeButton_Clicked;
    }


    // =====================================================
    // KÄIVITA
    // =====================================================

    private async void Käivita_Clicked(object sender, EventArgs e)
    {
        if (isAnimating)
        {
            return;
        }

        if (isFallen)
        {
            InfoLabel.Text = "🪵 Puu on maas! Vajuta RESTART.";
            return;
        }

        if (ActionPicker.SelectedItem == null)
        {
            InfoLabel.Text = "⚠️ Vali kõigepealt tegevus!";
            return;
        }

        string action =
            ActionPicker.SelectedItem.ToString() ?? "";

        uint speed =
            (uint)SpeedStepper.Value;

        switch (action)
        {
            case "Kasva":
                await GrowTree(speed);
                break;

            case "Õitse":
                await BloomTree(speed);
                break;

            case "Värise":
                await ShakeTree(speed);
                break;

            case "Langeta":
                await DropLeaves(speed);
                break;
        }
    }


    // =====================================================
    // KASVA
    // =====================================================

    private async Task GrowTree(uint speed)
    {
        isAnimating = true;

        InfoLabel.Text = "🌱 Puu kasvab...";

        Flowers.IsVisible = false;

        Tree.Rotation = 0;
        Tree.TranslationX = 0;
        Tree.TranslationY = 0;

        Tree.Scale = 0.55;
        Tree.Opacity = 0.5;

        await Task.WhenAll(
            Tree.ScaleTo(
                1,
                speed,
                Easing.BounceOut),

            Tree.FadeTo(
                1,
                speed)
        );

        Foliage.Opacity =
            OpacitySlider.Value;

        isGrown = true;
        isAnimating = false;

        InfoLabel.Text =
            "🌳 Puu on suureks kasvanud!";
    }


    // =====================================================
    // ÕITSE
    // =====================================================

    private async Task BloomTree(uint speed)
    {
        if (!isGrown)
        {
            InfoLabel.Text =
                "🌱 Kõigepealt kasvata puu!";
            return;
        }

        isAnimating = true;

        InfoLabel.Text =
            "🌸 Puu hakkab õitsema...";

        Flowers.IsVisible = true;
        Flowers.Opacity = 0;
        Flowers.Scale = 0.5;

        await Task.WhenAll(
            Flowers.FadeTo(
                1,
                speed),

            Flowers.ScaleTo(
                1,
                speed,
                Easing.BounceOut)
        );

        isAnimating = false;

        InfoLabel.Text =
            "🌸 Puu õitseb!";
    }


    // =====================================================
    // VÄRISE
    // =====================================================

    private async Task ShakeTree(uint speed)
    {
        if (isFallen)
        {
            InfoLabel.Text =
                "🪵 Puu on maas!";
            return;
        }

        isAnimating = true;

        InfoLabel.Text =
            "💨 Tuul puhub ja puu väriseb...";

        uint shakeSpeed =
            speed / 10;

        if (shakeSpeed < 50)
        {
            shakeSpeed = 50;
        }

        Tree.AnchorX = 0.5;
        Tree.AnchorY = 1.0;

        for (int i = 0; i < 5; i++)
        {
            await Tree.RotateTo(
                -4,
                shakeSpeed,
                Easing.Linear);

            await Tree.RotateTo(
                4,
                shakeSpeed,
                Easing.Linear);
        }

        await Tree.RotateTo(
            0,
            shakeSpeed,
            Easing.Linear);

        isAnimating = false;

        InfoLabel.Text =
            "🌳 Puu lõpetas värisemise.";
    }


    // =====================================================
    // LANGETA LEHED
    // =====================================================

    private async Task DropLeaves(uint speed)
    {
        if (!isGrown)
        {
            InfoLabel.Text =
                "🌱 Kõigepealt kasvata puu!";
            return;
        }

        isAnimating = true;

        InfoLabel.Text =
            "🍂 Lehed langevad...";

        Flowers.IsVisible = false;

        await Foliage.FadeTo(
            0.10,
            speed,
            Easing.CubicIn);

        isAnimating = false;

        InfoLabel.Text =
            "🍂 Puu langetas lehed.";
    }


    // =====================================================
    // KUKUTA PUU 90 KRAADI
    // =====================================================

    private async void FallTreeButton_Clicked(
        object? sender,
        EventArgs e)
    {
        if (isAnimating)
        {
            return;
        }

        if (isFallen)
        {
            InfoLabel.Text =
                "🪵 Puu on juba maas!";
            return;
        }

        isAnimating = true;

        InfoLabel.Text =
            "🌳 Puu hakkab kukkuma...";


        // -----------------------------------------
        // PUU ALUSTAB PÜSTISELT
        // -----------------------------------------

        Tree.AnchorX = 0.5;
        Tree.AnchorY = 1.0;

        Tree.Rotation = 0;

        Tree.TranslationX = 0;
        Tree.TranslationY = 0;


        // -----------------------------------------
        // VÄIKE NÕKS VASAKULE
        // -----------------------------------------

        await Tree.RotateTo(
            -5,
            200,
            Easing.CubicOut);


        // -----------------------------------------
        // KUKKUMINE
        // -----------------------------------------
        // Rotation = 90 tähendab,
        // et puu on täiesti külili.
        //
        // TranslationX viib puu paremale.
        // TranslationY viib puu murule.
        // -----------------------------------------

        await Task.WhenAll(

            Tree.RotateTo(
                90,
                1400,
                Easing.CubicIn),

            Tree.TranslateTo(
                105,
                55,
                1400,
                Easing.CubicIn)
        );


        // -----------------------------------------
        // KINDLUSTAME TÄPSE LÕPPASENDI
        // -----------------------------------------

        Tree.Rotation = 90;
        Tree.TranslationX = 105;
        Tree.TranslationY = 55;


        isFallen = true;
        isAnimating = false;

        InfoLabel.Text =
            "🪵 Puu kukkus 90° maha!";
    }


    // =====================================================
    // LEHESTIKU LÄBIPAISTVUS
    // =====================================================

    private void Opacity_Changed(
        object sender,
        ValueChangedEventArgs e)
    {
        if (Foliage != null)
        {
            Foliage.Opacity =
                e.NewValue;
        }
    }


    // =====================================================
    // RESTART
    // =====================================================

    private async void Restart_Clicked(
        object sender,
        EventArgs e)
    {
        if (isAnimating)
        {
            return;
        }

        isAnimating = true;

        InfoLabel.Text =
            "🔄 Restart...";


        // MUST EKRAAN
        BlackScreen.IsVisible = true;
        BlackScreen.Opacity = 0;

        await BlackScreen.FadeTo(
            1,
            250);


        // =================================================
        // PUU TAGASI PÜSTI
        // =================================================

        Tree.CancelAnimations();

        Tree.Rotation = 0;

        Tree.TranslationX = 0;
        Tree.TranslationY = 0;

        Tree.AnchorX = 0.5;
        Tree.AnchorY = 1.0;

        Tree.Scale = 1;
        Tree.Opacity = 1;


        // =================================================
        // LEHESTIK
        // =================================================

        Foliage.CancelAnimations();

        Foliage.IsVisible = true;
        Foliage.Opacity = 1;
        Foliage.Scale = 1;


        // =================================================
        // ÕIED
        // =================================================

        Flowers.CancelAnimations();

        Flowers.IsVisible = false;
        Flowers.Opacity = 1;
        Flowers.Scale = 1;


        // =================================================
        // PICKERID
        // =================================================

        ActionPicker.SelectedIndex = -1;
        SeasonPicker.SelectedIndex = -1;


        // =================================================
        // SLIDER
        // =================================================

        OpacitySlider.Value = 1;


        // =================================================
        // KIIRUS
        // =================================================

        SpeedStepper.Value = 1000;


        // =================================================
        // KUUPÄEV JA KELL
        // =================================================

        DatePickerControl.Date =
            DateTime.Today;

        TimePickerControl.Time =
            DateTime.Now.TimeOfDay;


        // =================================================
        // OLEK
        // =================================================

        isGrown = false;
        isFallen = false;


        await Task.Delay(200);


        // MUST EKRAAN KAOB
        await BlackScreen.FadeTo(
            0,
            250);

        BlackScreen.IsVisible = false;

        isAnimating = false;

        InfoLabel.Text =
            "🌳 Puu on algseisus. Vali tegevus.";
    }
}
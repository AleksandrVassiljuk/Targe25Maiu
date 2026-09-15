using Microsoft.Maui.Layouts;

namespace Targe25Maiu;

public partial class RgbPage : ContentPage
{
    Label redLabel, greenLabel, blueLabel;
    Slider redSlider, greenSlider, blueSlider;
    BoxView boxView;
    Button randomButton;
    AbsoluteLayout absoluteLayout;

    public RgbPage()
    {
        Title = "RGB Värvi Muutja";

        // Värvi kuvamise ala
        boxView = new BoxView
        {
            Color = Colors.Black,
            CornerRadius = 10
        };

        // Liugurid ja nende sildid
        redSlider = CreateColorSlider();
        greenSlider = CreateColorSlider();
        blueSlider = CreateColorSlider();

        redLabel = CreateColorLabel("Red = 00");
        greenLabel = CreateColorLabel("Green = 00");
        blueLabel = CreateColorLabel("Blue = 00");

        // Juhusliku värvi nupp
        randomButton = new Button
        {
            Text = "Juhuslik värv",
            BackgroundColor = Colors.DarkSlateGray,
            TextColor = Colors.White,
            CornerRadius = 8
        };
        randomButton.Clicked += OnRandomButtonClicked;

        absoluteLayout = new AbsoluteLayout();

        // Elementide paigutus (AbsoluteLayout)
        AddControlToLayout(boxView, 0.1, 300, 120);

        AddControlToLayout(redLabel, 0.32, 300, 25);
        AddControlToLayout(redSlider, 0.38, 300, 40);

        AddControlToLayout(greenLabel, 0.48, 300, 25);
        AddControlToLayout(greenSlider, 0.54, 300, 40);

        AddControlToLayout(blueLabel, 0.64, 300, 25);
        AddControlToLayout(blueSlider, 0.70, 300, 40);

        AddControlToLayout(randomButton, 0.84, 300, 50);

        Content = absoluteLayout;
    }

    private Slider CreateColorSlider()
    {
        var slider = new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 0
        };
        slider.ValueChanged += OnSliderValueChanged;
        return slider;
    }

    private Label CreateColorLabel(string text)
    {
        return new Label
        {
            Text = text,
            FontSize = 16,
            HorizontalTextAlignment = TextAlignment.Center
        };
    }

    private void AddControlToLayout(View view, double yProportional, double width, double height)
    {
        absoluteLayout.Children.Add(view);
        AbsoluteLayout.SetLayoutBounds(view, new Rect(0.5, yProportional, width, height));
        AbsoluteLayout.SetLayoutFlags(view, AbsoluteLayoutFlags.PositionProportional);
    }

    // Pildilt pärit sündmusfunktsioon
    private void OnSliderValueChanged(object? sender, ValueChangedEventArgs args)
    {
        if (sender == redSlider)
        {
            redLabel.Text = String.Format("Red = {0:X2}", (int)args.NewValue);
        }
        else if (sender == greenSlider)
        {
            greenLabel.Text = String.Format("Green = {0:X2}", (int)args.NewValue);
        }
        else if (sender == blueSlider)
        {
            blueLabel.Text = String.Format("Blue = {0:X2}", (int)args.NewValue);
        }

        // MAUI-s teisendatakse int väärtused (0..255) floatiks (0.0..1.0) vahemikus
        boxView.Color = Color.FromRgb((int)redSlider.Value,
                                     (int)greenSlider.Value,
                                     (int)blueSlider.Value);
    }

    // Juhusliku värvi generaator (animatsiooniga)
    private async void OnRandomButtonClicked(object? sender, EventArgs e)
    {
        Random rnd = new Random();
        double targetR = rnd.Next(0, 256);
        double targetG = rnd.Next(0, 256);
        double targetB = rnd.Next(0, 256);

        uint duration = 400;
        var anim = new Animation();
        anim.Add(0, 1, new Animation(v => redSlider.Value = v, redSlider.Value, targetR));
        anim.Add(0, 1, new Animation(v => greenSlider.Value = v, greenSlider.Value, targetG));
        anim.Add(0, 1, new Animation(v => blueSlider.Value = v, blueSlider.Value, targetB));

        anim.Commit(this, "ColorAnimation", 16, duration, Easing.CubicInOut);
        await Task.Delay((int)duration);
    }
}
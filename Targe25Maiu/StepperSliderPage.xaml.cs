using Microsoft.Maui.Layouts;

namespace Targe25Maiu;

public partial class StepperSliderPage : ContentPage
{
    Label _label;
    Stepper _stepper;
    Slider _slider;
    AbsoluteLayout _al;

    public StepperSliderPage()
    {
        _label = new Label
        {
            Text = "V‰‰rtus: 50",
            FontSize = 24,
            BackgroundColor = Colors.LightGray,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _stepper = new Stepper
        {
            Minimum = 0,
            Maximum = 360,
            Increment = 5,
            Value = 50,
            HorizontalOptions = LayoutOptions.Center
        };

        _slider = new Slider
        {
            Minimum = 0,
            Maximum = 360,
            Value = 50,
            HorizontalOptions = LayoutOptions.Center,
            MinimumTrackColor = Colors.LightGray,
            MaximumTrackColor = Colors.DarkGray,
            ThumbColor = Colors.Gray
        };

        _stepper.ValueChanged += Stepper_ValueChanged;
        _slider.ValueChanged += Slider_ValueChanged;

        _al = new AbsoluteLayout();

        _al.Children.Add(_label);
        _al.Children.Add(_stepper);
        _al.Children.Add(_slider);

        List<View> controls = new List<View>
        {
            _label,
            _stepper,
            _slider
        };

        for (int i = 0; i < controls.Count; i++)
        {
            double yKoht = 0.2 + i * 0.2;

            AbsoluteLayout.SetLayoutBounds(
                controls[i],
                new Rect(
                    0.5,
                    yKoht,
                    300,
                    60
                )
            );

            AbsoluteLayout.SetLayoutFlags(
                controls[i],
                AbsoluteLayoutFlags.PositionProportional
            );
        }

        Content = _al;
    }

    private void Stepper_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        _slider.Value = e.NewValue;
        MuudaVaartust(e.NewValue);
    }

    private void Slider_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        _stepper.Value = e.NewValue;
        MuudaVaartust(e.NewValue);
    }

    private void MuudaVaartust(double value)
    {
        _label.Text = $"V‰‰rtus: {value:F0}";

        _label.FontSize = 24 + value / 4;

        int punane = Math.Clamp((int)(value * 2.55), 0, 255);
        int roheline = Math.Clamp((int)(255 - value * 2.55), 0, 255);

        _label.BackgroundColor = Color.FromRgb(
            punane,
            roheline,
            128
        );

        _label.TextColor = Color.FromRgb(
            roheline,
            punane,
            128
        );

        _label.Rotation = value;
    }
}
using Microsoft.Maui.Layouts;

namespace Targe25Maiu;

public partial class DateTimePage : ContentPage
{
    DatePicker datePicker;
    TimePicker timePicker;
    Label datetimeLabel;
    AbsoluteLayout al;

    public DateTimePage()
    {
        datePicker = new DatePicker
        {
            MinimumDate = DateTime.Now.AddDays(-15),
            MaximumDate = DateTime.Now.AddDays(15),
            Date = DateTime.Now,
            HorizontalOptions = LayoutOptions.Center,
            Format = "D"
        };

        timePicker = new TimePicker
        {
            Time = DateTime.Now.TimeOfDay,
            HorizontalOptions = LayoutOptions.Center,
            Format = "T"
        };

        datetimeLabel = new Label
        {
            Text = "Valitud kuupäev või aeg",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        datePicker.DateSelected += (sender, e) =>
        {
            datetimeLabel.Text =
                $"Valitud kuupäev:\n{datePicker.Date:D}";
        };

        timePicker.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(TimePicker.Time))
            {
                datetimeLabel.Text =
                    $"Valitud kellaaeg:\n{timePicker.Time:T}";
            }
        };

        al = new AbsoluteLayout();

        al.Children.Add(datePicker);
        al.Children.Add(timePicker);
        al.Children.Add(datetimeLabel);

        List<View> controls = new List<View>
        {
            datePicker,
            timePicker,
            datetimeLabel
        };

        for (int i = 0; i < controls.Count; i++)
        {
            double ykont = 0.2 + i * 0.2;

            AbsoluteLayout.SetLayoutBounds(
                controls[i],
                new Rect(
                    0.5,
                    ykont,
                    AbsoluteLayout.AutoSize,
                    AbsoluteLayout.AutoSize
                )
            );

            AbsoluteLayout.SetLayoutFlags(
                controls[i],
                AbsoluteLayoutFlags.PositionProportional
            );
        }

        Content = al;
    }
}
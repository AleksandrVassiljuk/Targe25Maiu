namespace Targe25Maiu;

public partial class TextPage : ContentPage
{
    Label lbl;
    Editor editor;
    HorizontalStackLayout hsl;
    VerticalStackLayout vsl;

    List<string> nupud = new List<string>()
    {
        "Tagasi",
        "Avaleht",
        "Edasi"
    };

    public TextPage()
    {
        lbl = new Label
        {
            Text = "Pealkiri",
            FontSize = 30,
            FontFamily = "Luffio",
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold
        };

        editor = new Editor
        {
            Placeholder = "Sisesta tekst...",
            PlaceholderColor = Colors.Red,
            FontSize = 18,
            FontAttributes = FontAttributes.Italic,
            HorizontalOptions = LayoutOptions.Center
        };

        editor.TextChanged += (sender, e) =>
        {
            lbl.Text = editor.Text;
        };

        hsl = new HorizontalStackLayout
        {
            Spacing = 20,
            HorizontalOptions = LayoutOptions.Center
        };

        for (int j = 0; j < nupud.Count; j++)
        {
            Button nupp = new Button
            {
                Text = nupud[j],
                FontSize = 20,
                FontFamily = "Luffio",
                TextColor = Colors.BlueViolet,
                BackgroundColor = Colors.LightGray,
                CornerRadius = 10,
                HeightRequest = 50,
                ZIndex = j
            };

            hsl.Add(nupp);
            nupp.Clicked += Liikumine;
        }

        vsl = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            Children =
            {
                lbl,
                editor,
                hsl
            },
            HorizontalOptions = LayoutOptions.Center
        };

        Content = vsl;
    }

    private void Liikumine(object? sender, EventArgs e)
    {
        if (sender is not Button nupp)
            return;

        switch (nupp.ZIndex)
        {
            case 0:
                Navigation.PopAsync();
                break;

            case 1:
                Navigation.PopToRootAsync();
                break;

            case 2:
                Navigation.PushAsync(new FigurePage());
                break;
        }
    }
}
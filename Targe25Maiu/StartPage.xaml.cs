namespace Targe25Maiu;

public partial class StartPage : ContentPage
{
    VerticalStackLayout vst;
    ScrollView sv;

    public List<ContentPage> Lehed = new List<ContentPage>()
    {
        new TextPage(),
        new FigurePage(),
        new DateTimePage(),
        new StepperSliderPage(),
        new RgbPage(),
        new TreePage()
    };

    public List<string> LeheNimed = new List<string>()
    {
        "Tekst",
        "Kujund",
        "DateTime",
        "Stepper + Slider",
        "RGB",
        "Puu"
    };

    public StartPage()
    {
        vst = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15
        };

        for (int i = 0; i < Lehed.Count; i++)
        {
            Button nupp = new Button
            {
                Text = LeheNimed[i],
                FontSize = 30,
                FontFamily = "Luffio",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10,
                HeightRequest = 60,
                ZIndex = i
            };

            vst.Add(nupp);

            nupp.Clicked += (sender, e) =>
            {
                int indeks = nupp.ZIndex;
                Navigation.PushAsync(Lehed[indeks]);
            };
        }

        sv = new ScrollView
        {
            Content = vst
        };

        Content = sv;
    }
}
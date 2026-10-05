using System.Globalization;

namespace Targe25Maiu;

public partial class CarouselPage : ContentPage
{
    private List<ProgrammingLanguage> languages = new();
    private bool isRunning = true;
    private bool isEstonian = true;

    public CarouselPage()
    {
        InitializeComponent();

        LanguagePicker.SelectedIndex = 0;

        LoadLanguages();

        LanguageCarousel.ItemsSource = languages;

        StartAutoScroll();
    }

    private void LoadLanguages()
    {
        languages.Clear();

        if (isEstonian)
        {
            languages.Add(new ProgrammingLanguage
            {
                Name = "C#",
                Description = "Võimas objektorienteeritud keel",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Csharp_Logo.png",
                Details = "C# loodi Microsoftis. Hello World:\nConsole.WriteLine(\"Hello, World!\");"
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "Python",
                Description = "Suurepärane andmetöötluseks ja automatiseerimiseks",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c3/Python-logo-notext.svg",
                Details = "Python loodi 1991. aastal. Hello World:\nprint(\"Hello, World!\")"
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "JavaScript",
                Description = "Veebiarenduse põhikeel",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6a/JavaScript-logo.png",
                Details = "JavaScript loodi 1995. aastal. Hello World:\nconsole.log(\"Hello, World!\");"
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "Java",
                Description = "Kirjuta kord, käivita igal pool",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/30/Java_programming_language_logo.svg",
                Details = "Java avaldati 1995. aastal. Seda kasutatakse paljude rakenduste loomisel."
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "C++",
                Description = "Suure jõudlusega süsteemikeel",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/18/ISO_C%2B%2B_Logo.svg",
                Details = "C++ loodi 1980. aastatel. Seda kasutatakse mängude ja suure jõudlusega tarkvara arendamisel."
            });
        }
        else
        {
            languages.Add(new ProgrammingLanguage
            {
                Name = "C#",
                Description = "Powerful object-oriented language",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Csharp_Logo.png",
                Details = "C# was created by Microsoft. Hello World:\nConsole.WriteLine(\"Hello, World!\");"
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "Python",
                Description = "Great for data and automation",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c3/Python-logo-notext.svg",
                Details = "Python was created in 1991. Hello World:\nprint(\"Hello, World!\")"
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "JavaScript",
                Description = "The language of the web",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6a/JavaScript-logo.png",
                Details = "JavaScript was created in 1995. Hello World:\nconsole.log(\"Hello, World!\");"
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "Java",
                Description = "Write once, run anywhere",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/30/Java_programming_language_logo.svg",
                Details = "Java was released in 1995. It is used to build many different applications."
            });

            languages.Add(new ProgrammingLanguage
            {
                Name = "C++",
                Description = "High-performance system language",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/18/ISO_C%2B%2B_Logo.svg",
                Details = "C++ was created in the 1980s. It is commonly used for games and high-performance software."
            });
        }

        LanguageCarousel.ItemsSource = null;
        LanguageCarousel.ItemsSource = languages;
    }

    private async void StartAutoScroll()
    {
        while (isRunning)
        {
            await Task.Delay(4000);

            if (!isRunning || languages.Count == 0)
                return;

            int nextPosition = LanguageCarousel.Position + 1;

            if (nextPosition >= languages.Count)
                nextPosition = 0;

            LanguageCarousel.Position = nextPosition;
        }
    }

    private async void Card_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is ProgrammingLanguage language)
        {
            await DisplayAlert(
                language.Name,
                language.Details,
                "OK");
        }
    }

    private void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (LanguagePicker.SelectedIndex == 0)
        {
            isEstonian = true;

            TitleLabel.Text = "Programmeerimiskeeled";
            LanguageLabel.Text = "Keel:";
        }
        else
        {
            isEstonian = false;

            TitleLabel.Text = "Programming Languages";
            LanguageLabel.Text = "Language:";
        }

        LoadLanguages();

        LanguageCarousel.Position = 0;
    }

    private void LanguageCarousel_PositionChanged(
        object sender,
        PositionChangedEventArgs e)
    {
        // CarouselView muudab aktiivset kaarti.
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        isRunning = false;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!isRunning)
        {
            isRunning = true;
            StartAutoScroll();
        }
    }
}

public class ProgrammingLanguage
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string Details { get; set; } = "";
}
namespace Targe25Maiu;

public partial class CarouselPage : ContentPage
{
    // Programmeerimiskeelte nimekiri
    private List<ProgrammingLanguage> languages = new();

    // Praegu avatud kaardi number
    private int currentIndex = 0;

    // Vaikimisi on eesti keel
    private bool isEstonian = true;

    // Kontrollib animatsiooni
    private bool isAnimating = false;


    public CarouselPage()
    {
        InitializeComponent();

        // Laeb eesti keelsed andmed
        LoadLanguages();

        // Näitab esimest kaarti
        ShowCurrentLanguage();

        // Valib vaikimisi eesti keele
        LanguagePicker.SelectedIndex = 0;
    }


    // Laeb programmeerimiskeelte andmed
    private void LoadLanguages()
    {
        languages = new List<ProgrammingLanguage>();


        // EESTI KEEL
        if (isEstonian)
        {
            // C#
            languages.Add(new ProgrammingLanguage
            {
                Name = "C#",

                Description =
                    "Võimas objektorienteeritud programmeerimiskeel",

                ImageUrl =
                    "csharp.png",

                InfoText =
                    "Vajuta kaardile lisainfo saamiseks",

                Details =
                    "C# on Microsofti loodud programmeerimiskeel.\n\n" +
                    "Seda kasutatakse näiteks .NET rakenduste, " +
                    "veebirakenduste ja mängude arendamiseks.\n\n" +
                    "Hello World:\n" +
                    "Console.WriteLine(\"Hello, World!\");",

                WebsiteUrl =
                    "https://learn.microsoft.com/en-us/dotnet/csharp/"
            });


            // Python
            languages.Add(new ProgrammingLanguage
            {
                Name = "Python",

                Description =
                    "Lihtne ja populaarne programmeerimiskeel",

                ImageUrl =
                    "python.png",

                InfoText =
                    "Vajuta kaardile lisainfo saamiseks",

                Details =
                    "Python loodi 1991. aastal.\n\n" +
                    "Seda kasutatakse automatiseerimises, " +
                    "andmetöötluses, tehisintellektis ja veebiarenduses.\n\n" +
                    "Hello World:\n" +
                    "print(\"Hello, World!\")",

                WebsiteUrl =
                    "https://www.python.org/"
            });


            // JavaScript
            languages.Add(new ProgrammingLanguage
            {
                Name = "JavaScript",

                Description =
                    "Üks peamisi veebiarenduse programmeerimiskeeli",

                ImageUrl =
                    "javascript.png",

                InfoText =
                    "Vajuta kaardile lisainfo saamiseks",

                Details =
                    "JavaScript loodi 1995. aastal.\n\n" +
                    "Seda kasutatakse veebilehtede interaktiivseks " +
                    "muutmiseks ja veebirakenduste arendamiseks.\n\n" +
                    "Hello World:\n" +
                    "console.log(\"Hello, World!\");",

                WebsiteUrl =
                    "https://developer.mozilla.org/en-US/docs/Web/JavaScript"
            });


            // Java
            languages.Add(new ProgrammingLanguage
            {
                Name = "Java",

                Description =
                    "Populaarne objektorienteeritud programmeerimiskeel",

                ImageUrl =
                    "java.png",

                InfoText =
                    "Vajuta kaardile lisainfo saamiseks",

                Details =
                    "Java avaldati 1995. aastal.\n\n" +
                    "Seda kasutatakse näiteks serverirakendustes, " +
                    "ettevõtete süsteemides ja paljudes teistes rakendustes.",

                WebsiteUrl =
                    "https://dev.java/"
            });


            // C++
            languages.Add(new ProgrammingLanguage
            {
                Name = "C++",

                Description =
                    "Kiire ja suure jõudlusega programmeerimiskeel",

                ImageUrl =
                    "cplusplus.png",

                InfoText =
                    "Vajuta kaardile lisainfo saamiseks",

                Details =
                    "C++ loodi 1980. aastatel.\n\n" +
                    "Seda kasutatakse näiteks mängude, " +
                    "operatsioonisüsteemide ja suure jõudlusega " +
                    "tarkvara arendamisel.",

                WebsiteUrl =
                    "https://en.cppreference.com/"
            });
        }


        // INGLISE KEEL
        else
        {
            // C#
            languages.Add(new ProgrammingLanguage
            {
                Name = "C#",

                Description =
                    "Powerful object-oriented programming language",

                ImageUrl =
                    "csharp.png",

                InfoText =
                    "Tap the card for more information",

                Details =
                    "C# is a programming language created by Microsoft.\n\n" +
                    "It is used for .NET applications, web applications " +
                    "and game development.\n\n" +
                    "Hello World:\n" +
                    "Console.WriteLine(\"Hello, World!\");",

                WebsiteUrl =
                    "https://learn.microsoft.com/en-us/dotnet/csharp/"
            });


            // Python
            languages.Add(new ProgrammingLanguage
            {
                Name = "Python",

                Description =
                    "Simple and popular programming language",

                ImageUrl =
                    "python.png",

                InfoText =
                    "Tap the card for more information",

                Details =
                    "Python was created in 1991.\n\n" +
                    "It is used for automation, data processing, " +
                    "artificial intelligence and web development.\n\n" +
                    "Hello World:\n" +
                    "print(\"Hello, World!\")",

                WebsiteUrl =
                    "https://www.python.org/"
            });


            // JavaScript
            languages.Add(new ProgrammingLanguage
            {
                Name = "JavaScript",

                Description =
                    "One of the main languages of web development",

                ImageUrl =
                    "javascript.png",

                InfoText =
                    "Tap the card for more information",

                Details =
                    "JavaScript was created in 1995.\n\n" +
                    "It is used to make websites interactive " +
                    "and to create web applications.\n\n" +
                    "Hello World:\n" +
                    "console.log(\"Hello, World!\");",

                WebsiteUrl =
                    "https://developer.mozilla.org/en-US/docs/Web/JavaScript"
            });


            // Java
            languages.Add(new ProgrammingLanguage
            {
                Name = "Java",

                Description =
                    "Popular object-oriented programming language",

                ImageUrl =
                    "java.png",

                InfoText =
                    "Tap the card for more information",

                Details =
                    "Java was released in 1995.\n\n" +
                    "It is used for server applications, " +
                    "enterprise systems and many other applications.",

                WebsiteUrl =
                    "https://dev.java/"
            });


            // C++
            languages.Add(new ProgrammingLanguage
            {
                Name = "C++",

                Description =
                    "Fast and high-performance programming language",

                ImageUrl =
                    "cplusplus.png",

                InfoText =
                    "Tap the card for more information",

                Details =
                    "C++ was created in the 1980s.\n\n" +
                    "It is commonly used for games, operating systems " +
                    "and high-performance software.",

                WebsiteUrl =
                    "https://en.cppreference.com/"
            });
        }
    }


    // Näitab praegu valitud programmeerimiskeelt
    private void ShowCurrentLanguage()
    {
        if (languages.Count == 0)
        {
            return;
        }


        // Võtab praeguse keele
        ProgrammingLanguage language =
            languages[currentIndex];


        // Muudab logo
        LanguageImage.Source =
            language.ImageUrl;


        // Muudab nime
        NameLabel.Text =
            language.Name;


        // Muudab kirjeldust
        DescriptionLabel.Text =
            language.Description;


        // Muudab lisainfo teksti
        InfoLabel.Text =
            language.InfoText;


        // Näitab kaardi numbrit
        PositionLabel.Text =
            $"{currentIndex + 1} / {languages.Count}";


        // Esimesel kaardil Tagasi nupp ei tööta
        PreviousButton.IsEnabled =
            currentIndex > 0;


        // Viimasel kaardil Järgmine nupp ei tööta
        NextButton.IsEnabled =
            currentIndex < languages.Count - 1;
    }


    // JÄRGMINE
    private async void NextButton_Clicked(
        object sender,
        EventArgs e)
    {
        // Kui animatsioon töötab
        if (isAnimating)
        {
            return;
        }


        // Kui oleme viimasel kaardil
        if (currentIndex >= languages.Count - 1)
        {
            return;
        }


        isAnimating = true;


        // Vana kaart kaob
        await LanguageCard.FadeTo(
            0,
            120);


        // Liigub ühe kaardi edasi
        currentIndex++;


        // Näitab uut kaarti
        ShowCurrentLanguage();


        // Uus kaart alustab natuke väiksemana
        LanguageCard.Scale = 0.95;


        // Uus kaart ilmub
        await LanguageCard.FadeTo(
            1,
            180);


        // Kaart kasvab normaalseks
        await LanguageCard.ScaleTo(
            1,
            120);


        isAnimating = false;
    }


    // TAGASI
    private async void PreviousButton_Clicked(
        object sender,
        EventArgs e)
    {
        // Kui animatsioon töötab
        if (isAnimating)
        {
            return;
        }


        // Kui oleme esimesel kaardil
        if (currentIndex <= 0)
        {
            return;
        }


        isAnimating = true;


        // Vana kaart kaob
        await LanguageCard.FadeTo(
            0,
            120);


        // Liigub ühe kaardi tagasi
        currentIndex--;


        // Näitab uut kaarti
        ShowCurrentLanguage();


        // Uus kaart alustab natuke väiksemana
        LanguageCard.Scale = 0.95;


        // Uus kaart ilmub
        await LanguageCard.FadeTo(
            1,
            180);


        // Kaart kasvab normaalseks
        await LanguageCard.ScaleTo(
            1,
            120);


        isAnimating = false;
    }


    // Kaardile vajutamine
    private async void Card_Tapped(
        object sender,
        TappedEventArgs e)
    {
        if (languages.Count == 0)
        {
            return;
        }


        // Võtab praeguse keele
        ProgrammingLanguage language =
            languages[currentIndex];


        // Näitab lisainfot
        await DisplayAlert(
            language.Name,
            language.Details,
            "OK");
    }


    // Veebilehe avamine
    private async void WebsiteButton_Clicked(
        object sender,
        EventArgs e)
    {
        if (languages.Count == 0)
        {
            return;
        }


        // Võtab praeguse keele
        ProgrammingLanguage language =
            languages[currentIndex];


        // Kontrollib linki
        if (string.IsNullOrWhiteSpace(language.WebsiteUrl))
        {
            return;
        }


        try
        {
            // Avab lingi brauseris
            await Launcher.Default.OpenAsync(
                language.WebsiteUrl);
        }
        catch
        {
            if (isEstonian)
            {
                await DisplayAlert(
                    "Viga",
                    "Veebilehte ei saanud avada.",
                    "OK");
            }
            else
            {
                await DisplayAlert(
                    "Error",
                    "The website could not be opened.",
                    "OK");
            }
        }
    }


    // Keele muutmine
    private async void LanguagePicker_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        // EESTI
        if (LanguagePicker.SelectedIndex == 0)
        {
            isEstonian = true;


            // Näitab Eesti lippu
            CountryFlag.Source =
                "estonia.png";


            // Muudab tekstid eesti keelde
            TitleLabel.Text =
                "Programmeerimiskeeled";

            LanguageLabel.Text =
                "Keel:";

            WebsiteButton.Text =
                "Ava veebileht";

            PreviousButton.Text =
                "Tagasi";

            NextButton.Text =
                "Järgmine";
        }


        // ENGLISH
        else if (LanguagePicker.SelectedIndex == 1)
        {
            isEstonian = false;


            // Näitab Suurbritannia lippu
            CountryFlag.Source =
                "uk.png";


            // Muudab tekstid inglise keelde
            TitleLabel.Text =
                "Programming Languages";

            LanguageLabel.Text =
                "Language:";

            WebsiteButton.Text =
                "Open website";

            PreviousButton.Text =
                "Previous";

            NextButton.Text =
                "Next";
        }


        else
        {
            return;
        }


        // Kaart kaob
        await LanguageCard.FadeTo(
            0,
            100);


        // Laeb valitud keele andmed
        LoadLanguages();


        // Läheb esimesele kaardile
        currentIndex = 0;


        // Näitab esimest kaarti
        ShowCurrentLanguage();


        // Kaart ilmub tagasi
        await LanguageCard.FadeTo(
            1,
            180);
    }
}


// Programmeerimiskeele andmemudel
public class ProgrammingLanguage
{
    // Nimi
    public string Name { get; set; } = "";


    // Kirjeldus
    public string Description { get; set; } = "";


    // Pildi failinimi
    public string ImageUrl { get; set; } = "";


    // Juhendtekst
    public string InfoText { get; set; } = "";


    // Lisainfo
    public string Details { get; set; } = "";


    // Veebilehe link
    public string WebsiteUrl { get; set; } = "";
}
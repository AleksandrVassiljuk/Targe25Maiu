using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Targe25Maiu;

public partial class FriendsPage : ContentPage
{
    // Ühe sõbra andmed
    public class Sober
    {
        public string Nimi { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefon { get; set; } = "";
        public string Kirjeldus { get; set; } = "";
        public string Pilt { get; set; } = "";
    }


    // Kõik kontaktid
    private readonly ObservableCollection<Sober> Sobrad;


    // Hetkel valitud kontakt
    private Sober? valitudSober;


    // Uue pildi faili asukoht
    private string valitudPildiTee = "";


    // Juhuslikud tervitused
    private readonly string[] greetings =
    {
        "Palju õnne ja kõike head!",
        "Soovin sulle imelist päeva!",
        "Palju rõõmu ja häid hetki!",
        "Parimad soovid sulle!",
        "Olgu sinu päev täis rõõmu!"
    };


    // Lehe käivitamine
    public FriendsPage()
    {
        InitializeComponent();


        // Alguses olevad näidiskontaktid
        Sobrad = new ObservableCollection<Sober>
        {
            new Sober
            {
                Nimi = "Toomas",
                Email = "toomas@gmail.com",
                Telefon = "55512345",
                Kirjeldus = "Sõber",
                Pilt = "toomas.jpg"
            },

            new Sober
            {
                Nimi = "Karl",
                Email = "karl@gmail.com",
                Telefon = "55567890",
                Kirjeldus = "Koolikaaslane",
                Pilt = "dotnet_bot.png"
            }
        };


        // Ühendame nimekirja ListView-ga
        FriendsList.ItemsSource = Sobrad;
    }


    // TEE FOTO KAAMERAGA
    private async void TakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Kontrollime, kas kaamera on olemas
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync(
                    "Viga",
                    "Kaamera pole selles seadmes toetatud.",
                    "OK");

                return;
            }


            // Avame kaamera
            FileResult? photo =
                await MediaPicker.Default.CapturePhotoAsync();


            // Kui foto tehti
            if (photo != null)
            {
                // Salvestame foto rakenduse kausta
                valitudPildiTee =
                    await SalvestaPilt(photo);


                // Näitame fotot
                FriendImage.Source =
                    ImageSource.FromFile(valitudPildiTee);


                PiltLabel.Text =
                    "Foto tehtud";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "Fotot ei saanud teha.\n" + ex.Message,
                "OK");
        }
    }


    // VALI FOTO GALERIIST
    private async void ChoosePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Avame galerii
            FileResult? photo =
                await MediaPicker.Default.PickPhotoAsync();


            // Kui kasutaja valis pildi
            if (photo != null)
            {
                // Salvestame pildi rakenduse kausta
                valitudPildiTee =
                    await SalvestaPilt(photo);


                // Näitame pilti
                FriendImage.Source =
                    ImageSource.FromFile(valitudPildiTee);


                // Näitame faili nime
                PiltLabel.Text =
                    "Valitud pilt: " + photo.FileName;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "Pilti ei saanud valida.\n" + ex.Message,
                "OK");
        }
    }


    // SALVESTAB PILDI RAKENDUSE KAUSTA
    private async Task<string> SalvestaPilt(FileResult photo)
    {
        // Teeme pildile unikaalse nime
        string fileName =
            Guid.NewGuid().ToString() +
            Path.GetExtension(photo.FileName);


        // Pildi uus asukoht
        string newFile =
            Path.Combine(
                FileSystem.AppDataDirectory,
                fileName);


        // Avame algse pildi
        using Stream sourceStream =
            await photo.OpenReadAsync();


        // Loome uue faili
        using FileStream localFileStream =
            File.OpenWrite(newFile);


        // Kopeerime pildi
        await sourceStream.CopyToAsync(
            localFileStream);


        // Tagastame pildi asukoha
        return newFile;
    }


    // LISA UUS KONTAKT
    private async void AddFriend_Clicked(object sender, EventArgs e)
    {
        // Nimi peab olema sisestatud
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta nimi.",
                "OK");

            return;
        }


        // Email peab olema sisestatud
        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta email.",
                "OK");

            return;
        }


        // Telefon peab olema sisestatud
        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta telefoninumber.",
                "OK");

            return;
        }


        // Kui pilti ei valitud, kasutame vaikimisi pilti
        string pilt = "dotnet_bot.png";


        // Kui pilt valiti, kasutame valitud pilti
        if (!string.IsNullOrWhiteSpace(valitudPildiTee))
        {
            pilt = valitudPildiTee;
        }


        // Loome uue kontakti
        Sober uusSober = new Sober
        {
            Nimi = NameEntry.Text,
            Email = EmailEntry.Text,
            Telefon = PhoneEntry.Text,
            Kirjeldus = DescriptionEntry.Text ?? "",
            Pilt = pilt
        };


        // Lisame kontakti nimekirja
        Sobrad.Add(uusSober);


        // Tühjendame väljad
        TyhjendaValjad();


        await DisplayAlertAsync(
            "Valmis",
            "Kontakt lisatud.",
            "OK");
    }


    // MUUDA KONTAKTI
    private async void EditFriend_Clicked(object sender, EventArgs e)
    {
        // Kõigepealt peab kontakt olema valitud
        if (valitudSober == null)
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kõigepealt nimekirjast kontakt.",
                "OK");

            return;
        }


        // Kontrollime nime
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta nimi.",
                "OK");

            return;
        }


        // Kontrollime emaili
        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta email.",
                "OK");

            return;
        }


        // Kontrollime telefoni
        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta telefoninumber.",
                "OK");

            return;
        }


        // Jätame vana pildi alles
        string uusPilt =
            valitudSober.Pilt;


        // Kui valiti uus pilt, kasutame seda
        if (!string.IsNullOrWhiteSpace(valitudPildiTee))
        {
            uusPilt =
                valitudPildiTee;
        }


        // Leiame kontakti asukoha nimekirjas
        int index =
            Sobrad.IndexOf(valitudSober);


        // Asendame vana kontakti uute andmetega
        if (index >= 0)
        {
            Sobrad[index] = new Sober
            {
                Nimi = NameEntry.Text,
                Email = EmailEntry.Text,
                Telefon = PhoneEntry.Text,
                Kirjeldus = DescriptionEntry.Text ?? "",
                Pilt = uusPilt
            };
        }


        // Eemaldame valiku
        valitudSober = null;

        FriendsList.SelectedItem = null;


        // Tühjendame väljad
        TyhjendaValjad();


        await DisplayAlertAsync(
            "Valmis",
            "Kontakt muudetud.",
            "OK");
    }


    // KUSTUTA KONTAKT
    private async void DeleteFriend_Clicked(object sender, EventArgs e)
    {
        // Kontrollime, kas kontakt on valitud
        if (valitudSober == null)
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kõigepealt kontakt.",
                "OK");

            return;
        }


        // Küsime kinnitust
        bool vastus =
            await DisplayAlertAsync(
                "Kustutamine",
                $"Kas soovid kustutada {valitudSober.Nimi}?",
                "Jah",
                "Ei");


        // Kui kasutaja vajutas Jah
        if (vastus)
        {
            // Kustutame kontakti
            Sobrad.Remove(valitudSober);


            // Eemaldame valiku
            valitudSober = null;

            FriendsList.SelectedItem = null;


            // Tühjendame väljad
            TyhjendaValjad();
        }
    }


    // KUI VAJUTAME KONTAKTILE
    private void Friend_ItemTapped(
        object sender,
        ItemTappedEventArgs e)
    {
        // Võtame valitud kontakti
        Sober? sober =
            e.Item as Sober;


        if (sober != null)
        {
            // Salvestame valitud kontakti
            valitudSober = sober;


            // Näitame nime
            NameEntry.Text =
                sober.Nimi;


            // Näitame emaili
            EmailEntry.Text =
                sober.Email;


            // Näitame telefoni
            PhoneEntry.Text =
                sober.Telefon;


            // Näitame kirjeldust
            DescriptionEntry.Text =
                sober.Kirjeldus;


            // Näitame pilti
            FriendImage.Source =
                sober.Pilt;


            // Näitame pildi nime
            PiltLabel.Text =
                "Praegune pilt: " +
                Path.GetFileName(sober.Pilt);


            // Uut pilti pole veel valitud
            valitudPildiTee = "";
        }
    }


    // HELISTA KONTAKTILE
    private async void Call_Clicked(object sender, EventArgs e)
    {
        // Loeme telefoninumbri
        string phone =
            PhoneEntry.Text ?? "";


        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kontakt või sisesta telefoninumber.",
                "OK");

            return;
        }


        try
        {
            // Avame helistamise rakenduse
            PhoneDialer.Default.Open(phone);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "Helistamist ei saanud avada.\n" + ex.Message,
                "OK");
        }
    }


    // SAADA SMS
    private async void SendSms_Clicked(object sender, EventArgs e)
    {
        // Telefoninumber
        string phone =
            PhoneEntry.Text ?? "";


        // Sõnum
        string message =
            MessageEntry.Text ?? "";


        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kontakt või sisesta telefon.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(message))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta sõnum.",
                "OK");

            return;
        }


        try
        {
            // Loome SMS-i
            SmsMessage sms =
                new SmsMessage(
                    message,
                    phone);


            // Kontrollime, kas SMS rakendus on olemas
            if (Sms.Default.IsComposeSupported)
            {
                // Avame SMS rakenduse
                await Sms.Default.ComposeAsync(sms);
            }
            else
            {
                await DisplayAlertAsync(
                    "Viga",
                    "SMS pole selles seadmes toetatud.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "SMS-i ei saanud avada.\n" + ex.Message,
                "OK");
        }
    }


    // SAADA EMAIL
    private async void SendEmail_Clicked(object sender, EventArgs e)
    {
        // Emaili aadress
        string emailAddress =
            EmailEntry.Text ?? "";


        // Sõnum
        string message =
            MessageEntry.Text ?? "";


        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kontakt või sisesta email.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(message))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta sõnum.",
                "OK");

            return;
        }


        try
        {
            // Loome emaili
            EmailMessage email =
                new EmailMessage
                {
                    Subject = "Tervitus",

                    Body = message,

                    BodyFormat =
                        EmailBodyFormat.PlainText,

                    To = new List<string>
                    {
                        emailAddress
                    }
                };


            // Kontrollime, kas emaili rakendus on olemas
            if (Email.Default.IsComposeSupported)
            {
                // Avame emaili rakenduse
                await Email.Default.ComposeAsync(email);
            }
            else
            {
                await DisplayAlertAsync(
                    "Viga",
                    "E-mail pole selles seadmes toetatud.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "E-maili ei saanud avada.\n" + ex.Message,
                "OK");
        }
    }


    // JUHUSLIK TERVITUS
    private async void Greeting_Clicked(object sender, EventArgs e)
    {
        // Loome juhusliku numbri
        Random random =
            new Random();


        // Valime ühe tervituse
        string greeting =
            greetings[
                random.Next(greetings.Length)
            ];


        // Paneme tervituse sõnumikasti
        MessageEntry.Text =
            greeting;


        // Küsime, kuidas saata
        string? choice =
            await DisplayActionSheetAsync(
                "Kuidas soovid tervituse saata?",
                "Tühista",
                null,
                "SMS",
                "E-mail");


        // Saada SMS-iga
        if (choice == "SMS")
        {
            SendSms_Clicked(
                sender,
                e);
        }

        // Saada emailiga
        else if (choice == "E-mail")
        {
            SendEmail_Clicked(
                sender,
                e);
        }
    }


    // TÜHJENDAB VÄLJAD
    private void TyhjendaValjad()
    {
        NameEntry.Text = "";

        EmailEntry.Text = "";

        PhoneEntry.Text = "";

        DescriptionEntry.Text = "";

        MessageEntry.Text = "";


        // Vaikimisi pilt
        FriendImage.Source =
            "dotnet_bot.png";


        // Uut pilti pole valitud
        valitudPildiTee = "";


        PiltLabel.Text =
            "Pilti pole valitud";
    }
}
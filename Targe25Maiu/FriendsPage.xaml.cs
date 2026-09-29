using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Media;

namespace Targe25Maiu;

public partial class FriendsPage : ContentPage
{
    // Juhuslike tervituste nimekiri
    private readonly string[] greetings =
    {
        "Palju õnne ja kõike head!",
        "Soovin sulle imelist päeva!",
        "Palju rõõmu ja häid hetki!",
        "Parimad soovid sulle!",
        "Olgu sinu päev täis rõõmu!"
    };

    public FriendsPage()
    {
        InitializeComponent();
    }


    // PILDISTAB SÕPRA TELEFONI KAAMERAGA
    private async void TakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Kontrollime, kas telefon toetab kaameraga pildistamist
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync(
                    "Viga",
                    "Kaamera ei ole selles seadmes toetatud.",
                    "OK");

                return;
            }

            // Avame telefoni kaamera
            FileResult? photo =
                await MediaPicker.Default.CapturePhotoAsync();

            // Kui kasutaja tegi pildi
            if (photo != null)
            {
                // Avame pildi
                Stream stream =
                    await photo.OpenReadAsync();

                // Näitame pilti Image elemendis
                FriendImage.Source =
                    ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                ex.Message,
                "OK");
        }
    }


    // HELISTAMINE
    private async void Call_Clicked(object sender, EventArgs e)
    {
        // Loeme telefoninumbri tabelist
        string phone = PhoneEntry.Text ?? "";

        // Kontrollime, kas telefoninumber on sisestatud
        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta telefoninumber.",
                "OK");

            return;
        }

        try
        {
            // Avame telefoni helistamise rakenduse
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


    // SMS SAATMINE
    private async void SendSms_Clicked(object sender, EventArgs e)
    {
        // Loeme telefoninumbri tabelist
        string phone = PhoneEntry.Text ?? "";

        // Loeme sõnumi tabelist
        string message = MessageEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta telefoninumber.",
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
            // Loome SMS sõnumi
            SmsMessage sms =
                new SmsMessage(message, phone);

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
                    "SMS saatmine pole selles seadmes toetatud.",
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


    // EMAILI SAATMINE
    private async void SendEmail_Clicked(object sender, EventArgs e)
    {
        // Loeme emaili tabelist
        string emailAddress = EmailEntry.Text ?? "";

        // Loeme sõnumi tabelist
        string message = MessageEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta e-mail.",
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
            // Loome uue emaili
            EmailMessage email = new EmailMessage
            {
                Subject = "Tervitus",
                Body = message,
                BodyFormat = EmailBodyFormat.PlainText,

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
                    "E-maili saatmine pole selles seadmes toetatud.",
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
        // Valime nimekirjast juhusliku tervituse
        Random random = new Random();

        string greeting =
            greetings[random.Next(greetings.Length)];

        // Paneme tervituse sõnumi lahtrisse
        MessageEntry.Text = greeting;

        // Küsime kasutajalt, kuidas ta soovib tervituse saata
        string? choice =
            await DisplayActionSheetAsync(
                "Kuidas soovid tervituse saata?",
                "Tühista",
                null,
                "SMS",
                "E-mail");

        // SMS
        if (choice == "SMS")
        {
            SendSms_Clicked(sender, e);
        }

        // EMAIL
        else if (choice == "E-mail")
        {
            SendEmail_Clicked(sender, e);
        }
    }
}
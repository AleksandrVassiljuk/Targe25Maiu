namespace Targe25Maiu;

public partial class FriendsPage : ContentPage
{
    public FriendsPage()
    {
        InitializeComponent();
    }

    // FOTO TEGEMINE
    private async void TakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync(
                    "Viga",
                    "Kaamera pole selles seadmes toetatud.",
                    "OK");

                return;
            }

            FileResult? photo =
                await MediaPicker.Default.CapturePhotoAsync();

            if (photo == null)
                return;

            byte[] imageBytes;

            using (Stream stream = await photo.OpenReadAsync())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            FriendImage.Source =
                ImageSource.FromStream(() =>
                    new MemoryStream(imageBytes));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "Foto tegemine ebaõnnestus: " + ex.Message,
                "OK");
        }
    }


    // HELISTAMINE
    private async void Call_Clicked(object sender, EventArgs e)
    {
        string phone = PhoneEntry.Text ?? "";

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
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(phone);
            }
            else
            {
                await DisplayAlertAsync(
                    "Viga",
                    "Helistamine pole selles seadmes toetatud.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "Helistamine ebaõnnestus: " + ex.Message,
                "OK");
        }
    }


    // SMS SAATMINE
    private async void SendSms_Clicked(object sender, EventArgs e)
    {
        string phone = PhoneEntry.Text ?? "";
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
            if (Sms.Default.IsComposeSupported)
            {
                SmsMessage sms =
                    new SmsMessage(message, phone);

                await Sms.Default.ComposeAsync(sms);
            }
            else
            {
                await DisplayAlertAsync(
                    "Viga",
                    "SMS-i saatmine pole selles seadmes toetatud.",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Viga",
                "SMS-i avamine ebaõnnestus: " + ex.Message,
                "OK");
        }
    }


    // EMAILI SAATMINE
    private async void SendEmail_Clicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text ?? "";
        string message = MessageEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta e-maili aadress.",
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
            if (Email.Default.IsComposeSupported)
            {
                EmailMessage emailMessage =
                    new EmailMessage
                    {
                        Subject = "Sõnum sõbrale",
                        Body = message,
                        BodyFormat = EmailBodyFormat.PlainText,
                        To = new List<string>
                        {
                            email
                        }
                    };

                await Email.Default.ComposeAsync(emailMessage);
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
                "E-maili avamine ebaõnnestus: " + ex.Message,
                "OK");
        }
    }
}
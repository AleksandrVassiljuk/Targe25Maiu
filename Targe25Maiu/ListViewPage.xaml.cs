using System.Collections.ObjectModel;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Targe25Maiu;

public partial class ListViewPage : ContentPage
{
    // Ühe telefoni andmed
    public class Telefon
    {
        // Telefoni nimetus
        public string Nimetus { get; set; } = "";

        // Telefoni tootja
        public string Tootja { get; set; } = "";

        // Telefoni hind
        public int Hind { get; set; }

        // Telefoni pildi asukoht
        public string Pilt { get; set; } = "";
    }


    // Telefonide nimekiri
    private readonly ObservableCollection<Telefon> Telefonid;


    // Hetkel nimekirjast valitud telefon
    private Telefon? valitudTelefon;


    // Galeriist valitud pildi asukoht
    private string valitudPildiTee = "";


    // Lehe käivitamine
    public ListViewPage()
    {
        InitializeComponent();


        // Lisame alguses telefonid nimekirja
        Telefonid = new ObservableCollection<Telefon>
        {
            new Telefon
            {
                Nimetus = "iPhone 16 Pro Max",
                Tootja = "Apple",
                Hind = 1199,
                Pilt = "iphone16promax.jpg"
            },

            new Telefon
            {
                Nimetus = "Galaxy S25 Ultra",
                Tootja = "Samsung",
                Hind = 1399,
                Pilt = "galaxys25ultra.jpg"
            },

            new Telefon
            {
                Nimetus = "Xiaomi 15 Ultra",
                Tootja = "Xiaomi",
                Hind = 1299,
                Pilt = "xiaomi15ultra.jpg"
            },

            new Telefon
            {
                Nimetus = "OnePlus 13",
                Tootja = "OnePlus",
                Hind = 999,
                Pilt = "oneplus13512gb.jpg"
            },

            new Telefon
            {
                Nimetus = "Pixel 9 Pro",
                Tootja = "Google",
                Hind = 1099,
                Pilt = "pixel9pro.jpg"
            }
        };


        // Ühendame telefonide nimekirja ListView-ga
        TelefonideList.ItemsSource = Telefonid;
    }


    // VALIB PILDI GALERIIST
    private async void ValiPilt_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Avame telefoni galerii
            FileResult? pilt =
                await MediaPicker.Default.PickPhotoAsync();


            // Kui kasutaja valis pildi
            if (pilt != null)
            {
                // Teeme pildile uue unikaalse nime
                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(pilt.FileName);


                // Määrame, kuhu pilt rakenduses salvestatakse
                string newFile =
                    Path.Combine(
                        FileSystem.AppDataDirectory,
                        fileName);


                // Avame valitud pildi
                using Stream sourceStream =
                    await pilt.OpenReadAsync();


                // Loome rakenduse kausta uue pildifaili
                using FileStream localFileStream =
                    File.OpenWrite(newFile);


                // Kopeerime pildi rakenduse kausta
                await sourceStream.CopyToAsync(
                    localFileStream);


                // Salvestame pildi asukoha
                valitudPildiTee = newFile;


                // Näitame kasutajale pildi nime
                PiltLabel.Text =
                    "Valitud pilt: " + pilt.FileName;
            }
        }
        catch (Exception ex)
        {
            // Kui galerii avamisel tekib viga
            await DisplayAlertAsync(
                "Viga",
                "Pilti ei saanud valida.\n" +
                ex.Message,
                "OK");
        }
    }


    // LISAB UUE TELEFONI
    private async void Lisa_Clicked(object sender, EventArgs e)
    {
        // Kontrollime nimetust
        if (string.IsNullOrWhiteSpace(NimetusEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta telefoni nimetus.",
                "OK");

            return;
        }


        // Kontrollime tootjat
        if (string.IsNullOrWhiteSpace(TootjaEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta tootja.",
                "OK");

            return;
        }


        // Kontrollime hinda
        if (!int.TryParse(
            HindEntry.Text,
            out int hind))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta õige hind.",
                "OK");

            return;
        }


        // Kui pilti pole valitud,
        // kasutame vaikimisi pilti
        string pilt = "dotnet_bot.png";


        // Kui kasutaja valis galerii kaudu pildi,
        // kasutame seda pilti
        if (!string.IsNullOrWhiteSpace(valitudPildiTee))
        {
            pilt = valitudPildiTee;
        }


        // Loome uue telefoni
        Telefon uusTelefon = new Telefon
        {
            Nimetus = NimetusEntry.Text,
            Tootja = TootjaEntry.Text,
            Hind = hind,
            Pilt = pilt
        };


        // Lisame telefoni nimekirja
        Telefonid.Add(uusTelefon);


        // Tühjendame sisestusväljad
        TyhjendaValjad();


        await DisplayAlertAsync(
            "Valmis",
            "Telefon lisatud.",
            "OK");
    }


    // MUUDAB VALITUD TELEFONI
    private async void Muuda_Clicked(object sender, EventArgs e)
    {
        // Kontrollime, kas telefon on valitud
        if (valitudTelefon == null)
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kõigepealt nimekirjast telefon.",
                "OK");

            return;
        }


        // Kontrollime nimetust
        if (string.IsNullOrWhiteSpace(NimetusEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta telefoni nimetus.",
                "OK");

            return;
        }


        // Kontrollime tootjat
        if (string.IsNullOrWhiteSpace(TootjaEntry.Text))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta tootja.",
                "OK");

            return;
        }


        // Kontrollime hinda
        if (!int.TryParse(
            HindEntry.Text,
            out int hind))
        {
            await DisplayAlertAsync(
                "Viga",
                "Sisesta õige hind.",
                "OK");

            return;
        }


        // Jätame alguses vana pildi alles
        string uusPilt =
            valitudTelefon.Pilt;


        // Kui kasutaja valis uue pildi,
        // kasutame uut pilti
        if (!string.IsNullOrWhiteSpace(valitudPildiTee))
        {
            uusPilt =
                valitudPildiTee;
        }


        // Leiame telefoni asukoha nimekirjas
        int index =
            Telefonid.IndexOf(valitudTelefon);


        // Kui telefon leiti
        if (index >= 0)
        {
            // Asendame vana telefoni uute andmetega
            Telefonid[index] = new Telefon
            {
                Nimetus = NimetusEntry.Text,
                Tootja = TootjaEntry.Text,
                Hind = hind,
                Pilt = uusPilt
            };
        }


        // Eemaldame valitud telefoni
        valitudTelefon = null;


        // Eemaldame ListView valiku
        TelefonideList.SelectedItem = null;


        // Tühjendame väljad
        TyhjendaValjad();


        await DisplayAlertAsync(
            "Valmis",
            "Telefoni andmed on muudetud.",
            "OK");
    }


    // KUSTUTAB VALITUD TELEFONI
    private async void Kustuta_Clicked(object sender, EventArgs e)
    {
        // Kontrollime, kas telefon on valitud
        if (valitudTelefon == null)
        {
            await DisplayAlertAsync(
                "Viga",
                "Vali kõigepealt nimekirjast telefon.",
                "OK");

            return;
        }


        // Küsime enne kustutamist kinnitust
        bool vastus =
            await DisplayAlertAsync(
                "Kustutamine",
                $"Kas soovid kustutada {valitudTelefon.Nimetus}?",
                "Jah",
                "Ei");


        // Kui kasutaja valis Jah
        if (vastus)
        {
            // Kustutame telefoni nimekirjast
            Telefonid.Remove(valitudTelefon);


            // Eemaldame valiku
            valitudTelefon = null;

            TelefonideList.SelectedItem = null;


            // Tühjendame väljad
            TyhjendaValjad();
        }
    }


    // KÄIVITUB, KUI VAJUTAME NIMEKIRJAS TELEFONILE
    private void Telefon_ItemTapped(
        object sender,
        ItemTappedEventArgs e)
    {
        // Võtame vajutatud telefoni
        Telefon? telefon =
            e.Item as Telefon;


        // Kui telefon leiti
        if (telefon != null)
        {
            // Salvestame valitud telefoni
            valitudTelefon = telefon;


            // Paneme telefoni nimetuse sisestusvälja
            NimetusEntry.Text =
                telefon.Nimetus;


            // Paneme tootja sisestusvälja
            TootjaEntry.Text =
                telefon.Tootja;


            // Paneme hinna sisestusvälja
            HindEntry.Text =
                telefon.Hind.ToString();


            // Näitame, milline pilt telefonil praegu on
            PiltLabel.Text =
                "Praegune pilt: " +
                Path.GetFileName(telefon.Pilt);


            // Uut pilti pole veel valitud
            valitudPildiTee = "";
        }
    }


    // TÜHJENDAB KÕIK SISESTUSVÄLJAD
    private void TyhjendaValjad()
    {
        // Tühjendame nimetuse
        NimetusEntry.Text = "";


        // Tühjendame tootja
        TootjaEntry.Text = "";


        // Tühjendame hinna
        HindEntry.Text = "";


        // Eemaldame valitud pildi
        valitudPildiTee = "";


        // Muudame pildi teksti tagasi
        PiltLabel.Text =
            "Pilti pole valitud";
    }
}
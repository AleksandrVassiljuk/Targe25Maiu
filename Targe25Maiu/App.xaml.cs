using Microsoft.Extensions.DependencyInjection;

namespace Targe25Maiu
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Loome esimese lehe (nt StartPage)
            var startPage = new StartPage();

            // Pakime selle NavigationPage sisse, et tekiks ülemine riba ja "tagasi" nupp
            var navPage = new NavigationPage(startPage)
            {
                BarBackgroundColor = Colors.Blue, // Saab stiilida riba
                BarTextColor = Colors.White
            };

            return new Window(navPage);
        }
    };

}
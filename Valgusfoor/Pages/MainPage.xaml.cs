using Valgusfoor.Models;
using Valgusfoor.PageModels;

namespace Valgusfoor.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
using Microsoft.Maui.Controls;

namespace steganography
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddImagePage());
        }
    }
}

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace steganography
{
    public partial class AddImagePage : ContentPage
    {
        public AddImagePage()
        {
            InitializeComponent();
        }

        private async void OnAddPhotoClicked(object sender, EventArgs e)
        {
            try
            {
                // Pick a photo from the gallery
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select a photo",
                    FileTypes = FilePickerFileType.Images,
                });

                if (result != null)
                {
                    // Assuming you have an Image control in your XAML to display the selected photo
                    // For example: <Image x:Name="SelectedPhoto" />
                    SelectedPhoto.Source = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if any
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}

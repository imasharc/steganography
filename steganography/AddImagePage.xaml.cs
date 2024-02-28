using SkiaSharp;

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
                // Check and request the necessary permission
                bool hasPermission = await CheckAndRequestStoragePermissionAsync();

                if (!hasPermission)
                {
                    // Inform the user that the permission is required
                    await DisplayAlert("Permission Required", "This app needs storage permission to access photos.", "OK");
                    return;
                }

                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select a photo",
                    FileTypes = FilePickerFileType.Images,
                });

                if (result != null)
                {
                    // Load the selected image into memory
                    using (Stream stream = await result.OpenReadAsync())
                    using (SKBitmap originalBitmap = SKBitmap.Decode(stream))
                    {
                        // Convert the bitmap to a PNG format and set it as the ImageSource
                        SKImage image = SKImage.FromBitmap(originalBitmap);
                        SKData data = image.Encode(SKEncodedImageFormat.Png, 100); // 100 is the quality level for PNG, which is lossless
                        MemoryStream pngStream = new MemoryStream(data.ToArray());
                        SelectedPhoto.Source = ImageSource.FromStream(() => pngStream);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task<bool> CheckAndRequestStoragePermissionAsync()
        {
            int majorVersion = DeviceInfo.Version.Major;
            Console.WriteLine($"The device's major version number is: {majorVersion}");

            // Check the SDK version first
            if (majorVersion < 30)
            {
                // Below Android 10 (API level 30), check for read external storage permission
                var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
                if (status == PermissionStatus.Denied)
                {
                    bool isRationaleAccepted = await DisplayAlert(
                        "Permission Needed",
                        "This app needs permission to access images to upload them into the app.",
                        "OK", "Cancel");

                    if (isRationaleAccepted)
                    {
                        AppInfo.ShowSettingsUI();
                    }
                    return false;
                }

                if (status != PermissionStatus.Granted)
                {
                    // The permission has not been asked for yet, so request it.
                    status = await Permissions.RequestAsync<Permissions.Photos>();
                }

                return status == PermissionStatus.Granted;
            }
            else
            {
                // For Android 10 (API level 30) and above, permissions might not be needed due to scoped storage,
                // but if you do need to access files in a specific way that requires permissions, handle that here.
                // As of now, there is no `READ_MEDIA_IMAGES` permission, so this part should return true.
                return true;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Test10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private async void GetPass()
        {
            Windows.Storage.StorageFolder curFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile pass = await curFolder.CreateFileAsync("pass", Windows.Storage.CreationCollisionOption.OpenIfExists);
            Application.Current.Resources["UserPassFile"] = pass;

            Application.Current.Resources["Password"] = await Windows.Storage.FileIO.ReadTextAsync(pass);
            if (Application.Current.Resources["Password"] == null || Application.Current.Resources["Password"].ToString() == "")
            {
                Frame.Navigate(typeof(Settings));
            }

            if (!((bool)Application.Current.Resources["LoginRequired"]))
            {
               Frame.Navigate(typeof(Menu));
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
           
            GetPass();
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.HideAsync();
            }
            
        }

        private void OnPassSelect(object sender, RoutedEventArgs e)
        {
            this.passwordBox.Password = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(this.passwordBox.Password == Application.Current.Resources["Password"].ToString())
            {
                Frame.Navigate(typeof(Menu));
            }
            else
            {
                var passError = new Windows.UI.Popups.MessageDialog("Podane hasło jest niepoprawne.","Błąd");
                passError.ShowAsync();
            }
        }
    }

    
}

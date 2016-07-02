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
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Test10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            if(Application.Current.Resources["Password"].ToString() == null || Application.Current.Resources["Password"].ToString() == "")
            {
                passChange_Click(this.passChange, null);
            }
            this.loginRequired.IsOn = (bool)Application.Current.Resources["LoginRequired"];
        }

        private async void SaveSettings()
        {
            string settingsText="";
            Application.Current.Resources["LoginRequired"] = this.loginRequired.IsOn;
            settingsText += "LoginRequired " + this.loginRequired.IsOn.ToString()+"\n";
            await Windows.Storage.FileIO.WriteTextAsync((Windows.Storage.StorageFile)Application.Current.Resources["SettingsFile"], settingsText);
        }

        private void passChange_Click(object sender, RoutedEventArgs e)
        {
            if(passChangePop.IsOpen)
            {
                passChangePop.IsOpen = false;
                ((Button)sender).BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
            }
            else
            {
                passChangePop.IsOpen = true;
                ((Button)sender).BorderBrush = new SolidColorBrush(Windows.UI.Colors.WhiteSmoke);
            }
        }

        private void saveChangedPass(object sender, RoutedEventArgs e)
        {
            if ((Windows.Storage.StorageFile)Application.Current.Resources["UserPassFile"] != null)
            {
                if (this.pass.Password == this.passConf.Password)
                {
                    SavePass(this.pass.Password);
                    new MessageDialog("Hasło zapisane pomyślnie.", "Informacja").ShowAsync();
                    passChangePop.IsOpen = false;
                    this.passChange.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);
                }
                else
                {
                    var passError = new Windows.UI.Popups.MessageDialog("Podane hasła nie zgadzają się.", "Błąd");
                    passError.ShowAsync();
                }
            }

            if (Application.Current.Resources["Password"].ToString() == null || Application.Current.Resources["Password"].ToString() == "")
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        private async void SavePass(string pass)
        {
            await Windows.Storage.FileIO.WriteTextAsync((Windows.Storage.StorageFile)Application.Current.Resources["UserPassFile"], pass);
        }

        private void loginRequired_Toggled(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }
    }
}

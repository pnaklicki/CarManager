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
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Background;
using ZXing.Mobile;
using ZXing;
using ZXing.Aztec;
using Windows.Media.Capture;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Test10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CarAdd : Page
    {      
        public CarAdd()
        {
            this.InitializeComponent();
        }

        private async void saveCarBtn_Click(object sender, RoutedEventArgs e)
        {

            if (!(new Regex(@"^[0-9]+$").Match(this.distance.Text).Success) || !(new Regex(@"^[0-9]{4}$").Match(this.year.Text).Success) || this.carName.Text == "" || this.year.Text == "" || this.manufacturer.SelectedItem == null || this.distance.Text == "" || this.numbers.Text == "")
            {
                await new MessageDialog("Podane dane są nieprawidłowe.", "Błąd").ShowAsync();
            }
            else if (!(new Regex(@"^[^ ].+[^ ]$").Match(this.carName.Text).Success))
            {
                await new MessageDialog("Model auta musi zawierać minimum 3 znaki oraz pierwszy i ostatni znak nie może być spacją", "Błąd").ShowAsync();
            }
            else
            {
                if (App.CarList.Count == 0)
                {
                    App.CarList.Add(new Car(this.carName.Text, ((ComboBoxItem)this.manufacturer.SelectedItem).Content.ToString(), this.year.Text, Convert.ToInt32(this.distance.Text), Convert.ToInt32(this.volume.Text), Convert.ToInt32(this.weight.Text), this.numbers.Text));
                }
                else
                {
                    if (App.CarList.Where(m => m.Name == this.carName.Text).Count() == 0)
                    {
                        App.CarList.Add(new Car(this.carName.Text, ((ComboBoxItem)this.manufacturer.SelectedItem).Content.ToString(), this.year.Text, Convert.ToInt32(this.distance.Text), Convert.ToInt32(this.volume.Text), Convert.ToInt32(this.weight.Text), this.numbers.Text));
                    }
                    else
                    {
                        await new MessageDialog("Podane auto już istnieje!", "Błąd").ShowAsync();
                    }
                }
                Frame.GoBack();
            }
        }

        private void manufacturer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //ObservableCollection<string> manufacturersText = new ObservableCollection<string>() { "Volkswagen", "Audi", "Opel", "Honda", "Chrysler", "Citroen", "Mercedes" };
            //this.manufacturer.ItemsSource = manufacturersText;
        }
    }
}

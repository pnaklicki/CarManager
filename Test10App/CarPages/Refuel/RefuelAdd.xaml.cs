using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Test10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RefuelAdd : Page
    {
        private Car currentCar { get; set; }

        public RefuelAdd()
        {
            this.InitializeComponent();
            this.currentCar = ((List<Car>)Application.Current.Resources["CarList"]).Where(m => m.Name == Application.Current.Resources["CarDetails"].ToString()).Single();
            this.refuelDate.Date = DateTime.Now;
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(this.distance.Text) <= this.currentCar.DrivenDistance)
            {
                await new MessageDialog("Przebieg nie może być mniejszy niż przebieg całkowity samochodu", "Błąd").ShowAsync();
            }
            else
            {
                this.currentCar.Refuels.Add(new Refuel(Convert.ToDouble(this.volume.Text), Convert.ToDouble(this.pricePerLitre.Text), Convert.ToInt32(this.distance.Text), this.fullCheck.IsChecked.Value, this.rezCheck.IsChecked.Value, this.refuelDate.Date.DateTime));
                this.currentCar.DrivenDistance = Convert.ToInt32(this.distance.Text);
                await Windows.Storage.FileIO.WriteTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.currentCar.Name).AsTask().Result.GetFileAsync("Cardata").AsTask().Result, "Manufacturer " + this.currentCar.Manufacturer + "\nCarYear " + this.currentCar.CarYear + "\nDrivenDistance " + this.currentCar.DrivenDistance + "\nVolume " + this.currentCar.Volume + "\nWeight " + this.currentCar.Weight + "\nCarNumber " + this.currentCar.CarNumber);
                string refuel = this.volume.Text + "\t" + this.refuelDate.Date.DateTime.ToString("yyyy-M-dd") + "\t" + this.pricePerLitre.Text + "\t" + this.distance.Text;
                if (this.fullCheck.IsChecked.Value)
                {
                    refuel += "\tFull";
                }
                else if (this.rezCheck.IsChecked.Value)
                {
                    refuel += "\tEmpty";
                }
                else
                {
                    refuel += "\tNone";
                }
                refuel += "\n";
                await Windows.Storage.FileIO.AppendTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + currentCar.Name).AsTask().Result.GetFileAsync("Refuel").AsTask().Result, refuel);
                this.Frame.GoBack();
            }
        }

        private void rezCheck_Checked(object sender, RoutedEventArgs e)
        {
            this.fullCheck.IsChecked = false;
        }

        private void fullCheck_Checked(object sender, RoutedEventArgs e)
        {
            this.rezCheck.IsChecked = false;
        }
    }
}

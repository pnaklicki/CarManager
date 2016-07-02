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
    public sealed partial class RefuelEdit : Page
    {
        private Car currentCar { get; set; }
        private Refuel currentRefuel { get; set; }

        public RefuelEdit()
        {
            this.InitializeComponent();
            this.currentCar = ((List<Car>)Application.Current.Resources["CarList"]).Where(m => m.Name == Application.Current.Resources["CarDetails"].ToString()).Single();
            this.currentRefuel = (Refuel)Application.Current.Resources["CurrentRefuel"];
            this.refuelDate.Date = this.currentRefuel.Date;
            this.volume.Text = this.currentRefuel.Volume.ToString();
            this.distance.Text = this.currentRefuel.CarDistance.ToString();
            this.pricePerLitre.Text = this.currentRefuel.PricePerLitre.ToString();
            if(this.currentRefuel.ifFullRefuel)
            {
                this.fullCheck.IsChecked = true;
            }
            else if(this.currentRefuel.ifEmptyTank)
            {
                this.rezCheck.IsChecked = true;
            }
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            Refuel tmpRef = this.currentRefuel;
            this.currentRefuel.Date = this.refuelDate.Date.DateTime;
            this.currentRefuel.Volume = Convert.ToDouble(this.volume.Text);
            this.currentRefuel.CarDistance = Convert.ToInt32(this.distance.Text);
            this.currentRefuel.PricePerLitre = Convert.ToDouble(this.pricePerLitre.Text);
            if(this.fullCheck.IsChecked.Value)
            {
                this.currentRefuel.ifFullRefuel = true;
                this.currentRefuel.ifEmptyTank = false;
            }
            else if(this.rezCheck.IsChecked.Value)
            {
                this.currentRefuel.ifFullRefuel = false;
                this.currentRefuel.ifEmptyTank = true;
            }
            else
            {
                this.currentRefuel.ifFullRefuel = false;
                this.currentRefuel.ifEmptyTank = false;
            }
            this.currentRefuel.Price = Math.Round(this.currentRefuel.PricePerLitre * this.currentRefuel.Volume, 2);

            List<Refuel> dateSorted = this.currentCar.Refuels, distanceSorted = this.currentCar.Refuels;

            dateSorted = dateSorted.OrderBy(m => m.Date).ToList();
            distanceSorted = distanceSorted.OrderBy(m => m.CarDistance).ToList();

            if (!Enumerable.SequenceEqual(dateSorted, distanceSorted))
            {
                await new MessageDialog("Przebiegi auta podczas tankowań oraz daty tankowań muszą się zgadzać", "Błąd").ShowAsync();
                this.currentRefuel = tmpRef;
            }
            else
            {
                this.currentCar.Refuels = this.currentCar.Refuels.OrderBy(m => m.CarDistance).ToList();
                this.currentCar.DrivenDistance = Convert.ToInt32(this.currentCar.Refuels.Last().CarDistance);
                await Windows.Storage.FileIO.WriteTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.currentCar.Name).AsTask().Result.GetFileAsync("Cardata").AsTask().Result, "Manufacturer " + this.currentCar.Manufacturer + "\nCarYear " + this.currentCar.CarYear + "\nDrivenDistance " + this.currentCar.DrivenDistance + "\nVolume " + this.currentCar.Volume + "\nWeight " + this.currentCar.Weight + "\nCarNumber " + this.currentCar.CarNumber);

                string refuels = "";
                foreach (var item in this.currentCar.Refuels)
                {
                    refuels += item.Volume.ToString() + "\t" + item.Date.ToString("yyyy-M-dd") + "\t" + item.PricePerLitre.ToString() + "\t" + item.CarDistance.ToString();
                    if (item.ifFullRefuel)
                    {
                        refuels += "\tFull";
                    }
                    else if (item.ifEmptyTank)
                    {
                        refuels += "\tEmpty";
                    }
                    else
                    {
                        refuels += "\tNone";
                    }
                    refuels += "\n";
                }

                await Windows.Storage.FileIO.WriteTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + currentCar.Name).AsTask().Result.GetFileAsync("Refuel").AsTask().Result, refuels);
                Application.Current.Resources["CurrentRefuel"] = "";

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class CarDetails : Page
    {
        private Car currentCar { get; set; }
        private Refuel itemToEdit;
        public CarDetails()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += this.BackButtonPressed;
            this.currentCar = ((List<Car>)Application.Current.Resources["CarList"]).Where(m=>m.Name== Application.Current.Resources["CarDetails"].ToString()).Single();
            this.carPivot.Title = this.currentCar.Name;
            this.name.Text = this.currentCar.Name;
            this.manufacturer.Text = this.currentCar.Manufacturer;
            this.year.Text = this.currentCar.CarYear;
            this.distance.Text = this.currentCar.DrivenDistance.ToString()+" km";
            this.numbers.Text = this.currentCar.CarNumber;
            this.weight.Text = this.currentCar.Weight.ToString() + " kg";
            this.vol.Text = this.currentCar.Volume.ToString()+" l";
            if(this.currentCar.TechnicalRev.Where(m=>m.IsActive).Count() == 1)
            {
                this.tech.Text = "Tak";
                this.tech.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
            else
            {
                this.tech.Text = "Nie";
                this.tech.Foreground = new SolidColorBrush(Colors.Crimson);
            }
            if (this.currentCar.OCPolicies.Where(m => m.IsActive).Count() == 1)
            {
                this.oc.Text = "Tak";
                this.oc.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
            else
            {
                this.oc.Text = "Nie";
                this.oc.Foreground = new SolidColorBrush(Colors.Crimson);
            }

            this.currentCar.CalculateAverageFuelUse();
            this.refuelsList.ItemsSource = this.currentCar.Refuels;
            
            if (this.currentCar.FuelConsumption == -1)
            {
                this.fuelCon.Text = "Brak danych";
            }
            else
            {
                this.fuelCon.Text = this.currentCar.FuelConsumption.ToString() + "l/100km";
            }
            double averageFuelPrice = 0, refuels = 0,moneyUsed = 0;
            if (this.currentCar.Refuels.Where(m => m.ifFullRefuel).Count() >= 3)
            {
                foreach (var item in this.currentCar.Refuels)
                {
                    averageFuelPrice += item.PricePerLitre;
                    refuels++;
                    if (item.Date.ToString("M-yyyy") == DateTime.Now.ToString("M-yyyy"))
                    {
                        moneyUsed += item.Price;
                    }
                }
                averageFuelPrice = Math.Round(averageFuelPrice / refuels, 2);
                this.price10km.Text += " " + (Math.Round(this.currentCar.FuelConsumption * averageFuelPrice,1)).ToString() + " zł";
                this.moneyUsed.Text += " (" + DateTime.Now.ToString("MMMM") + "): " + moneyUsed.ToString() + " zł";
            }
            else
            {
                this.price10km.Text += " B/D";
                foreach (var item in this.currentCar.Refuels)
                {
                  

                    if (item.Date.ToString("M-yyyy") == DateTime.Now.ToString("M-yyyy"))
                    {
                        moneyUsed += item.Price;
                    }
                }
                this.moneyUsed.Text += " (" + DateTime.Now.ToString("MMMM") + "): " + moneyUsed.ToString() + " zł";
            }

            this.repairList.ItemsSource = this.currentCar.Events.Where(m => m.Type == "Naprawa");
        }

        private void BackButtonPressed(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void AddBarButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.addPopup.IsOpen)
            {
                this.addPopup.IsOpen = false;
            }
            else
            {
                this.addPopup.IsOpen = true;
            }
        }

        private void techAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddTechnicalRev));
        }

        private void ocBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddOCPolicy));
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.addPopup.IsOpen = false;

        }

        private void reBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RefuelAdd));
        }

        private void ListViewItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var holdedElement = e.OriginalSource as FrameworkElement;
            if (holdedElement != null)
            {
                this.itemToEdit = holdedElement.DataContext as Refuel;
                RightTapMenu.ShowAt(holdedElement);
            }
        }

        private void EditRefuel(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["CurrentRefuel"] = this.itemToEdit;
            this.Frame.Navigate(typeof(RefuelEdit));
        }

        private void coBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void evBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EventAdd));
        }
    }
}

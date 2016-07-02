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
    public sealed partial class AddTechnicalRev : Page
    {
        public AddTechnicalRev()
        {
            this.InitializeComponent();
            this.techDate.Date = DateTime.Now;
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.techPlace == null)
            {
                await new MessageDialog("Pola nie mogą być puste", "Błąd").ShowAsync();
            }
            else
            {
                Car currentCar = ((List<Car>)Application.Current.Resources["CarList"]).Where(m => m.Name == Application.Current.Resources["CarDetails"].ToString()).Single();

                if (currentCar.TechnicalRev.Count() > 0)
                {
                    currentCar.TechnicalRev.Last().IsActive = false;
                }
                currentCar.TechnicalRev.Add(new TechnicalReview(this.techDate.Date.DateTime, this.techPlace.Text, true));

                string techRev = this.techPlace.Text + "\t" + this.techDate.Date.DateTime.ToString("yyyy-M-dd") + "\n";
                await Windows.Storage.FileIO.AppendTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + currentCar.Name).AsTask().Result.GetFileAsync("TechnicalReviews").AsTask().Result, techRev);
                this.Frame.GoBack();
            }
        }
    }
}

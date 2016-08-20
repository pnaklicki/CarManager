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
    public sealed partial class EventAdd : Page
    {
        private Car currentCar;

        public EventAdd()
        {
            this.InitializeComponent();
            this.currentCar = App.CarList.Where(m => m.Name == Application.Current.Resources["CarDetails"].ToString()).Single();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (this.place.Text == "" || this.price.Text == "" || this.description.Text == "" || this.type.SelectedItem == null)
            {
                await new MessageDialog("Podane dane są niepoprawne", "Błąd").ShowAsync();
            }
            else
            {
                this.currentCar.Events.Add(new Event(this.place.Text,this.date.Date.DateTime,Convert.ToDouble(this.price.Text),this.description.Text, ((ComboBoxItem)this.type.SelectedItem).Content.ToString()));

                string carEvent = this.place.Text + "\t" + this.date.Date.DateTime.ToString("yyyy-M-dd") + "\t" + this.price.Text +"\t" +this.description.Text + "\t"+((ComboBoxItem)this.type.SelectedItem).Content.ToString()+"\n";

                await Windows.Storage.FileIO.AppendTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.currentCar.Name).AsTask().Result.GetFileAsync("Event").AsTask().Result, carEvent);
                this.Frame.GoBack();
            }
        }
    }
}

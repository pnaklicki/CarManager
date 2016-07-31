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
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.Storage;
using Windows.Media.SpeechRecognition;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Test10App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Menu : Page
    {
        private Car itemToDelete;
        public Menu()
        {
            this.InitializeComponent();
            List<ListViewItem> listItems = new List<ListViewItem>();
            TimeSpan delay = TimeSpan.FromSeconds(1);
            object tmp = new object();
            Application.Current.Resources.TryGetValue("CarList", out tmp);
            try
            {
                if (tmp != null)
                {
                    if ((List<Car>)Application.Current.Resources["CarList"] != null)
                    {
                        string carTechs = "";
                        string ocPol = "";
                        foreach (var item in (List<Car>)Application.Current.Resources["CarList"])
                        {
                            ListViewItem newCar = new ListViewItem();
                            var carButton = new Button();
                            newCar.Content = item.Name;
                            newCar.Tapped += CarDetail;
                            newCar.RightTapped += ItemRightTapMenu;
                            listItems.Add(newCar);
                            if (item.TechnicalRev.Count > 0 && item.TechnicalRev.Where(m => m.IsActive == true) != null)
                            {
                                if (item.TechnicalRev.Where(m => m.IsActive == true).Single().ValidTo > DateTime.Now)
                                {
                                    carTechs += item.Name + "\t" + item.TechnicalRev.Where(m => m.IsActive == true).Single().ValidTo.ToString("yyyy-M-dd") + "\n";
                                }
                                else
                                {
                                    item.TechnicalRev.Where(m => m.IsActive == true).Single().IsActive = false;
                                }
                            }
                            if (item.OCPolicies.Count() > 0 && item.OCPolicies.Where(m => m.IsActive == true) != null)
                            {
                                if (item.OCPolicies.Where(m => m.IsActive == true).Single().ValidTo > DateTime.Now)
                                {
                                    ocPol += item.Name + "\t" + item.OCPolicies.Where(m => m.IsActive == true).Single().ValidTo.ToString("yyyy-M-dd") + "\n";
                                }
                                else
                                {
                                    item.OCPolicies.Where(m => m.IsActive == true).Single().IsActive = false;
                                }
                            }
                        }
                        var settings = ApplicationData.Current.LocalSettings;
                        settings.Values["CarsBackgroundTechs"] = carTechs;
                        settings.Values["CarsBackgroundOC"] = ocPol;
                    }
                    listView.ItemsSource = listItems;
                    foreach (var item in (List<Car>)Application.Current.Resources["CarList"])
                    {
                        item.LoadCarImage();
                    }
                }
                else
                {
                    ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreateTimer(
                    (source) =>
                    {
                        Dispatcher.RunAsync(
                            CoreDispatcherPriority.High,
                            () =>
                            {
                                if ((List<Car>)Application.Current.Resources["CarList"] != null)
                                {
                                    string carTechs = "";
                                    foreach (var item in (List<Car>)Application.Current.Resources["CarList"])
                                    {
                                        ListViewItem newCar = new ListViewItem();
                                        var carButton = new Button();
                                        newCar.Content = item.Name;
                                        newCar.Tapped += CarDetail;
                                        newCar.RightTapped += ItemRightTapMenu;
                                        listItems.Add(newCar);
                                        if (item.TechnicalRev.Count > 0 && item.TechnicalRev.Where(m => m.IsActive != true) != null)
                                        {
                                            if (item.TechnicalRev.Where(m => m.IsActive == true).Single().ValidTo > DateTime.Now)
                                            {
                                                carTechs += item.Name + "\t" + item.TechnicalRev.Where(m => m.IsActive == true).Single().ValidTo.ToString("yyyy-M-dd") + "\n";
                                            }
                                            else
                                            {
                                                item.TechnicalRev.Where(m => m.IsActive == true).Single().IsActive = false;
                                            }
                                        }
                                        var settings = ApplicationData.Current.LocalSettings;
                                        settings.Values["CarsBackground"] = carTechs;
                                    }
                                    listView.ItemsSource = listItems;

                                    foreach (var item in (List<Car>)Application.Current.Resources["CarList"])
                                    {
                                        item.LoadCarImage();
                                    }
                                }
                            });

                    }, delay);


                }
            }
            catch
            {
                
            }
        }

        private void ItemRightTapMenu(object sender, RightTappedRoutedEventArgs e)
        {
            var holdedElement = e.OriginalSource as FrameworkElement;
            if (holdedElement != null)
            {
                itemToDelete = ((List<Car>)Application.Current.Resources["CarList"]).Where(m => m.Name == ((ListViewItem)sender).Content.ToString()).Single();
                RightTapMenu.ShowAt(holdedElement);
            }
        }

        private void CarDetail(object sender, RoutedEventArgs e)
        {
            
            Application.Current.Resources["CarDetails"]=((ListViewItem)sender).Content;
            Frame.Navigate(typeof(CarDetails));
        }

        private async void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            var exitConf = new MessageDialog("Czy na pewno chcesz wyjść?", "Potwierdzenie");
            exitConf.Commands.Add(new UICommand("Tak") { Id = 1 });
            exitConf.Commands.Add(new UICommand("Nie") { Id = 0 });
            exitConf.DefaultCommandIndex = 0;
            exitConf.CancelCommandIndex = 1;
            var result = await exitConf.ShowAsync();
            
            if (result.Label == "Tak")
            {
                Application.Current.Exit();
            }
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CarAdd));
        }

        private async void DeleteRightTap(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageFolder curFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            await curFolder.GetFolderAsync("Car" + itemToDelete.Name).AsTask().Result.DeleteAsync();
            ((List<Car>)Application.Current.Resources["CarList"]).Remove(((List<Car>)Application.Current.Resources["CarList"]).Where(m => m.Name == itemToDelete.Name).Single());
            Frame.Navigate(typeof(Menu));
            Frame.GoBack();
        }
    }
}

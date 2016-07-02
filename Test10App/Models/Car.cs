using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Test10App
{
    class Car
    {
        public string Name { get; }
        public string Manufacturer { get; set; }
        public string CarYear { get; set; }
        public int DrivenDistance { get; set; }
        public int Volume { get; set; }
        public int Weight { get; set; }
        public string CarNumber { get; set; }
        public double FuelConsumption { get; set; }
        public List<TechnicalReview> TechnicalRev { get; set; }
        public List<OCPolicy> OCPolicies { get; set; }
        public List<Refuel> Refuels { get; set; }
        public List<Event> Events { get; set; }

        public Car()
        {
        }

        public Car(string name, string manufacturer, string year, int distance, int vol, int weight, string numbers)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.CarYear = year;
            this.DrivenDistance = distance;
            this.Volume = vol;
            this.Weight = weight;
            this.CarNumber = numbers;
            this.TechnicalRev = new List<TechnicalReview>();
            this.OCPolicies = new List<OCPolicy>();
            this.Refuels = new List<Refuel>();
            this.Events = new List<Event>();
            AddCarFolder(name);
        }

        public Car(string name, bool ifAutoAdd = false)
        {
            this.Name = name;
            this.TechnicalRev = new List<TechnicalReview>();
            this.OCPolicies = new List<OCPolicy>();
            this.Refuels = new List<Refuel>();
            this.Events = new List<Event>();
            if (!ifAutoAdd)
            {
                AddCarFolder(name);
            }
        }

        public void CalculateAverageFuelUse()
        {
            double fuelPer100km = 0;
            double fuelCounts = 0;
            if (this.Refuels.Where(m => m.ifFullRefuel).Count() >= 3)
            {
                foreach (var item in this.Refuels)
                {
                    if (this.Refuels.IndexOf(item) != 0 && item.ifFullRefuel)
                    {
                        if (this.Refuels.First() != item && this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).ifFullRefuel)
                        {
                            fuelPer100km += (item.Volume / (item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).CarDistance)) * 100 * 2;
                            fuelCounts += 2;
                        }
                        else if (this.Refuels.First() != item && this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).ifEmptyTank)
                        {
                            fuelPer100km += ((this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).Volume + 5 - (this.Volume - item.Volume)) / (item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).CarDistance)) * 100 * 1;
                            fuelCounts++;
                        }
                        else if (this.Refuels.First() != item)
                        {
                            fuelPer100km += (((this.Volume - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).Volume - 5) * (2 / 3)) / (item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).CarDistance)) * 100 * 0.5;
                            fuelCounts += 0.5;
                        }
                    }
                    else if (this.Refuels.IndexOf(item) != 0 && item.ifEmptyTank)
                    {
                        if (this.Refuels.First() != item && this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).ifFullRefuel)
                        {
                            fuelPer100km += (this.Volume - 5) / (item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).CarDistance) * 100 * 2;
                            fuelCounts += 2;
                        }
                        else if (this.Refuels.First() != item && this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).ifEmptyTank)
                        {
                            fuelPer100km += this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).Volume / (item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).CarDistance) * 100 * 1;
                            fuelCounts++;
                        }
                        else if (this.Refuels.First() != item)
                        {
                            fuelPer100km += ((1 / 3) * (this.Volume - item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).Volume)) / (item.CarDistance - this.Refuels.ElementAt(this.Refuels.IndexOf(item) - 1).CarDistance) * 100 * 0.5;
                            fuelCounts += 0.5;
                        }
                    }
                }
                this.FuelConsumption = Math.Round(fuelPer100km/fuelCounts,2);
            }
            else
            {
                this.FuelConsumption = -1;
            }
        }

        private async void AddCarFolder(string fileName)
        {
            try
            {
                await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("Car" + fileName, Windows.Storage.CreationCollisionOption.FailIfExists);
                await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + fileName).AsTask().Result.CreateFileAsync("Cardata", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.Name).AsTask().Result.GetFileAsync("Cardata").AsTask().Result, "Manufacturer " + this.Manufacturer + "\nCarYear " + this.CarYear + "\nDrivenDistance " + this.DrivenDistance+"\nVolume "+this.Volume+"\nWeight "+this.Weight+"\nCarNumber "+this.CarNumber);
                await ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.Name).AsTask().Result.CreateFileAsync("TechnicalReviews");
                await ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.Name).AsTask().Result.CreateFileAsync("OCPolicy");
                await ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.Name).AsTask().Result.CreateFileAsync("Refuel");
                await ApplicationData.Current.LocalFolder.GetFolderAsync("Car" + this.Name).AsTask().Result.CreateFileAsync("Event");
            }
            catch(System.Exception e)
            {
                await new Windows.UI.Popups.MessageDialog("Podane auto już istnieje!", "Błąd").ShowAsync();
                ((List<Car>)Windows.UI.Xaml.Application.Current.Resources["CarList"]).Remove(((List<Car>)Windows.UI.Xaml.Application.Current.Resources["CarList"]).Where(m => m.Name == fileName).Single());
            }
        }

        public async void GetAllCars()
        {
            List<Car> cars = new List<Car>();
           
            IReadOnlyList<Windows.Storage.IStorageItem> carFolders = await Windows.Storage.ApplicationData.Current.LocalFolder.GetItemsAsync();
            foreach (Windows.Storage.IStorageItem item in carFolders)
            {
                  if (item.IsOfType(Windows.Storage.StorageItemTypes.Folder) && item.Name.Contains("Car"))
                {
                    //item.DeleteAsync();
                    string refuel = await FileIO.ReadTextAsync(((Windows.Storage.StorageFolder)item).GetFileAsync("Refuel").AsTask().Result);
                    string ocPol  = await FileIO.ReadTextAsync(((Windows.Storage.StorageFolder)item).GetFileAsync("OCPolicy").AsTask().Result);
                    string techRev = await FileIO.ReadTextAsync(((Windows.Storage.StorageFolder)item).GetFileAsync("TechnicalReviews").AsTask().Result);
                    string carDataText = await Windows.Storage.FileIO.ReadTextAsync(((Windows.Storage.StorageFolder)item).GetFileAsync("Cardata").AsTask().Result);
                    string events = await Windows.Storage.FileIO.ReadTextAsync(((Windows.Storage.StorageFolder)item).GetFileAsync("Event").AsTask().Result);

                    string[] carData = carDataText.Split('\n');
                    Car newCar = new Car(item.Name.Substring(3), true);
                    foreach (var option in carData)
                    {
                        string[] curOption = option.Split(' ');
                        switch(curOption[0])
                        {
                            case "Manufacturer":
                                newCar.Manufacturer = curOption[1];
                                break;
                            case "CarYear":
                                newCar.CarYear = curOption[1];
                                break;
                            case "DrivenDistance":
                                newCar.DrivenDistance = Convert.ToInt32(curOption[1]);
                                break;
                            case "Volume":
                                newCar.Volume = Convert.ToInt32(curOption[1]);
                                break;
                            case "Weight":
                                newCar.Weight = Convert.ToInt32(curOption[1]);
                                break;
                            case "CarNumber":
                                newCar.CarNumber = curOption[1];
                                break;
                        }
                    }
                    if (((Windows.Storage.StorageFolder)item).GetFileAsync("TechnicalReviews") != null)
                    {
                        bool activeTech = true;
                        string[] reviews = techRev.Split('\n');

                        foreach (var rev in reviews)
                        {
                            if (rev != "")
                            {
                                string[] data = rev.Split('\t');
                                string[] date = data[1].Split('-');
                                newCar.TechnicalRev.Add(new TechnicalReview(new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2])), data[0],activeTech));
                                activeTech = false;
                            }
                        }
                    }

                    if (((Windows.Storage.StorageFolder)item).GetFileAsync("OCPolicy") != null)
                    {
                        bool activeOC = true;
                        string[] policies = ocPol.Split('\n');
                        foreach (var oc in policies)
                        {
                            if(oc != "")
                            {
                                string[] data = oc.Split('\t');
                                string[] date = data[1].Split('-');
                                newCar.OCPolicies.Add(new OCPolicy(new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2])), data[0], Convert.ToInt32(data[2]),activeOC));
                                activeOC = false;
                            }
                        }
                    }

                    if(((Windows.Storage.StorageFolder)item).GetFileAsync("Refuel") != null)
                    {
                        string[] refuels = refuel.Split('\n');
                        foreach (var re in refuels)
                        {
                            if(re != "")
                            {
                                bool ifFull = false,ifEmpty=false;
                                string[] data = re.Split('\t');
                                string[] date = data[1].Split('-');
                                if(data[4] == "Full")
                                {
                                    ifFull = true;
                                }
                                else if(data[4] == "Empty")
                                {
                                    ifEmpty = true;
                                }
                                newCar.Refuels.Add(new Refuel(Convert.ToDouble(data[0]), Convert.ToDouble(data[2]), Convert.ToInt32(data[3]),ifFull, ifEmpty, new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]))));
                            }
                        }
                    }

                    if (((Windows.Storage.StorageFolder)item).GetFileAsync("Event") != null)
                    {
                        string[] events1 = events.Split('\n');
                        foreach (var ev in events1)
                        {
                            if (ev != "")
                            {
                                string[] data = ev.Split('\t');
                                string[] date = data[1].Split('-');

                                newCar.Events.Add(new Event(data[0], new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2])),Convert.ToDouble(data[2]),data[3],data[4]));
                            }
                        }
                    }
                    cars.Add(newCar);
                }
            }
            Windows.UI.Xaml.Application.Current.Resources["CarList"] = cars;
            
        }
    }
}

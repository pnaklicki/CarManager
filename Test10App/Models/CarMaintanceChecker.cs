using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Test10App.Models
{
    public class CarMaintanceChecker
    {
        private ApplicationDataContainer settings;

        public CarMaintanceChecker()
        {
            settings = ApplicationData.Current.LocalSettings;
        }

        public void CheckCarMaintanceInfo(string[] carMaintanceData)
        {
            string[] reviewsData = carMaintanceData[0].Split('\n');
            string[] ocData = carMaintanceData[1].Split('\n');
            CheckTechs(reviewsData);
            CheckOC(ocData);
        }

        private void CheckOC(string[] ocData)
        {
            foreach (var item in ocData)
            {
                if (item != "")
                {
                    string[] carData = item.ToString().Split('\t');
                    string[] date = carData[1].ToString().Split('-');
                    int daysLeft = (new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]))).Subtract(DateTime.Now).Days + 1;
                    if (daysLeft <= 7)
                    {
                        string dayInfo;

                        if (daysLeft == 0)
                        {
                            dayInfo = "Dzisiaj";
                        }
                        else if (daysLeft == 1)
                        {
                            dayInfo = "Za " + daysLeft.ToString() + " dzień";
                        }
                        else
                        {
                            dayInfo = "Za " + daysLeft.ToString() + " dni";
                        }

                        var template = $@"
<toast>
<visual>
<binding template=""ToastGeneric"">
  <text>Koniec polisy OC!</text>
  <text>{dayInfo} kończy się polisa OC następującego samochodu: {carData[0]}!</text>
</binding>
</visual>
</toast>
";
                        var xml = new XmlDocument();
                        xml.LoadXml(template);
                        var toast = new ToastNotification(xml);
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
            }
        }

        private void CheckTechs(string[] reviewsData)
        {
            foreach (string item in reviewsData)
            {
                if (item != "")
                {
                    string[] carData = item.ToString().Split('\t');
                    string[] date = carData[1].ToString().Split('-');
                    int daysLeft = (new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]))).Subtract(DateTime.Now).Days + 1;
                    if (daysLeft <= 7)
                    {
                        string dayInfo;

                        if (daysLeft == 0)
                        {
                            dayInfo = "Dzisiaj";
                        }
                        else if (daysLeft == 1)
                        {
                            dayInfo = "Za " + daysLeft.ToString() + " dzień";
                        }
                        else
                        {
                            dayInfo = "Za " + daysLeft.ToString() + " dni";
                        }

                        var template = $@"
<toast>
<visual>
<binding template=""ToastGeneric"">
  <text>Koniec przeglądu technicznego!</text>
  <text>{dayInfo} kończy się przegląd następującego samochodu: {carData[0]}!</text>
</binding>
</visual>
</toast>
";
                        var xml = new XmlDocument();
                        xml.LoadXml(template);
                        var toast = new ToastNotification(xml);
                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
            }
        }
    }
}

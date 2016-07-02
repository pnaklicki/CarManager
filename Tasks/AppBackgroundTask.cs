using NotificationsExtensions.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.System.Threading;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Tasks
{
    public sealed class AppBackgroundTask : IBackgroundTask
    {
        static ApplicationTrigger trigger = new ApplicationTrigger();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            var settings = ApplicationData.Current.LocalSettings;
            string techReviews = settings.Values["CarsBackgroundTechs"].ToString();
            string ocPolicies = settings.Values["CarsBackgroundOC"].ToString();
            string[] reviewsData = techReviews.Split('\n');
            string[] ocData = ocPolicies.Split('\n');
            CheckTechs(reviewsData);
            CheckOC(ocData);
            _deferral.Complete();
        }

        private void CheckOC(string[] ocData)
        {
            foreach (var item in ocData)
            {
                if(item != "")
                {
                    string[] carData = item.ToString().Split('\t');
                    string[] date = ocData[1].ToString().Split('-');
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

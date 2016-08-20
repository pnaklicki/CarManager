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

        
    }
}

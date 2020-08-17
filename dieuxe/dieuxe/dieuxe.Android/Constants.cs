using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace dieuxe.Droid
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://dieuxenotificationhubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=li62qPne9r6mX+oA+cHBeLrv+FSpQYQBVooxUqbT6Ho=";
        public const string NotificationHubName = "dieuxeNotificationHub";
    }
}
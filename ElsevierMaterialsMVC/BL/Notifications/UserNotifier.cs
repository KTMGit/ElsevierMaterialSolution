using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Notifications
{
    public class UserNotifier
    {

        private const string NotificationKey = "UserNotificationMessage";

        private const string RegistrationKey = "UserRegistration";

        public static void SetMessage(string message)
        {
            HttpContext.Current.Session[NotificationKey] = message;
        }

        public static string ReadMessageAndDestroy
        {
            get
            {
                string message = (string)HttpContext.Current.Session[NotificationKey];
                HttpContext.Current.Session[NotificationKey] = null;
                return message;
            }
        }

        public static bool MessagePending
        {
            get
            {
                return HttpContext.Current.Session[NotificationKey] != null;
            }
        }

        public static bool RegistrationMessage
        {
            set
            {
                HttpContext.Current.Session[RegistrationKey] = value;
            }
            get
            {
                bool? val = (bool?)HttpContext.Current.Session[RegistrationKey];
                HttpContext.Current.Session[RegistrationKey] = null;
                return val ?? false;
            }
        }
    }
}
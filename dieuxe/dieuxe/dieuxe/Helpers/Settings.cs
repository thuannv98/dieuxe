using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace dieuxe.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        //#region Setting Constants

        //private const string SettingsKey = "settings_key";
        //private static readonly string SettingsDefault = string.Empty;

        //#endregion
        public static string email
        {
            get
            {
                return AppSettings.GetValueOrDefault("email", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("email", value);
            }
        }
        public static string password
        {
            get
            {
                return AppSettings.GetValueOrDefault("password", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("password", value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }
    }
}

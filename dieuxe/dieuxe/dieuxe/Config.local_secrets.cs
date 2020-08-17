using System;
using System.Collections.Generic;
using System.Text;

namespace dieuxe
{
    public static partial class Config
    {
        static Config()
        {
            ApiKey = "apikey";
            BackendServiceEndpoint = "https://pushnotificationapp.azurewebsites.net/";
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace dieuxe.Services
{
    public interface ILocationUpdateService
    {
        void GetUserLocation();
        event EventHandler<ILocationEventArgs> LocationChanged;
    }
    public interface ILocationEventArgs
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}

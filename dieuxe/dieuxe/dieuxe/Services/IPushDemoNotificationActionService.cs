using System;
using System.Collections.Generic;
using System.Text;
using dieuxe.Models;

namespace dieuxe.Services
{
    public interface IPushDemoNotificationActionService : INotificationActionService
    {
        event EventHandler<PushDemoAction> ActionTriggered;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace dieuxe.Services
{
    public interface INotificationActionService
    {
        void TriggerAction(string action);
    }
}

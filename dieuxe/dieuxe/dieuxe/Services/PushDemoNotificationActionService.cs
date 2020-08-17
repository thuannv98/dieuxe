using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using dieuxe.Models;

namespace dieuxe.Services
{
    public class PushDemoNotificationActionService : IPushDemoNotificationActionService
    {
        readonly Dictionary<string, PushDemoAction> _actionMappings = new Dictionary<string, PushDemoAction>
        {
            { "xem_thong_bao", PushDemoAction.XemThongBao },
            { "action_b", PushDemoAction.ActionB }
        };

        public event EventHandler<PushDemoAction> ActionTriggered = delegate { };

        public void TriggerAction(string action)
        {
            if (!_actionMappings.TryGetValue(action, out var pushDemoAction))
                return;

            List<Exception> exceptions = new List<Exception>();

            foreach (var handler in ActionTriggered?.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, pushDemoAction);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}

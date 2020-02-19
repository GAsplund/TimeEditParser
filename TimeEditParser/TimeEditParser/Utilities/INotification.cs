using System;
using System.Collections.Generic;
using System.Text;
using TimeEditParser.Models;

namespace TimeEditParser.Utilities
{
    public interface INotification
    {
        void NotifyNotification(ScheduledNotification notification, bool notify);
    }
}

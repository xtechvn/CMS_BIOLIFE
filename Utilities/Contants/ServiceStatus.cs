using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public enum ServiceStatus
    {
        New=0,
        WaitingExcution=1,
        OnExcution=2,
        ServeCode=3,
        Payment=4,
        Decline=5,
        Cancel=6,
        ServeCodeKH=8,
    }
}

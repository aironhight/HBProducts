using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Services
{
    interface INotifyView
    {
        void notify(string type, params object[] list);
    }
}

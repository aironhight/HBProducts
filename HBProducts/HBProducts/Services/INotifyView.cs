using System;
using System.Collections.Generic;
using System.Text;

namespace HBProducts.Services
{
    public interface INotifyView
    {
        void notify(string type, params object[] list);
    }
}

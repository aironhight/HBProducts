using System;
using System.Collections.Generic;
using System.Text;

namespace HBProductsSupport.Services
{
    public interface INotifyView
    {
        void notify(string type, params object[] list);
    }
}

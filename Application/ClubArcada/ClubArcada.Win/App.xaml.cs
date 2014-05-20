using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ClubArcada.Win
{
    public partial class App : Application
    {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Other.Functions.SendMail(e.Exception.Message.ToString(), e.Exception.InnerException.ToString());
            e.Handled = true;
        }
    }
}

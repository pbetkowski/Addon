using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace PowerBusiness.Shared
{
    public static class InfoBoxes
    {
        public static void UseMessageBox(String message)
        {
            Application.SBO_Application.MessageBox(message);
        }

        public static void UseMessageBox(int message)
        {
            Application.SBO_Application.MessageBox(message.ToString());
        }

        public static void UseMessageBox(double message)
        {
            Application.SBO_Application.MessageBox(message.ToString());
        }
    }
}

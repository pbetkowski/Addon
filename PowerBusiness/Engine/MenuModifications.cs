using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
namespace PowerBusiness.Engine
{
    class MenuModifications
    {
        public void addMenuItem(String uniqueID, String name, SAPbouiCOM.Menus oMenus, SAPbouiCOM.MenuCreationParams oCreationPackage)
        {
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            oCreationPackage.UniqueID = uniqueID;
            oCreationPackage.String = name;
            oMenus.AddEx(oCreationPackage);
        }

        public SAPbouiCOM.MenuCreationParams returnOnCreationPackage(SAPbouiCOM.Menus oMenus, SAPbouiCOM.MenuItem oMenuItem)
        {
            oMenus = null;
            oMenuItem = null;
            oMenus = Application.SBO_Application.Menus;
            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "PowerBusiness";
            oCreationPackage.String = "Electropoli Poland";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;
            oMenus = oMenuItem.SubMenus;
            return oCreationPackage;
        }
    }
}

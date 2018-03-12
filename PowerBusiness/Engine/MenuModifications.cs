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

        public SAPbouiCOM.MenuCreationParams addMenu(String uniqueID, String name)
        {
            SAPbouiCOM.MenuCreationParams oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = uniqueID;
            oCreationPackage.String = name;
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;
            return oCreationPackage;
        }
    }
}

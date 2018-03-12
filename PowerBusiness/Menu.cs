using System;
using System.Collections.Generic;
using System.Text;
using PowerBusiness.Engine;
using SAPbouiCOM.Framework;
using PowerBusiness.Shared;

namespace PowerBusiness
{
    class Menu
    {
        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus;
            SAPbouiCOM.MenuItem oMenuItem;
            SAPbouiCOM.MenuCreationParams oCreationPackage;
            MenuModifications modifications = new MenuModifications();

            oMenus = Application.SBO_Application.Menus;

            oCreationPackage = modifications.addMenu("PowerBusiness", "Electropoli Poland");         
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'
            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception)
            {
               
            }

            try
                {
                 // Get the menu collection of the newly added pop-up item
                     oMenuItem = Application.SBO_Application.Menus.Item("PowerBusiness");
                     oMenus = oMenuItem.SubMenus;

                     // Create s sub menu
                    modifications.addMenuItem("PowerBusiness.Form1", "System raportów", oMenus, oCreationPackage);
                
                 }
            catch (Exception)
            {
                InfoBoxes.StatusBarSucces("Nadpisano strukturę menu");
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "PowerBusiness.Form1")
                {
                    Form1 activeForm = new Form1();
                    activeForm.Show();
                }
            }
            catch (Exception e)
            {
                InfoBoxes.UseMessageBox(e.Message);
            }
        }
    }
}

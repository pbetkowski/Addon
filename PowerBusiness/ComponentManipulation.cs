using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;


namespace PowerBusiness
{
     public class ComponentManipulation
    {
        int white = 16777215;
        int red = 255;
        int green = 6026752;
        int orange = 16490752;
        int yellow = 16766852;
        int sea = 16766852;
        int blue = 16766852;
        
        public void changeLabel(SAPbouiCOM.StaticText StaticText0, SAPbouiCOM.StaticText StaticText1, SAPbouiCOM.StaticText StaticText2, SAPbouiCOM.StaticText StaticText3, SAPbouiCOM.StaticText StaticText4, SAPbouiCOM.StaticText StaticText5, SAPbouiCOM.StaticText StaticText6, SAPbouiCOM.StaticText StaticText7, String par1, String par2, String par3, String par4, String par5, String par6, String par7, String par8)
        {
            StaticText0.Caption = par1;
            StaticText1.Caption = par2;
            StaticText2.Caption = par3;
            StaticText3.Caption = par4;
            StaticText4.Caption = par5;
            StaticText5.Caption = par6;
            StaticText6.Caption = par7;
            StaticText7.Caption = par8;          
        }


        public void changeMainLabel(SAPbouiCOM.StaticText MainLabel, String Name)
        {
            MainLabel.Caption = Name;      
        }

         // painting grid for purchase_department
        public void fillWithColorsPurchaseOrder(SAPbouiCOM.Grid gridPanel, int columnIndex)
        {
            try
            {
                int numberOfRows = gridPanel.Rows.Count;

                for (int i = 0; i < numberOfRows; i++)
                {
                    //i= number of record, column is defined while executing method 
                    //double columnValue = Double.Parse(gridPanel.DataTable.GetValue(columnIndex, i).ToString());
                    String columnvalue = gridPanel.DataTable.GetValue(columnIndex, i).ToString();

                    if (columnvalue.Equals("Zlecenie wewnętrzne"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i+1, red);

                    }

                    else if (columnvalue.Equals("Dyr_Zak/Log"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, orange);

                    }

                    else if (columnvalue.Equals("OK"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, blue);
                    }

                    else if (columnvalue.Equals("Zarząd"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, sea);
                    }

                    else if (columnvalue.Equals("Zrealizowane"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, yellow);
                    }

                    else if (columnvalue.Equals("Dyr_Zakładu"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i+1, blue);
                    }
               

                }
            }

            catch (Exception e)
            {
                Application.SBO_Application.MessageBox(e.Message);
            }
         
        }


        public void fillWithColorsChemicalOrders(SAPbouiCOM.Grid gridPanel, int columnIndex)
        {
            try
            {
                int numberOfRows = gridPanel.Rows.Count;

                for (int i = 0; i < numberOfRows; i++)
                {
                    //i= number of record, column is defined while executing method 
                    //double columnValue = Double.Parse(gridPanel.DataTable.GetValue(columnIndex, i).ToString());
                    String columnvalue = gridPanel.DataTable.GetValue(columnIndex, i).ToString();

                    if (columnvalue.Equals("Niezrealizowane"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, red);

                    }

                    if (columnvalue.Equals("Zrealizowane"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, green);

                    }

                    if (columnvalue.Equals("Częściowo"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, orange);
                    }

                }
            }

            catch (Exception e)
            {
                Application.SBO_Application.MessageBox(e.Message);
            }
        }
             public void fillWithColorsChemicalStock(SAPbouiCOM.Grid gridPanel, int columnIndex)
          {
        
            try
            {
                int numberOfRows = gridPanel.Rows.Count;

                for (int i = 0; i < numberOfRows; i++)
                {
                    //i= number of record, column is defined while executing method 
                    //double columnValue = Double.Parse(gridPanel.DataTable.GetValue(columnIndex, i).ToString());
                    String columnvalue = gridPanel.DataTable.GetValue(columnIndex, i).ToString();

                    if (columnvalue.Equals("OK"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, green);

                    }

                    if (columnvalue.Equals("NOK"))
                    {
                        gridPanel.CommonSetting.SetRowBackColor(i + 1, red);

                    }

                }
            }

            catch (Exception e)
            {
                Application.SBO_Application.MessageBox(e.Message);
            }

        }
        public void cleanRows(SAPbouiCOM.Grid gridPanel)
        {
            int numberOfRows = gridPanel.Rows.Count;
            for (int i = 1; i < numberOfRows; i++ )
            {   
                
                gridPanel.CommonSetting.SetRowBackColor(i,white);
            }
        }


         //inicjalizacja listy


        public List<SAPbouiCOM.EditText> addItemsToList(SAPbouiCOM.EditText e1, SAPbouiCOM.EditText e2, SAPbouiCOM.EditText e3, SAPbouiCOM.EditText e4, SAPbouiCOM.EditText e5, SAPbouiCOM.EditText e6, SAPbouiCOM.EditText e7, SAPbouiCOM.EditText e8)
        {
            List<SAPbouiCOM.EditText> list = new List<SAPbouiCOM.EditText>();
            list.Add(e1);
            list.Add(e2);
            list.Add(e3);
            list.Add(e4);
            list.Add(e5);
            list.Add(e6);
            list.Add(e7);
            list.Add(e8);
            return list;
        }


        public void checkIfItemValueIsNull(List<SAPbouiCOM.EditText> list)
        {
            foreach (var item in list)
            {
                if (!String.IsNullOrEmpty(item.String))
                {
                    item.String = String.Empty;
                }
            }
        }


        //closing


        public void closeApplication()
        {
            Application.SBO_Application.Forms.Item("ff").Close();
        }
    }
}

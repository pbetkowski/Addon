using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;
using PowerBusiness;
using PowerBusiness.Shared;

namespace PowerBusiness
{
    [FormAttribute("PowerBusiness.Form1", "Form1.b1f")]
    class Form1 : UserFormBase
    {
        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.EditText Edit1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText Edit2;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText Edit3;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText Edit4;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText Edit5;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.EditText Edit6;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.EditText Edit7;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.EditText Edit8;
        private SAPbouiCOM.Button FillTableButton;
        private SAPbouiCOM.Button RefreshButton;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.Grid Grid1;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.EditText Edit0;
        private SAPbouiCOM.LinkedButton LinkedButton0;
        private SAPbouiCOM.StaticText StaticText11;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Button HelpButton;
        private SAPbouiCOM.Button CountButton;
        private SAPbouiCOM.Form form;
        private SAPbouiCOM.Grid Grid2;
        private SAPbouiCOM.Button AuthenticationButton;

        SqlClass SqlExecutor = new SqlClass();
        ComponentManipulation CM_Obj = new ComponentManipulation();
        Counter cntObj = new Counter();
        Authentication authentication = new Authentication();
        String dynamicColumnName;
        String SecondPar;
        String par1;
        String par2;
        String par3;
        String par4;
        String par5;
        String par6;
        String par7;
        String par8;
        Boolean isColored;
        Boolean isSorted;
        List<SAPbouiCOM.EditText> listOfEditText;
        #region frameworkSpam
        public Form1()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        /// 
       
        public override void OnInitializeComponent()
        {
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_0").Specific));
            this.Grid0.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.Grid0_DoubleClickAfter);
            this.Grid0.PressedAfter += new SAPbouiCOM._IGridEvents_PressedAfterEventHandler(this.OnClickListener);
            this.Edit1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.Edit1.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit1_DoubleClickBefore);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.Edit2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.Edit2.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit2_DoubleClickBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.Edit3 = ((SAPbouiCOM.EditText)(this.GetItem("Item_6").Specific));
            this.Edit3.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit3_DoubleClickBefore);
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.Edit4 = ((SAPbouiCOM.EditText)(this.GetItem("Item_8").Specific));
            this.Edit4.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit4_DoubleClickBefore);
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.Edit5 = ((SAPbouiCOM.EditText)(this.GetItem("Item_10").Specific));
            this.Edit5.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit5_DoubleClickBefore);
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.Edit6 = ((SAPbouiCOM.EditText)(this.GetItem("Item_12").Specific));
            this.Edit6.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit6_DoubleClickBefore);
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_13").Specific));
            this.Edit7 = ((SAPbouiCOM.EditText)(this.GetItem("Item_14").Specific));
            this.Edit7.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit7_DoubleClickBefore);
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_15").Specific));
            this.Edit8 = ((SAPbouiCOM.EditText)(this.GetItem("Item_16").Specific));
            this.Edit8.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.Edit8_DoubleClickBefore);
            this.FillTableButton = ((SAPbouiCOM.Button)(this.GetItem("Item_17").Specific));
            this.FillTableButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.FillTableButton_ClickBefore);
            this.RefreshButton = ((SAPbouiCOM.Button)(this.GetItem("Item_18").Specific));
            this.RefreshButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.ResetButton);
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_19").Specific));
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_20").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_21").Specific));
            this.Grid1 = ((SAPbouiCOM.Grid)(this.GetItem("Item_22").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_23").Specific));
            this.Edit0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_24").Specific));
            this.Edit0.DoubleClickBefore += new SAPbouiCOM._IEditTextEvents_DoubleClickBeforeEventHandler(this.EditText8_DoubleClickBefore);
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_25").Specific));
            this.StaticText11 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_26").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_27").Specific));
            this.Button2.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);

            //     
            this.listOfEditText = this.CM_Obj.addItemsToList(this.Edit0, this.Edit1, this.Edit2, this.Edit3, this.Edit4, this.Edit5, this.Edit6, this.Edit7, this.Edit8);
            //     
            this.CountButton = ((SAPbouiCOM.Button)(this.GetItem("Item_31").Specific));
            this.CountButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.CountTotal);
            this.GetItem("Item_31").Visible = false;
            this.GetItem("Item_19").Enabled = false;
            this.Grid2 = ((SAPbouiCOM.Grid)(this.GetItem("Item_32").Specific));
            this.AuthenticationButton = ((SAPbouiCOM.Button)(this.GetItem("Item_29").Specific));
            this.AuthenticationButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.OnCustomInitialize();

        }

       
        public override void OnInitializeFormEvents()
        {

        }



        private void OnCustomInitialize()
        {

        }

        #endregion

        //get value from the EditText
        private void InitializeVariables()
        {
            par1 = Edit1.String;
            par2 = Edit2.String;
            par3 = Edit3.String;
            par4 = Edit4.String;
            par5 = Edit5.String;
            par6 = Edit6.String;
            par7 = Edit7.String;
            par8 = Edit8.String;
        }

        private void ResetButton(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            InitializeVariables();
            cntObj.getDate(StaticText11);
            SecondPar = "";

            try
            {
                CM_Obj.checkIfItemValueIsNull(listOfEditText);
            }

            catch (Exception e)
            {
                Application.SBO_Application.MessageBox(e.Message);
            }

        }

        private void ComboBox0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                if (isColored == true)
                {
                    CM_Obj.cleanRows(Grid0);
                }
                cntObj.getDate(StaticText11);
                CM_Obj.checkIfItemValueIsNull(listOfEditText);
                InitializeVariables();

             
                    if (ComboBox0.Selected.Description == "1")
                    {
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Indeks", "Numer surowy", "Numer gotowy", "Opis", "Magazyn", "Lokalizacja", "Kod kreskowy", "Partia klienta");
                        CM_Obj.changeMainLabel(StaticText8, "Stany na lokalizacjach");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.loadDataIntoTable(Grid0, par1, par2, par3, par4, par5, par6, par7, par8);
                        this.GetItem("Item_31").Visible = true;


                    }

                    else if (ComboBox0.Selected.Description == "2")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Nieprzelokalizowane detale");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Indeks", "Numer surowy", "Klient", "Magazyn", "Lokalizacja", "Kod kreskowy", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.detailsOnSP(Grid0, par1, par2, par3, par4, par5, par6);
                        this.GetItem("Item_31").Visible = false;

                    }

                    else if (ComboBox0.Selected.Description == "3")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Stany po numerze surowym");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Numer surowy", "Opis", "Klient", "N/D", "N/D", "N/D", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.u_DrawNoRawSumRaport(Grid0, par1, par2, par3);
                        this.GetItem("Item_31").Visible = false;

                    }

                    else if (ComboBox0.Selected.Description == "4")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Stany po numerze gotowym");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Numer gotowy", "Opis", "Klient", "N/D", "N/D", "N/D", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.u_DrawNoFinalSumRaport(Grid0, par1, par2, par3);
                        this.GetItem("Item_31").Visible = false;

                    }

                    else if (ComboBox0.Selected.Description == "5")
                    {
                      
                        CM_Obj.changeMainLabel(StaticText8, "Zamówienia działu zakupów");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Numer zamówienia", "Dostawca", "Waluta", "Uwagi", "Status", "Odział", "Przeznaczenie", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.purchaseOrdersRapport(Grid0, par1, par2, par3, par4, par5, par6, par7);
                        CM_Obj.fillWithColorsPurchaseOrder(Grid0, 5);
                        this.GetItem("Item_31").Visible = false;
                        isColored = true;
                     }

                    else if (ComboBox0.Selected.Description == "6")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Zamówienia magazynu chemicznego");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Numer zamówienia", "Dostawca", "Status", "Waluta", "Uwagi", "Odział", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.chemicalOrdersReport(Grid0, par1, par2, par3, par4, par5, par6);
                        CM_Obj.fillWithColorsChemicalOrders(Grid0, 9);
                        this.GetItem("Item_31").Visible = false;
                        isColored = true;

                    }

                    else if (ComboBox0.Selected.Description == "7")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Gospodarka materiałowa");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Klient", "Indeks", "Numer gotowy", "Opis", "N/D", "N/D", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.chemicalStocks(Grid0, par1, par2, par3, par4);
                        CM_Obj.fillWithColorsChemicalStock(Grid0, 9);
                        this.GetItem("Item_31").Visible = false;
                        isColored = true;

                    }

                    else if (ComboBox0.Selected.Description == "8")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Status zleceń zakupu");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Numer zlecenia zakupu", "N/D", "N/D", "N/D", "N/D", "N/D", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.orderStatusForCommoners(Grid0, par1);
                        CM_Obj.fillWithColorsPurchaseOrder(Grid0, 6);
                        this.GetItem("Item_31").Visible = false;
                        
                        isColored = true;

                    }

                    else if (ComboBox0.Selected.Description == "9")
                    {

                        CM_Obj.changeMainLabel(StaticText8, "Sumaryczny raport dostarczonych wyrobów");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Dostawca", "Logo", "Data od:", "Data do:", "N/D", "N/D", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        Edit3.String = "2017-01-01";
                        Edit4.String = "2020-01-01";
                        SqlExecutor.sqaDeliveredTotal(Grid0, par1, par2, par3, par4);
                        this.GetItem("Item_31").Visible = false;
                    }

                    else if (ComboBox0.Selected.Description == "10")
                    {
                        CM_Obj.changeMainLabel(StaticText8, "Lista zleceń zakupu");
                        CM_Obj.changeLabel(StaticText0, StaticText1, StaticText2, StaticText3, StaticText4, StaticText5, StaticText6, StaticText7, "Numer zlecenia zakupu", "Przeznaczenie", "Dział wystawiający", "Status", "N/D", "N/D", "N/D", "N/D");
                        CM_Obj.checkIfItemValueIsNull(listOfEditText);
                        SqlExecutor.OPRQForPurchaseDepartment(Grid0, par1, par2, par3, par4);
                        CM_Obj.fillWithColorsPurchaseOrder(Grid0, 8);
                        this.GetItem("Item_31").Visible = false;
                        isColored = true;
                    }

                
                }
            


            catch (Exception e)
            {
                 //Application.SBO_Application.MessageBox(e.Message);

            }
        }

        private void OnClickListener(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            dynamicColumnName = pVal.ColUID;
            try
            {

                int index = Grid0.GetDataTableRowIndex(pVal.Row); //get the row number on click
                SecondPar = Grid0.DataTable.GetValue(pVal.ColUID, index).ToString();  //get the specified col/row value
                String PurchasePar = Grid0.DataTable.GetValue(1, index).ToString();
                if (ComboBox0.Selected.Description == "5")
                {
                    SqlExecutor.fillSecondGridPurchase(Grid1, SecondPar);
                }

                else if (ComboBox0.Selected.Description == "6")
                {

                    SqlExecutor.fillSecondGridWitchChemicalDetails(Grid1, SecondPar);
                }

                else if (ComboBox0.Selected.Description == "7")
                {
                    SqlExecutor.fillSecondGridWitchChemicalStocks(Grid1, SecondPar);
                    //  Application.SBO_Application.MessageBox(SecondPar);
                }

                else if (ComboBox0.Selected.Description == "8")
                {
                    SqlExecutor.fillSecondGridWithOrderDetailsForCommoners(Grid1, PurchasePar);
                }

                else if (ComboBox0.Selected.Description == "9")
                {
                    SqlExecutor.sqaSecondGrid(Grid1, SecondPar, par3, par4);
                }

                else if (ComboBox0.Selected.Description == "10")
                {
                    SqlExecutor.fillSecondGridWithOrderDetailsForCommoners(Grid1, SecondPar);
                }


                else
                {
                    SqlExecutor.fillSecondGridDefault(Grid1, SecondPar);
                }
            }

            catch (Exception)
            {

            }

        }

        private void Edit1_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit1.String = SecondPar;

        }

        private void Edit2_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit2.String = SecondPar;

        }

        private void Edit3_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit3.String = SecondPar;

        }

        private void Edit4_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit4.String = SecondPar;

        }

        private void Edit5_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit5.String = SecondPar;

        }

        private void Edit6_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit6.String = SecondPar;

        }

        private void Edit7_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit7.String = SecondPar;

        }

        private void Edit8_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit8.String = SecondPar;

        }

        private void EditText8_DoubleClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            Edit0.String = SecondPar;

        }

        private void Grid0_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.GridColumn column;
            column = Grid0.Columns.Item(dynamicColumnName);
            column.TitleObject.Sortable = true;

            if (isSorted == false)
            {
                Grid0.Columns.Item(dynamicColumnName).TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Ascending);
                isSorted = true;
            }
            else
            {
                Grid0.Columns.Item(dynamicColumnName).TitleObject.Sort(SAPbouiCOM.BoGridSortType.gst_Descending);
                isSorted = false;
            }

        }

        private void FillTableButton_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            InitializeVariables();
            cntObj.getDate(StaticText11);
            SecondPar = "";
            if (isColored == true)
            {
                CM_Obj.cleanRows(Grid0);
            }

            try
            {
                if (ComboBox0.Selected.Description == "1")  //1 if "stany na lokalizacjach"
                {
                    SqlExecutor.loadDataIntoTable(Grid0, par1, par2, par3, par4, par5, par6, par7, par8);
                }

                else if (ComboBox0.Selected.Description == "2") //nieprzelokalizowane detale
                {
                    SqlExecutor.detailsOnSP(Grid0, par1, par2, par3, par4, par5, par6);
                }

                else if (ComboBox0.Selected.Description == "3")  //stany po numerze surowym
                {
                    SqlExecutor.u_DrawNoRawSumRaport(Grid0, par1, par2, par3);
                }

                else if (ComboBox0.Selected.Description == "4")  //stany po numerze gotowym
                {
                    SqlExecutor.u_DrawNoFinalSumRaport(Grid0, par1, par2, par3);
                }

                else if (ComboBox0.Selected.Description == "5")  //lista zamówień
                {
                    SqlExecutor.purchaseOrdersRapport(Grid0, par1, par2, par3, par4, par5, par6, par7);
                    CM_Obj.fillWithColorsPurchaseOrder(Grid0, 4);
                    isColored = true;
                }

                else if (ComboBox0.Selected.Description == "6")  //lista zamówień magazynu chemicznego
                {
                    SqlExecutor.chemicalOrdersReport(Grid0, par1, par2, par3, par4, par5, par6);
                    CM_Obj.fillWithColorsChemicalOrders(Grid0, 5);
                    isColored = true;
                }

                else if (ComboBox0.Selected.Description == "7")  //gospodarka materiałowa
                {
                    SqlExecutor.chemicalStocks(Grid0, par1, par2, par3, par4);
                    CM_Obj.fillWithColorsChemicalStock(Grid0, 9);
                    isColored = true;

                }

                else if (ComboBox0.Selected.Description == "8")  //status zamówień
                {
                    SqlExecutor.orderStatusForCommoners(Grid0, par1);
                    CM_Obj.fillWithColorsPurchaseOrder(Grid0, 6);
                    isColored = true;

                }

                else if (ComboBox0.Selected.Description == "9")  //sqa1
                {
                    SqlExecutor.sqaDeliveredTotal(Grid0, par1, par2, par3, par4);
                }

                else if (ComboBox0.Selected.Description == "10")  //zlecenia zakupu
                {
                    SqlExecutor.OPRQForPurchaseDepartment(Grid0, par1, par2, par3, par4);
                    CM_Obj.fillWithColorsPurchaseOrder(Grid0, 8);
                    isColored = true;
                   
                }
            }

            catch (Exception)
            {
                // Application.SBO_Application.MessageBox("Wybierz raport");
            }

        }

        private void Button2_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                // Grid0.Columns.Item("Indeks").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox; 
                // this.Grid0.Item.Width = 1200;
                // Grid0.Columns.Item("ItemCode").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox; 
                // authentication.returnDepartment(Grid2);

             
                Application.SBO_Application.ActivateMenuItem("ff");
                Application.SBO_Application.Forms.Item("F_21").Select();

            }

            catch (Exception e)
            {
                InfoBoxes.UseMessageBox(e.Message);
            }

        }

        private void CountTotal(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            InfoBoxes.UseMessageBox(cntObj.countTotal(Grid0, 8));
        }


        //dostosowanie ui pod użytkownika
        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.GetItem("Item_19").Enabled = true;
            
            if (!authentication.returnDepartment(Grid2).Equals("88"))
            {
                this.ComboBox0.ValidValues.Remove("Zamówienia działu zakupów");
                this.ComboBox0.ValidValues.Remove("Zamówienia magazynu chemicznego");
                this.ComboBox0.ValidValues.Remove("Zlecenia zakupu");
            }

            else
            {
                this.ComboBox0.ValidValues.Remove("Status zleceń zakupu");
            }
            this.GetItem("Item_29").Visible = false;
        }

    }
}
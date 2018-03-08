using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerBusiness.Shared;
using SAPbouiCOM.Framework;

namespace PowerBusiness
{
    [FormAttribute("140", "DeliveryNote.b1f")]
    class DeliveryNote : SystemFormBase
    {
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Button CountButton;
        public DeliveryNote()
        {
        }

        #region frameworkSpam

        public override void OnInitializeComponent()
        {
            this.CountButton = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.CountButton.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.CountButton_ClickBefore);
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("38").Specific));
            this.OnCustomInitialize();

        }

        #endregion
        public override void OnInitializeFormEvents()
        {
        }

        

        private void OnCustomInitialize()
        {

        }

        private void CountButton_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                InfoBoxes.UseMessageBox(CountTotal.Count(EditText0, Matrix0));
            }
            catch (Exception e)
            {
                InfoBoxes.UseMessageBox(e.Message);
            }

        }
    }
}

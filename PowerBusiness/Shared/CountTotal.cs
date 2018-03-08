using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBusiness.Shared
{
    public static class CountTotal
    {
        public static Double Count(SAPbouiCOM.EditText editText, SAPbouiCOM.Matrix matrix)
        {

            int rowNumbers = matrix.RowCount;
            string ColId = "10002117";
            double total = 0;
            String asd = "";
        

                for (int i = 1; i < rowNumbers + 1; i++)
                {
                    editText = (SAPbouiCOM.EditText)matrix.Columns.Item(ColId).Cells.Item(i).Specific;
                    asd = editText.Value;
                    double x = Convert.ToDouble(asd.Replace('.', ','));
                    total += x;

                }
            return total;

        }
    }
}

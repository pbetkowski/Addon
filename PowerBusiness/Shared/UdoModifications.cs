using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBusiness.Shared
{
    class UdoModifications
    {
        public void ColourMatrix(SAPbouiCOM.Matrix matrix, SAPbouiCOM.EditText ed1, SAPbouiCOM.EditText ed2, String columnId)
        {
            int rowNumber = matrix.RowCount;
            int color1 = 16777215;
            int color2 = 16766852;
            int currentColor = color1;

            matrix.CommonSetting.SetRowBackColor(1, color1);

            for (int i = 2; i < rowNumber; i++)
            {
                ed1 = (SAPbouiCOM.EditText)matrix.Columns.Item(columnId).Cells.Item(i - 1).Specific;
                ed2 = (SAPbouiCOM.EditText)matrix.Columns.Item(columnId).Cells.Item(i).Specific;
                String value = ed1.Value;
                String value2 = ed2.Value;

                if (value.Equals(value2))
                {
                    matrix.CommonSetting.SetRowBackColor(i, currentColor);
                }
                else
                {
                    //currentColor = currentColor == color1 ? color2 : color1;

                    if (currentColor == color1)
                    {
                        currentColor = color2;
                    }

                    else
                    {
                        currentColor = color1;
                    }

                    matrix.CommonSetting.SetRowBackColor(i, currentColor);
                }

            }
        }
    }
}

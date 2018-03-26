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
            String s1 = "";
            String s2 = "";
            int cnt = 1;
            int cnt2 = 0;
            for (int i = 0; i < rowNumber; i++)
            {

                matrix.CommonSetting.SetRowBackColor(i + 1, 16777215);        
            }

            for (int i = 0; i < rowNumber; i++)
            {
                ed1 = (SAPbouiCOM.EditText)matrix.Columns.Item(columnId).Cells.Item(i + 1).Specific;
                s1 = ed1.Value;
                if (i == 0)
                {
                    matrix.CommonSetting.SetRowBackColor(i + 1, 16777215);
                }

                else if (i > 0)
                {
                    ed2 = (SAPbouiCOM.EditText)matrix.Columns.Item(columnId).Cells.Item(i).Specific;

                    s1 = ed1.Value;
                    s2 = ed2.Value;

                    if (s1.Equals(s2) && cnt >= 2)
                    {

                        matrix.CommonSetting.SetRowBackColor(i, 16766852);
                        matrix.CommonSetting.SetRowBackColor(i + 1, 16766852);
                        cnt = 1;

                    }

                    else if (s1.Equals(s2) && cnt < 2)
                    {

                        matrix.CommonSetting.SetRowBackColor(i, 16777215);
                        matrix.CommonSetting.SetRowBackColor(i + 1, 16777215);
                        cnt++; cnt2 = 0;

                    }

                    else if (!s1.Equals(s2) & cnt2 < 1)
                    {
                        matrix.CommonSetting.SetRowBackColor(i + 1, 45555);
                        cnt2++;
                    }

                    else if (!s1.Equals(s2) & cnt2 >= 1)
                    {
                        matrix.CommonSetting.SetRowBackColor(i + 1, 7666766);
                        cnt2 = 0;
                    }
                }


                //old version
                //    for (int i = 0; i < rowNumber; i++)
                //    {
                //        EditText0 = (SAPbouiCOM.EditText)Matrix0.Columns.Item("C_0_3").Cells.Item(i + 1).Specific;

                //        String asd = EditText0.Value;

                //        int color = Int32.Parse(asd);

                //        if (color % 2 == 0)
                //        {
                //            Matrix0.CommonSetting.SetRowBackColor(i + 1, 16766852);  //kolorki
                //        }
                //    }

                //}

            }
        }
    }
}

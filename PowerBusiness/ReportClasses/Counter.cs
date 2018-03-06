using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBusiness
{
    public class Counter
    {
        Random random = new Random();

        protected long setRandom()
        {
            return random.Next(1, 999999910);
        }

        public void getDate(SAPbouiCOM.StaticText textDate)
        {
            String date = DateTime.Now.ToString();
            textDate.Caption = "Stan na " + date;
        }

        //counting values of certain column
        public Int32 countTotal(SAPbouiCOM.Grid Grid0, int colID)
        {
            int numberOfRows = Grid0.Rows.Count;
            Int32 total = 0;

            for (int i = 0; i < numberOfRows; i++)
            {
                Int32 currentValue = Convert.ToInt32(Grid0.DataTable.GetValue(colID, i));
                total += currentValue;
            }
            return total;
        }
    }
}

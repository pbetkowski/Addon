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
           //long rand;
           //rand = random.Next(1, 999999910);
           //return rand;

            return random.Next(1, 999999910);
        }

        public void getDate(SAPbouiCOM.StaticText textDate)
        {   
          
            String date = DateTime.Now.ToString();
            textDate.Caption = "Stan na " + date;

        }


        public int countTotal(SAPbouiCOM.Grid Grid0, int colID)
        {
            int numberOfRows = Grid0.Rows.Count;
            int total = 0;
            String result = "";

            for (int i = 0; i < numberOfRows; i++)
            {
                int x =  (int) Grid0.DataTable.GetValue(colID, i);
                total += x;
            }

            return total;
        }
    }
}

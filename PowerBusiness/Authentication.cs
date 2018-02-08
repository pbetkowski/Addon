using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBusiness
{
    class Authentication : Counter
    {
        private SAPbouiCOM.DataTable dataTable;
        SAPbouiCOM.Form form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
        long temporaryID;
        public String returnDepartment(SAPbouiCOM.Grid grid)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT\n" +
        " t2.\"Code\"\n" +
        "FROM\n" +
        "USR5 T0 \n" +
        "INNER JOIN OUSR T1 ON T0.\"UserCode\" = t1.USER_CODE\n" +
        "INNER JOIN OUDP t2 ON t1.\"Department\" = t2.\"Code\"\n" +
        "INNER JOIN M_CONNECTIONS MC ON MC.logical_connection_id = CURRENT_CONNECTION AND T0.\"ProcessID\" = MC.CLIENT_PID \n" +
        "AND T0.\"SessionID\" = CURRENT_CONNECTION");
            grid.DataTable = dataTable;
            String currentDepartment = grid.DataTable.GetValue(0, 0).ToString();
            dataTable = form.DataSources.DataTables.Add(base.setRandom().ToString());           
            return currentDepartment;
        }
    }
}

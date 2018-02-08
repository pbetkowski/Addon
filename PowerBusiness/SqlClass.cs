using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerBusiness
{
    public class SqlClass : Counter
    {

        long temporaryID;
        private SAPbouiCOM.DataTable dataTable;
        SAPbouiCOM.Form form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;

        //stare stany na lokalizacjach
        public void loadDataIntoTable(SAPbouiCOM.Grid gridPanel, String ItemCode, String U_DrawNoRaw, String U_DrawNoFinal, String ItemName, String WhsCode, String Localization, String DistNumber, String SuppNumber)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;

            dataTable.ExecuteQuery("--set schema \"SBOELECTROPOLI\"\n" +
         "WITH CTE1 AS\n" +
         "(--stan magazynu wraz z ostatnim dokumentem na indeksie\n" +
         "SELECT\n" +
         " T6.\"ItemCode\", MAX(T5.\"TransNum\") AS \"TransNum\",\n" +
         " T2.\"OnHandQty\", T3.\"AbsEntry\" AS \"EntryLokalizacji\", T3.\"WhsCode\",\n" +
         " T3.\"BinCode\" ,\n" +
         " T4.\"DistNumber\", \n" +
         " T4.\"U_SupNumber\", \n" +
         " T4.\"AbsEntry\",\n" +
         " T6.\"U_DrawNoRaw\", \n" +
         " T6.\"U_DrawNoFinal\", \n" +
         " T6.\"ItemName\", \n" +
         " T4.\"Status\"\n" +
         "FROM OITL T0\n" +
         " LEFT OUTER JOIN ITL1 T1 ON T1.\"LogEntry\"=T0.\"LogEntry\"\n" +
         " LEFT OUTER JOIN OBBQ T2 ON T1.\"MdAbsEntry\" = T2.\"SnBMDAbs\"\n" +
         " LEFT OUTER JOIN OBIN T3 ON T2.\"BinAbs\" = T3.\"AbsEntry\"\n" +
         " LEFT OUTER JOIN OBTN T4 ON T2.\"SnBMDAbs\"=t4.\"AbsEntry\"\n" +
         " LEFT OUTER JOIN OINM T5 ON T0.\"DocEntry\" = T5.\"CreatedBy\" AND T0.\"DocLine\" = T5.\"DocLineNum\" AND T0.\"DocType\" = T5.\"TransType\"\n" +
         " LEFT OUTER JOIN OITM T6 ON T0.\"ItemCode\" = T6.\"ItemCode\"\n" +
         "WHERE\n" +
         "T2.\"OnHandQty\" <> 0\n" +
         "GROUP BY\n" +
         " T6.\"ItemCode\", T2.\"OnHandQty\", T3.\"AbsEntry\", T3.\"WhsCode\", T3.\"BinCode\", T4.\"DistNumber\", T4.\"U_SupNumber\", T4.\"AbsEntry\",\n" +
         " T6.\"U_DrawNoRaw\", T6.\"U_DrawNoFinal\", T6.\"ItemName\", T4.\"Status\"\n" +
         "),\n" +
         "\n" +
         "CTE2 AS\n" +
         "(--informacje o dokumentach magazynowych\n" +
         "SELECT \n" +
         " T0.\"TransNum\", T0.\"TransType\", T0.\"CreatedBy\", T0.\"BASE_REF\", T1.\"USER_CODE\", T0.\"DocDate\", RIGHT('00'||T0.\"DocTime\",4) AS \"DocTime\"\n" +
         "FROM OINM T0\n" +
         " LEFT OUTER JOIN OUSR T1 ON T0.\"UserSign\" = T1.\"USERID\"\n" +
         "),\n" +
         "\n" +
         "CTE3 AS (--kody operacji i zasobów dla marszrut - do porówania z marszrutami na zleceniach produkcyjnych\n" +
         "SELECT DISTINCT--pozycje receptur\n" +
         " T1.\"U_ItemCode\", T3.\"U_OprSequence\", T2.\"U_RtgCode\", T4.\"U_RscCode\"\n" +
         ", IFNULL(T5.\"U_OprCode\",'SUR') AS \"PrevOpr\"\n" +
         ", IFNULL(T6.\"U_OprCode\",'GOT') AS \"NextOpr\"\n" +
         "FROM \"@CT_PF_OBOM\" T0\n" +
         " INNER JOIN \"@CT_PF_BOM1\" T1 ON T0.\"Code\" = T1.\"Code\"\n" +
         " INNER JOIN \"@CT_PF_BOM11\" T2 ON T0.\"Code\" = T2.\"Code\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM12\" T3 ON T2.\"Code\" = T3.\"Code\" AND T2.\"U_RtgCode\" = T3.\"U_RtgCode\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM16\" T4 ON T3.\"Code\" = T4.\"Code\" AND T3.\"U_RtgCode\" = T4.\"U_RtgCode\" AND T3.\"U_OprCode\" = T4.\"U_OprCode\"\n" +
         " LEFT OUTER JOIN (SELECT DISTINCT--kod operacji poprzedniej\n" +
         " \t\t\t\t\tT1.\"Code\", T1.\"U_RtgCode\", T1.\"U_RtgOprCode\" + 1 AS \"U_RtgOprCode\", T1.\"U_OprSequence\", T1.\"U_OprCode\" \n" +
         "\t\t\t\t  FROM \"@CT_PF_BOM11\" T0\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM12\" T1 ON T0.\"Code\" = T1.\"Code\" AND T0.\"U_RtgCode\" = T1.\"U_RtgCode\"\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM1\" T2 ON T1.\"Code\" = T2.\"Code\"\n" +
         "\t\t\t\t  ) T5 ON T2.\"Code\" = T5.\"Code\" \n" +
         "\t\t\t\t  AND T3.\"U_RtgOprCode\" = T5.\"U_RtgOprCode\" AND T2.\"U_RtgCode\" = T5.\"U_RtgCode\"\n" +
         " LEFT OUTER JOIN (SELECT DISTINCT--kod operacji następnej\n" +
         " \t\t\t\t\tT1.\"Code\", T1.\"U_RtgCode\", T1.\"U_RtgOprCode\" - 1 AS \"U_RtgOprCode\", T1.\"U_OprSequence\", T1.\"U_OprCode\" \n" +
         "\t\t\t\t  FROM \"@CT_PF_BOM11\" T0\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM12\" T1 ON T0.\"Code\" = T1.\"Code\" AND T0.\"U_RtgCode\" = T1.\"U_RtgCode\"\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM1\" T2 ON T1.\"Code\" = T2.\"Code\"\n" +
         "\t\t\t\t  ) T6 ON T2.\"Code\" = T6.\"Code\" \n" +
         "\t\t\t\t  AND T3.\"U_RtgOprCode\" = T6.\"U_RtgOprCode\" AND T2.\"U_RtgCode\" = T6.\"U_RtgCode\"\n" +
         "\n" +
         "UNION ALL\n" +
         "\n" +
         "SELECT DISTINCT--nagłówki receptur\n" +
         " T0.\"U_ItemCode\", T3.\"U_OprSequence\", T2.\"U_RtgCode\", T4.\"U_RscCode\"\n" +
         ", IFNULL(T5.\"U_OprCode\",'SUR') AS \"PrevOpr\"\n" +
         ", IFNULL(T6.\"U_OprCode\",'GOT') AS \"NextOpr\"\n" +
         "FROM \"@CT_PF_OBOM\" T0\n" +
         " INNER JOIN \"@CT_PF_BOM1\" T1 ON T0.\"Code\" = T1.\"Code\"\n" +
         " INNER JOIN \"@CT_PF_BOM11\" T2 ON T0.\"Code\" = T2.\"Code\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM12\" T3 ON T2.\"Code\" = T3.\"Code\" AND T2.\"U_RtgCode\" = T3.\"U_RtgCode\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM16\" T4 ON T3.\"Code\" = T4.\"Code\" AND T3.\"U_RtgCode\" = T4.\"U_RtgCode\" AND T3.\"U_OprCode\" = T4.\"U_OprCode\"\n" +
         " LEFT OUTER JOIN (SELECT DISTINCT--kod operacji poprzedniej\n" +
         " \t\t\t\t\tT1.\"Code\", T1.\"U_RtgCode\", T1.\"U_RtgOprCode\" + 1 AS \"U_RtgOprCode\", T1.\"U_OprSequence\", T1.\"U_OprCode\" \n" +
         "\t\t\t\t  FROM \"@CT_PF_BOM11\" T0\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM12\" T1 ON T0.\"Code\" = T1.\"Code\" AND T0.\"U_RtgCode\" = T1.\"U_RtgCode\"\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM1\" T2 ON T1.\"Code\" = T2.\"Code\"\n" +
         "\t\t\t\t  ) T5 ON T2.\"Code\" = T5.\"Code\" \n" +
         "\t\t\t\t  AND T3.\"U_RtgOprCode\" = T5.\"U_RtgOprCode\" AND T2.\"U_RtgCode\" = T5.\"U_RtgCode\"\n" +
         " LEFT OUTER JOIN (SELECT DISTINCT--kod operacji następnej\n" +
         " \t\t\t\t\tT1.\"Code\", T1.\"U_RtgCode\", T1.\"U_RtgOprCode\" - 1 AS \"U_RtgOprCode\", T1.\"U_OprSequence\", T1.\"U_OprCode\" \n" +
         "\t\t\t\t  FROM \"@CT_PF_BOM11\" T0\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM12\" T1 ON T0.\"Code\" = T1.\"Code\" AND T0.\"U_RtgCode\" = T1.\"U_RtgCode\"\n" +
         "   \t\t\t\t  INNER JOIN \"@CT_PF_BOM1\" T2 ON T1.\"Code\" = T2.\"Code\"\n" +
         "\t\t\t\t  ) T6 ON T2.\"Code\" = T6.\"Code\" \n" +
         "\t\t\t\t  AND T3.\"U_RtgOprCode\" = T6.\"U_RtgOprCode\" AND T2.\"U_RtgCode\" = T6.\"U_RtgCode\"\n" +
         "ORDER BY T2.\"U_RtgCode\", T3.\"U_OprSequence\"\n" +
         "),\n" +
         "\n" +
         "CTE4 AS (--transakcje magazynowe powiązane ze zleceniem produkcyjnym\n" +
         "SELECT\n" +
         " \"DocEntry\", \"ObjType\", \"ItemCode\", CAST(\"U_DocEntry\" AS NVARCHAR(10)) AS \"U_DocEntry\"\n" +
         "FROM WTR1\n" +
         "\n" +
         "\n" +
         "UNION ALL\n" +
         "\n" +
         "SELECT\n" +
         " \"DocEntry\", \"ObjType\", \"ItemCode\", CAST(\"U_DocEntry\" AS NVARCHAR(10)) AS \"U_DocEntry\"\n" +
         "FROM IGE1\n" +
         "\n" +
         "\n" +
         "UNION ALL\n" +
         "\n" +
         "SELECT\n" +
         " \"DocEntry\", \"ObjType\", \"ItemCode\", CAST(\"U_DocEntry\" AS NVARCHAR(10)) AS \"U_DocEntry\"\n" +
         "FROM IGN1\n" +
         "\n" +
         "),\n" +
         "\n" +
         "\n" +
         "\n" +
         "CTE5 AS (--marszruty na zleceniu produkcyjnym\n" +
         "SELECT \"DocEntry\", \"U_ItemCode\", \"U_RtgCode\" FROM \"@CT_PF_OMOR\"\n" +
         "\n" +
         "\n" +
         "UNION ALL\n" +
         "\n" +
         "SELECT T0.\"DocEntry\", T1.\"U_ItemCode\", T0.\"U_RtgCode\" \n" +
         "FROM \"@CT_PF_OMOR\" T0\n" +
         " INNER JOIN \"@CT_PF_MOR3\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\"\n" +
         "\n" +
         "),\n" +
         "\n" +
         "CTE6 AS (--operacje dla indeksów bez zlecenia produkcyjnego\n" +
         "SELECT--pozycje receptur\n" +
         " T1.\"U_ItemCode\", \n" +
         " T2.\"U_OprCode\", T3.\"U_RtgCode\"\n" +
         "FROM \"@CT_PF_OBOM\" T0\n" +
         " INNER JOIN \"@CT_PF_BOM1\" T1 ON T0.\"Code\" = T1.\"Code\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM12\" T2 ON T0.\"Code\" = T2.\"Code\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM11\" T3 ON T2.\"Code\" = T3.\"Code\" AND T2.\"U_RtgCode\" = T3.\"U_RtgCode\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM16\" T4 ON T3.\"Code\" = T4.\"Code\" AND T2.\"U_RtgCode\" = T4.\"U_RtgCode\" AND T2.\"U_OprCode\" = T4.\"U_OprCode\"\n" +
         "WHERE T3.\"U_IsDefault\" = 'Y' AND T4.\"U_IsDefault\" = 'Y' AND T2.\"U_OprSequence\" = 10\n" +
         "\n" +
         "UNION ALL\n" +
         "\n" +
         "SELECT--pozycje receptur\n" +
         " T0.\"U_ItemCode\", \n" +
         " T2.\"U_OprCode\", T3.\"U_RtgCode\"\n" +
         "FROM \"@CT_PF_OBOM\" T0\n" +
         " INNER JOIN \"@CT_PF_BOM1\" T1 ON T0.\"Code\" = T1.\"Code\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM12\" T2 ON T0.\"Code\" = T2.\"Code\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM11\" T3 ON T2.\"Code\" = T3.\"Code\" AND T2.\"U_RtgCode\" = T3.\"U_RtgCode\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_BOM16\" T4 ON T3.\"Code\" = T4.\"Code\" AND T2.\"U_RtgCode\" = T4.\"U_RtgCode\" AND T2.\"U_OprCode\" = T4.\"U_OprCode\"\n" +
         "WHERE T3.\"U_IsDefault\" = 'Y' AND T4.\"U_IsDefault\" = 'Y' AND T2.\"U_OprSequence\" = 10\n" +
         "),\n" +
         "\n" +
         "CTE7 AS (--nazwy obiektów\n" +
         "SELECT '13' AS \"ObjType\", N'FA' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '14' AS \"ObjType\", N'AFA' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '15' AS \"ObjType\", N'WZ' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '16' AS \"ObjType\", N'ZW' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '165' AS \"ObjType\", N'FK' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '166' AS \"ObjType\", N'SFK' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '67' AS \"ObjType\", N'MM' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '59' AS \"ObjType\", N'PW' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '60' AS \"ObjType\", N'RW' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '20' AS \"ObjType\", N'PZ' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '21' AS \"ObjType\", N'ZPZ' AS \"ObjName\" FROM DUMMY\n" +
         "UNION ALL\n" +
         "SELECT '10000071' AS \"ObjType\", N'INW' AS \"ObjName\" FROM DUMMY\n" +
         "),\n" +
         "\n" +
         "CTE9 AS( \n" +
         "SELECT DISTINCT\n" +
         " T3.\"U_OprSequence\", \n" +
         " IFNULL(T3.\"U_RtgCode\", T5.\"U_RtgCode\") AS \"U_RtgCode\", T3.\"U_RscCode\"\n" +
         ", T3.\"PrevOpr\"\n" +
         ", T3.\"NextOpr\"\n" +
         ",CAST(T5.\"DocEntry\" AS NVARCHAR(10)) AS \"DocEntry\"\n" +
         "FROM CTE5 T5\n" +
         " INNER JOIN CTE3 T3 ON T5.\"U_ItemCode\" = T3.\"U_ItemCode\" AND T5.\"U_RtgCode\" = T3.\"U_RtgCode\"\n" +
         "ORDER BY IFNULL(T3.\"U_RtgCode\", T5.\"U_RtgCode\"),T3.\"U_OprSequence\"\n" +
         "),\n" +
         "\n" +
         "CTE10 AS ( --zawieszone /zdjęte/wyprodukowane\n" +
         "select t1.\"DocEntry\" \"Link\", t0.\"DocNum\",t0.\"U_ItemCode\" ,\n" +
         " t0.\"U_Description\" ,t0.\"U_Quantity\",\"U_Status\" ,receipted.\"Przyjęte\" \"Produced Quantity\" ,\n" +
         "\n" +
         "draft.\"Zamiówione\",\n" +
         "mm.\"Przeniesione\"\n" +
         " \n" +
         "from \"@CT_PF_OMOR\" t0\n" +
         "left Outer join OWOR t1 on cast(t0.\"DocEntry\" as nvarchar(20))=t1.\"U_MnfOrdDocEntry\"\n" +
         "left outer join \n" +
         "(\n" +
         "select \"U_DocEntry\", sum(\"OpenQty\")   \"Zamiówione\"\n" +
         "from\n" +
         "wtq1 \n" +
         "where ifnull(\"U_DocEntry\",'0')<>'0'\n" +
         "group by \"U_DocEntry\"\n" +
         ") draft on cast(t0.\"DocEntry\" as nvarchar(20))=draft.\"U_DocEntry\"\n" +
         "left outer join \n" +
         "(\n" +
         "select \"U_DocEntry\" ,sum(\"Quantity\") \"Przeniesione\"\n" +
         "from\n" +
         "WTR1 \n" +
         "where ifnull(\"U_DocEntry\",'0')<>'0'\n" +
         "group by \"U_DocEntry\"\n" +
         ") mm  on cast(t0.\"DocEntry\" as nvarchar(20))=mm.\"U_DocEntry\"\n" +
         "left outer join \n" +
         "(\n" +
         "select \"U_DocEntry\",sum(\"Quantity\") \"Przyjęte\"\n" +
         "from\n" +
         "IGN1 \n" +
         "where ifnull(\"U_DocEntry\",'0')<>'0'\n" +
         "group by \"U_DocEntry\"\n" +
         ") receipted on cast(t0.\"DocEntry\" as nvarchar(20))=receipted.\"U_DocEntry\"\n" +
         "\n" +
         ")\n" +
         "\n" +
         "\n" +
         "SELECT DISTINCT\n" +
         "  T0.\"ItemCode\",\n" +
         "  T0.\"U_DrawNoRaw\" AS \"Surowy\", \n" +
         "  T0.\"U_DrawNoFinal\" AS \"Gotowy\",\n" +
         "  T0.\"ItemName\" AS \"Opis\",\n" +
         "  T0.\"WhsCode\" AS \"Magazyn\" \n" +
         "  --MPASZ\n" +
         "   , case when T0.\"ItemCode\" like 'WG-%' and T0.\"WhsCode\" in('MPR01', 'MPR01-N') and IFNULL(IFNULL(T5.\"NextOpr\",T6.\"U_OprCode\"),'SUR') = 'GOT'\n" +
         " \t\t\tthen T0.\"BinCode\"||'_ODB'\n" +
         " \t\t\telse T0.\"BinCode\"\n" +
         " end AS \"Lokalizacja\" ,\n" +
         " --MPASZEND\n" +
         "  --T0.\"BinCode\" AS \"Lokalizacja\",\n" +
         "  T0.\"DistNumber\" AS \"Kod kreskowy\",\n" +
         "  T0.\"U_SupNumber\" AS \"Nr partii klienta\",\n" +
         "  T0.\"OnHandQty\" AS \"Stan\", \n" +
         "  IFNULL(T5.\"U_RtgCode\",T6.\"U_RtgCode\") AS \"Technologia\",\n" +
         "  T1.\"USER_CODE\" AS \"Użytkownik\",\n" +
         "  CAST(FLOOR(ABS(SECONDS_BETWEEN\n" +
         "  (TO_SECONDDATE(NOW()),\n" +
         "  TO_SECONDDATE(CAST(T1.\"DocDate\" AS DATE)||' '|| TO_TIME(T1.\"DocTime\"))))/86400) AS INT) AS \"Przeterminowane (dni)\"\n" +
         "FROM CTE1 T0\n" +
         " LEFT OUTER JOIN CTE2 T1 ON T0.\"TransNum\" = T1.\"TransNum\"\n" +
         " LEFT OUTER JOIN \"@CT_PF_ORSC\" T2 ON T0.\"EntryLokalizacji\"=T2.\"U_BinAbs\"\n" +
         " LEFT OUTER JOIN CTE4 T4 ON T1.\"TransType\" = T4.\"ObjType\" AND T1.\"CreatedBy\" = T4.\"DocEntry\" AND T0.\"ItemCode\" = T4.\"ItemCode\"\n" +
         " LEFT OUTER JOIN CTE9 T5 ON IFNULL(T2.\"U_RscCode\",T0.\"BinCode\") = T5.\"U_RscCode\" AND T4.\"U_DocEntry\" = T5.\"DocEntry\"\n" +
         " LEFT OUTER JOIN CTE6 T6 ON T0.\"ItemCode\" = T6.\"U_ItemCode\"\n" +
         " LEFT OUTER JOIN CTE7 T7 ON T1.\"TransType\" = T7.\"ObjType\"\n" +
         " WHERE T0.\"ItemCode\" LIKE '%" + ItemCode + "%' AND T0.\"U_DrawNoRaw\" LIKE '%" + U_DrawNoRaw + "%' AND T0.\"U_DrawNoFinal\" LIKE '%" + U_DrawNoFinal + "%' AND T0.\"ItemName\" LIKE '%" + ItemName + "%' AND T0.\"WhsCode\" LIKE '%" + WhsCode + "%'\n" +
         " AND T0.\"DistNumber\" LIKE '%" + DistNumber + "%' AND T0.\"U_SupNumber\" LIKE '%" + SuppNumber + "%' AND T0.\"BinCode\" LIKE '%" + Localization + "%' \n");
            gridPanel.DataTable = dataTable;
            SAPbouiCOM.EditTextColumn column = (SAPbouiCOM.EditTextColumn)gridPanel.Columns.Item("ItemCode");
            column.LinkedObjectType = "4";



        }


        //nieprzelokalizowane detale
        public void detailsOnSP(SAPbouiCOM.Grid gridPanel, String ItemCode, String U_DrawNoRaw, String Client, String WhsCode, String Localization, String DistNumber)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT\n" +
            "\t DISTINCT T0.\"ItemCode\" \"Indeks\",\n" +
            "\t T4.\"U_DrawNoRaw\" \"Numer surowy\", \n" +
            "\t T0.\"OnHandQty\" AS \"Ilość\",\n" +
            "\t T0.\"WhsCode\" \"Magazyn\",\n" +
            "\t T3.\"BinCode\" AS \"Lokalizacja\",\n" +
            "\t T2.\"DistNumber\" AS \"Kod kreskowy\",\n" +
            "\t SUBSTR (t0.\"ItemCode\", 4, 5) \"Logo\",\n" +
            "\t t5.\"CardName\" \"Klient\"\n" +
            "FROM OBBQ T0 \n" +
            "LEFT OUTER JOIN OBTQ T1 ON T0.\"ItemCode\" = T1.\"ItemCode\" \n" +
            "AND T0.\"SnBMDAbs\" = T1.\"MdAbsEntry\" \n" +
            "LEFT OUTER JOIN OBTN T2 ON T1.\"ItemCode\" = T2.\"ItemCode\" \n" +
            "AND T1.\"SysNumber\" = T2.\"SysNumber\" \n" +
            "LEFT OUTER JOIN OBIN T3 ON T0.\"BinAbs\" = T3.\"AbsEntry\" \n" +
            "LEFT OUTER JOIN OITM T4 ON T0.\"ItemCode\" = T4.\"ItemCode\" \n" +
            "INNER JOIN OCRD t5 ON SUBSTR (t0.\"ItemCode\", 4, 5) = RIGHT (t5.\"CardCode\",5)\n" +
            "WHERE T3.\"BinCode\" LIKE 'MSU%SP%' AND T0.\"OnHandQty\" <> 0 AND T0.\"ItemCode\" LIKE '%" +
            ItemCode + "%' AND T4.\"U_DrawNoRaw\" LIKE '%" + U_DrawNoRaw + "%' AND T0.\"WhsCode\" LIKE '%" + WhsCode + "%' AND T3.\"BinCode\" LIKE '%" + Localization + "%' AND t5.\"CardName\" LIKE '%" + Client + "%' AND T2.\"DistNumber\" LIKE '%" + DistNumber + "%'");
            gridPanel.DataTable = dataTable;
        }

        public void u_DrawNoRawSumRaport(SAPbouiCOM.Grid gridPanel, String U_DrawNoRaw, String ItemName, String Client)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT\n" +
            "IFNULL (t0.\"U_DrawNoRaw\", 'Brak rysunku') \"Surowy\",\n" +
            "t0.\"ItemName\" \"Opis\",\n" +
            "SUM (t0.\"OnHand\") AS \"Ilość\",\n" +
            "RIGHT (t1.\"CardCode\", 5) \"Logo\",\n" +
            "t1.\"CardName\" \"Klient\"\n" +
            "FROM OITM t0\n" +
            "INNER JOIN OCRD t1 ON SUBSTR (t0.\"ItemCode\", 4, 5) = RIGHT (t1.\"CardCode\", 5)\n" +
            "WHERE \"OnHand\" > 0 AND t0.\"U_DrawNoRaw\" LIKE '%" + U_DrawNoRaw + "%'AND t0.\"ItemName\" LIKE '%" + ItemName + "%' AND t1.\"CardName\" LIKE '%" + Client + "%' \n" +
            "GROUP BY t0.\"U_DrawNoRaw\", t0.\"ItemName\", t1.\"CardName\", t1.\"CardCode\"");
            gridPanel.DataTable = dataTable;
        }

        //stany dla numeru gotowego
        public void u_DrawNoFinalSumRaport(SAPbouiCOM.Grid gridPanel, String U_DrawNoFinal, String ItemName, String Client)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT\n" +
            "IFNULL (t0.\"U_DrawNoFinal\", 'Brak rysunku') \"Gotowy\",\n" +
            "t0.\"ItemName\",\n" +
            "SUM (t0.\"OnHand\") AS \"Ilość\",\n" +
            "RIGHT (t1.\"CardCode\", 5) \"Logo\",\n" +
            "t1.\"CardName\" \"Klient\"\n" +
            "FROM OITM t0\n" +
            "INNER JOIN OCRD t1 ON SUBSTR (t0.\"ItemCode\", 4, 5) = RIGHT (t1.\"CardCode\", 5)\n" +
             "WHERE \"OnHand\" > 0 AND t0.\"U_DrawNoFinal\" LIKE '%" + U_DrawNoFinal + "%' AND t0.\"ItemName\" LIKE '%" + ItemName + "%' AND t1.\"CardName\" LIKE '%" + Client + "%' \n" +
            "GROUP BY t0.\"U_DrawNoFinal\", t0.\"ItemName\", t1.\"CardName\", t1.\"CardCode\"");
            gridPanel.DataTable = dataTable;

        }

        //stany dla danego numeru surowego
        public void fillSecondGridDefault(SAPbouiCOM.Grid gridPanel, String U_DrawNoRaw)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT\n" +
           "IFNULL (t0.\"U_DrawNoFinal\", 'Brak rysunku') \"Gotowy\",\n" +
           "t0.\"ItemName\" \"Opis\",\n" +
           "SUM (t0.\"OnHand\") AS \"Ilość\",\n" +
           "RIGHT (t1.\"CardCode\", 5) \"Logo\",\n" +
           "t1.\"CardName\" \"Klient\"\n" +
           "FROM OITM t0\n" +
           "INNER JOIN OCRD t1 ON SUBSTR (t0.\"ItemCode\", 4, 5) = RIGHT (t1.\"CardCode\", 5)\n" +
           "WHERE \"OnHand\" > 0 AND t0.\"U_DrawNoFinal\" LIKE '" + U_DrawNoRaw + "' \n" +
           "GROUP BY t0.\"U_DrawNoFinal\", t0.\"ItemName\", t1.\"CardName\", t1.\"CardCode\"");
            gridPanel.DataTable = dataTable;

        }
        //szczegóły zamówień
        public void fillSecondGridPurchase(SAPbouiCOM.Grid gridPanel, String OrderNumber)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT\n" +
             "t0.\"ItemCode\" \"Indeks\",\n" +
              "t0.\"FreeTxt\" \"Opis (ręczny)\",\n" +
               "t0.\"Quantity\" \"Ilość\",\n" +
               "t0.\"Price\" \"Cena jedn.\",\n" +
                "t0.\"LineTotal\" \"Wartość\",\n" +
                "t0.\"Currency\" \"Waluta\"\n" +
                  "\n" +
                "FROM POR1 t0\n" +
             "INNER JOIN OPOR t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
              "WHERE t1.\"DocNum\" = " + OrderNumber + "");
            gridPanel.DataTable = dataTable;
        }
        //zamówienia działu zakupów
        public void purchaseOrdersRapport(SAPbouiCOM.Grid gridPanel, String OrderNumber, String Supplier, String Currency, String Comments, String Status, String Branch)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("WITH TEMP AS (\n" +
            "\n" +
            "SELECT DISTINCT \n" +
            "t0.\"DocEntry\" \"Link\",\n" +
            "t0.\"DocNum\" \"Numer zamówienia\",\n" +
            "t0.\"DocDate\"  \"Data zamówienia\",\n" +
            "t0.\"CardName\" \"Dostawca\",\n" +
            "SUM (t1.\"OpenQty\" * t1.\"Price\") AS \"Wartość zamówienia\",\n" +
            "t0.\"DocCur\" \"Waluta\",\n" +
            "t0.\"U_Purchase_Comments\" \"Uwagi\",\n" +
            "(CASE WHEN (t0.\"U_Status_Zam\" = '1') THEN 'Nowe zamówienie'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '2') THEN 'Dyr_Zakładu'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '3') THEN 'Dyr_Zak/Log'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '4') THEN 'Dyr_Finansowy'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '5') THEN 'Zarząd'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '6') THEN 'OK'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '7') THEN 'Zablokowane'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '8') THEN 'W toku'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '9') THEN 'Realizacja częściowa'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '10') THEN 'Zrealizowane'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '11') THEN 'Faktura'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '12') THEN 'Archiwum' \n" +
            "END) AS \"Status\",\n" +
            "\n" +
            "t0.\"BPLName\" \"Oddział\",\n" +
            "t2.\"SeriesName\" \"Typ zamówienia\",\n" +
            "t0.\"DocEntry\"\n" +
            "FROM OPOR t0\n" +
            "INNER JOIN POR1 t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
            "INNER JOIN NNM1 t2 ON t0.\"Series\" = t2.\"Series\"\n" +
            "GROUP BY t0.\"DocCur\", t0.\"DocNum\", t0.\"DocDate\", t0.\"CardName\", t0.\"U_Purchase_Comments\", t0.\"U_Status_Zam\", t0.\"BPLName\" , t2.\"SeriesName\", t0.\"DocEntry\"\n" +
            "\n" +
            "),\n" +
            "\n" +
            "\n" +
            "\n" +
            "TEMP2 AS (\n" +
            "\n" +
            "SELECT DISTINCT \"DocEntry\", COUNT (*) AS \"Ilość pozycji\" FROM POR1 GROUP BY \"DocEntry\"\n" +
            "\n" +
            "),\n" +
            "\n" +
            "\n" +
            "TEMP3 AS (\n" +
            "\n" +
            "SELECT \"DocEntry\", count(\"DocEntry\") AS \"Zrealizowano\"\n" +
            "FROM POR1 WHERE \"LineStatus\" = 'C' GROUP BY \"DocEntry\"\n" +
            "\n" +
            ")\n" +
            "\n" +
            "SELECT * FROM (SELECT DISTINCT \n" +
            "\n" +
            "t1.\"Link\",\n" +
            "t1.\"Numer zamówienia\",\n" +
            "t1.\"Data zamówienia\",\n" +
            "t1.\"Dostawca\",\n" +
            "t1.\"Wartość zamówienia\",\n" +
            "t1.\"Waluta\",\n" +
            "t1.\"Uwagi\",\n" +
            "t1.\"Status\",\n" +
            "t1.\"Oddział\",\n" +
            "t1.\"Typ zamówienia\"\n" +
            "FROM TEMP t1 \n" +
            "INNER JOIN TEMP2 t2 on t1.\"DocEntry\" = t2.\"DocEntry\"\n" +
            "LEFT OUTER JOIN TEMP3 t3 on t1.\"DocEntry\" = t3.\"DocEntry\") AS table\n" +
            "WHERE (table.\"Typ zamówienia\" LIKE 'ZAK-BB' OR table.\"Typ zamówienia\" LIKE 'ZAK-NS') AND table.\"Numer zamówienia\" LIKE '" + OrderNumber + "%' AND table.\"Dostawca\" LIKE '%" + Supplier + "%' AND table.\"Waluta\" LIKE '%" + Currency + "%' AND IFNULL (\"Uwagi\", '1') LIKE '%" + Comments + "%' AND IFNULL (\"Status\", '1') LIKE '%" + Status + "%' AND table.\"Oddział\" LIKE '%" + Branch + "%'");
            gridPanel.DataTable = dataTable;
            SAPbouiCOM.EditTextColumn column = (SAPbouiCOM.EditTextColumn)gridPanel.Columns.Item("Link");
            column.LinkedObjectType = "22";
        }

        //zamówienia magazynu chemicznego
        public void chemicalOrdersReport(SAPbouiCOM.Grid gridPanel, String OrderNumber, String Supplier, String Status, String Currency, String Comments, String Branch)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("\n" +
            "\n" +
            "WITH TEMP0 AS (\n" +
            " \n" +
            " SELECT DISTINCT \n" +
            " t0.\"BaseDocNum\",\n" +
            " SUM (t0.\"Quantity\") AS \"Ilość\"\n" +
            " FROM PDN1 t0\n" +
            " GROUP BY t0.\"BaseDocNum\"\n" +
            " ),\n" +
            "\n" +
            "TEMP AS (\n" +
            "\n" +
            "SELECT DISTINCT \n" +
            "t0.\"DocNum\" \"Numer zamówienia\",\n" +
            "t0.\"DocDate\"  \"Data zamówienia\",\n" +
            "t0.\"CardName\" \"Dostawca\",\n" +
            "SUM (t1.\"OpenQty\" * t1.\"Price\") AS \"Wartość zamówienia\",\n" +
            "t0.\"DocCur\" \"Waluta\",\n" +
            "t0.\"U_Purchase_Comments\" \"Uwagi\",\n" +
            "t0.\"BPLName\" \"Oddział\",\n" +
            "t2.\"SeriesName\" \"Typ zamówienia\",\n" +
            "t0.\"DocEntry\",\n" +
            "(CASE WHEN (SUM (t1.\"Quantity\") = SUM (t4.\"Ilość\")) THEN 'Zrealizowane'\n" +
            "\t  WHEN (SUM (t1.\"Quantity\") > SUM (t4.\"Ilość\") AND SUM (t4.\"Ilość\") <> 0) THEN 'Częściowo'\n" +
            "\t  WHEN (SUM (t4.\"Ilość\") IS NULL) THEN 'Niezrealizowane'\n" +
            "\t  WHEN (SUM (t1.\"Quantity\") < SUM (t4.\"Ilość\")) THEN 'Przekroczone'\n" +
            "END) AS \"Status\"\n" +
            "FROM OPOR t0\n" +
            "INNER JOIN POR1 t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
            "INNER JOIN NNM1 t2 ON t0.\"Series\" = t2.\"Series\"\n" +
            "LEFT OUTER JOIN TEMP0 t4 ON t0.\"DocNum\" = t4.\"BaseDocNum\"\n" +
            "GROUP BY t0.\"DocCur\", t0.\"DocNum\", t0.\"DocDate\", t0.\"CardName\", t0.\"U_Purchase_Comments\", t0.\"U_Status_Zam\", t0.\"BPLName\" , t2.\"SeriesName\", t0.\"DocEntry\"\n" +
            "\n" +
            "),\n" +
            "\n" +
            "\n" +
            "TEMP2 AS (\n" +
            "\n" +
            "SELECT DISTINCT \"DocEntry\", COUNT (*) AS \"Ilość pozycji\" FROM POR1 GROUP BY \"DocEntry\"\n" +
            "\n" +
            "),\n" +
            "\n" +
            "\n" +
            "TEMP3 AS (\n" +
            "\n" +
            "SELECT \"DocEntry\", count(\"DocEntry\") AS \"Zrealizowano\"\n" +
            "FROM POR1 WHERE \"LineStatus\" = 'C' GROUP BY \"DocEntry\"\n" +
            "\n" +
            ")\n" +
            "\n" +
            "SELECT * FROM\n" +
            " (SELECT DISTINCT \n" +
            "\n" +
            "t1.\"DocEntry\",\n" +
            "t1.\"Numer zamówienia\",\n" +
            "t1.\"Data zamówienia\",\n" +
            "t1.\"Dostawca\",\n" +
            "t1.\"Wartość zamówienia\",\n" +
            "t1.\"Waluta\",\n" +
            "t1.\"Uwagi\",\n" +
            "t1.\"Oddział\",\n" +
            "t1.\"Typ zamówienia\",\n" +
            "t1.\"Status\"\n" +
            "FROM TEMP t1 \n" +
            "--INNER JOIN TEMP2 t2 on t1.\"DocEntry\" = t2.\"DocEntry\"\n" +
            "LEFT OUTER JOIN TEMP3 t3 on t1.\"DocEntry\" = t3.\"DocEntry\") AS table\n" +
            "\n" +
             "WHERE (table.\"Typ zamówienia\" LIKE 'MAG-BB' OR table.\"Typ zamówienia\" LIKE 'MAG-NS') AND table.\"Numer zamówienia\" LIKE '" + OrderNumber + "%' AND table.\"Dostawca\" LIKE '%" + Supplier + "%' AND table.\"Status\" LIKE '%" + Status + "%' AND table.\"Waluta\" LIKE '%" + Currency + "%' AND IFNULL (\"Uwagi\", '1') LIKE '%" + Comments + "%' AND table.\"Oddział\" LIKE '%" + Branch + "%'  \n" +
            "\n");
            gridPanel.DataTable = dataTable;
            SAPbouiCOM.EditTextColumn column = (SAPbouiCOM.EditTextColumn)gridPanel.Columns.Item("DocEntry");
            column.LinkedObjectType = "22";
        }

        public void fillSecondGridWitchChemicalDetails(SAPbouiCOM.Grid gridPanel, String OrderNumber)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT\n" +
             "t0.\"ItemCode\" \"Indeks\",\n" +
              "t0.\"Dscription\" \"Opis\",\n" +
              "t0.\"Quantity\" \"Ilość zamówiona\",\n" +
              "(t0.\"Quantity\" - t0.\"OpenQty\") AS \"Zrealizowano\",\n" +
              "t0.\"OpenQty\" \"Pozostało do zrealizowania\",\n" +
               "t0.\"Price\" \"Cena jedn.\",\n" +
                "t0.\"LineTotal\" \"Wartość\",\n" +
                "t0.\"Currency\" \"Waluta\"\n" +
                  "\n" +
                "FROM POR1 t0\n" +
             "INNER JOIN OPOR t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
              "WHERE t1.\"DocNum\" = " + OrderNumber + "");
            gridPanel.DataTable = dataTable;
        }


        public void chemicalStocks(SAPbouiCOM.Grid gridPanel, String Client, String ItemCode, String U_DrawNoFinal, String Description)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("WITH TEMP2 AS (\n" +
            "\t\t\t \t\t   \t\t\t\t\n" +
            "SELECT  \n" +
            " T6.\"ItemCode\", \n" +
            " SUM (T2.\"OnHandQty\") AS \"Linia\"\n" +
            "-- T3.\"WhsCode\"\n" +
            "FROM OITL T0\n" +
            " INNER JOIN ITL1 T1 ON T1.\"LogEntry\"=T0.\"LogEntry\"\n" +
            " INNER JOIN OBBQ T2 ON T1.\"MdAbsEntry\" = T2.\"SnBMDAbs\"\n" +
            " INNER JOIN OBIN T3 ON T2.\"BinAbs\" = T3.\"AbsEntry\"\n" +
            " INNER JOIN OBTN T4 ON T2.\"SnBMDAbs\"=t4.\"AbsEntry\"\n" +
            " INNER JOIN OINM T5 ON T0.\"DocEntry\" = T5.\"CreatedBy\" AND T0.\"DocLine\" = T5.\"DocLineNum\" AND T0.\"DocType\" = T5.\"TransType\"\n" +
            " INNER JOIN OITM T6 ON T0.\"ItemCode\" = T6.\"ItemCode\"\n" +
            "WHERE\n" +
            " T3.\"AltSortCod\" LIKE 'MAT_POM_LINIA'\n" +
            "GROUP BY\n" +
            " T6.\"ItemCode\"--, t3.\"WhsCode\"\n" +
            " \n" +
            "\t\t\t\t)\n" +
            "\t\t\t\t\t\t\t\t\t\n" +
            "SELECT DISTINCT\n" +
            "t1.\"CardName\" \"Klient\",\n" +
            "t0.\"ItemCode\" \"Indeks\",\n" +
            "t0.\"U_DrawNoFinal\" \"NrRysGot\",\n" +
            "t0.\"ItemName\" \"Opis\",\n" +
            "t0.\"InvntryUom\" \"jm\",\n" +
            "t0.\"U_MinQty\" \"Ilość min\",\n" +
            "t0.\"OnHand\" \"Stan sur\",\n" +
            "IFNULL (t5.\"Linia\",0) \"Stan lin\",\n" +
            "(CASE WHEN (t0.\"OnHand\" > t0.\"U_MinQty\" ) THEN 'OK'\n" +
            "      ELSE  'NOK' END ) AS \"Stan\"\n" +
            " --t0.\"U_GospodarkaMaterialowa\"\n" +
            "FROM OITM t0\n" +
            "INNER JOIN OCRD t1 ON SUBSTR (t0.\"ItemCode\",4,5) = RIGHT(t1.\"CardCode\",5)\n" +
            "LEFT OUTER JOIN  TEMP2 t5 ON t0.\"ItemCode\" = t5.\"ItemCode\"\n" +
            "WHERE t0.\"U_GospodarkaMaterialowa\" LIKE '1'\n AND IFNULL (t1.\"CardName\", '0') LIKE '%" + Client + "%' AND t0.\"ItemCode\" LIKE '%" + ItemCode + "%' AND IFNULL (t0.\"U_DrawNoFinal\", 'Brak') LIKE '%" + U_DrawNoFinal + "%' AND t0.\"ItemName\" LIKE '%" + Description + "%'" +
            "ORDER BY t1.\"CardName\"");
            gridPanel.DataTable = dataTable;
            SAPbouiCOM.EditTextColumn column = (SAPbouiCOM.EditTextColumn)gridPanel.Columns.Item("Indeks");
            column.LinkedObjectType = "4";
        }


        public void fillSecondGridWitchChemicalStocks(SAPbouiCOM.Grid gridPanel, String ItemCode)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("WITH TEMP0 AS (\n" +
            " \n" +
            " SELECT DISTINCT \n" +
            " t0.\"BaseDocNum\",\n" +
            " SUM (t0.\"Quantity\") AS \"Ilość\"\n" +
            " FROM PDN1 t0\n" +
            " GROUP BY t0.\"BaseDocNum\"\n" +
            " ),\n" +
            "\n" +
            "TEMP AS (\n" +
            "\n" +
            "SELECT DISTINCT \n" +
            "t0.\"DocNum\" \"Numer zamówienia\",\n" +
            "t0.\"DocDate\"  \"Data zamówienia\",\n" +
            "t0.\"DocDueDate\" \"Data dostawy\",\n" +
            "t0.\"CardName\" \"Dostawca\",\n" +
            "SUM (t1.\"OpenQty\" * t1.\"Price\") AS \"Wartość zamówienia\",\n" +
            "t0.\"DocCur\" \"Waluta\",\n" +
            "t0.\"U_Purchase_Comments\" \"Uwagi\",\n" +
            "t0.\"BPLName\" \"Oddział\",\n" +
            "t2.\"SeriesName\" \"Typ zamówienia\",\n" +
            "t0.\"DocEntry\",\n" +
            "t1.\"Quantity\" \"Zamówiono\",\n" +
            "t1.\"ItemCode\",\n" +
            "t4.\"Ilość\" \"Dostarczono\",\n" +
            "(CASE WHEN (SUM (t1.\"Quantity\") = SUM (t4.\"Ilość\")) THEN 'Zrealizowane'\n" +
            "\t  WHEN (SUM (t1.\"Quantity\") > SUM (t4.\"Ilość\") AND SUM (t4.\"Ilość\") <> 0) THEN 'Częściowo'\n" +
            "\t  WHEN (SUM (t4.\"Ilość\") IS NULL) THEN 'Niezrealizowane'\n" +
            "\t  WHEN (SUM (t1.\"Quantity\") < SUM (t4.\"Ilość\")) THEN 'Przekroczone'\n" +
            "END) AS \"Status\"\n" +
            "FROM OPOR t0\n" +
            "INNER JOIN POR1 t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
            "INNER JOIN NNM1 t2 ON t0.\"Series\" = t2.\"Series\"\n" +
            "LEFT OUTER JOIN TEMP0 t4 ON t0.\"DocNum\" = t4.\"BaseDocNum\"\n" +
            "WHERE (t2.\"SeriesName\" LIKE 'MAG-BB' OR t2.\"SeriesName\" LIKE 'MAG-NS')\n" +
            "GROUP BY t0.\"DocCur\", t0.\"DocNum\", t0.\"DocDate\", t0.\"CardName\", t0.\"U_Purchase_Comments\", t0.\"U_Status_Zam\", t0.\"BPLName\" , t2.\"SeriesName\", t0.\"DocEntry\", t1.\"Quantity\", t4.\"Ilość\", t0.\"DocDueDate\", t1.\"ItemCode\"\n" +
            "\n" +
            "),\n" +
            "\n" +
            "\n" +
            "TEMP2 AS (\n" +
            "\n" +
            "SELECT DISTINCT \"DocEntry\", COUNT (*) AS \"Ilość pozycji\" FROM POR1 GROUP BY \"DocEntry\"\n" +
            "\n" +
            "),\n" +
            "\n" +
            "\n" +
            "TEMP3 AS (\n" +
            "\n" +
            "SELECT \"DocEntry\", count(\"DocEntry\") AS \"Zrealizowano\"\n" +
            "FROM POR1 WHERE \"LineStatus\" = 'C' GROUP BY \"DocEntry\"\n" +
            "\n" +
            ")\n" +
            "\n" +
            "SELECT * FROM\n" +
            " (SELECT DISTINCT \n" +
            "\n" +
            "t1.\"Numer zamówienia\",\n" +
            "t1.\"Data zamówienia\",\n" +
            "(CASE WHEN (IFNULL (t1.\"Dostarczono\", 0) = 0) THEN 'Niezrealizowane'\n" +
            "      WHEN (t1.\"Zamówiono\" > IFNULL(t1.\"Dostarczono\", 0) AND IFNULL (t1.\"Dostarczono\", 0) <> 0) THEN 'Częściowa'\n" +
            "      ELSE 'nd'\n" +
            "END) AS \"ReaNazwa\",\n" +
            "t1.\"Zamówiono\",\n" +
            "t1.\"Dostarczono\",\n" +
            "(t1.\"Zamówiono\" - t1.\"Dostarczono\") AS \"Pozost.\",\n" +
            "t1.\"Data dostawy\",\n" +
            "t1.\"ItemCode\" \"Indeks\" \n" +
            "FROM TEMP t1 \n" +
            "LEFT OUTER JOIN TEMP3 t3 on t1.\"DocEntry\" = t3.\"DocEntry\") AS table\n" +
            "WHERE (table.\"ReaNazwa\" LIKE 'Częściowa' OR table.\"ReaNazwa\" LIKE 'Niezrealizowane') AND table.\"Indeks\" LIKE '" + ItemCode + "'");
            gridPanel.DataTable = dataTable;
        }


        //do skasowania później
        //public void sqlaTotalReport(SAPbouiCOM.Grid gridPanel, String CardCode, String Logo, String Description, String ItemCode, String U_DrawNoRaw, String U_DrawNoFinal)
        //{
        //    temporaryID = base.setRandom();
        //    form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
        //    dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
        //    temporaryID++;
        //    dataTable.ExecuteQuery("SELECT DISTINCT \n" +
        //    "t2.\"CardName\" \"Dostawca\",\n" +
        //    "RIGHT (t3.\"CardCode\", 5) \"Logo\",\n" +
        //    "SUM (t1.\"Quantity\") \"Ilość za wskazany okres\",\n" +
        //    "t1.\"Dscription\" \"Opis\",\n" +
        //    "t1.\"ItemCode\" \"Indeks\",\n" +
        //    "t4.\"U_DrawNoRaw\" \"Surowy\",\n" +
        //    "t4.\"U_DrawNoFinal\" \"Gotowy\"\n" +
        //    "--t1.\"DocDate\"\n" +
        //    "FROM POR1 T1\t\n" +
        //    "INNER JOIN OPOR t2 ON t1.\"DocEntry\" = t2.\"DocEntry\"\n" +
        //    "INNER JOIN OCRD t3 ON t2.\"CardCode\" = t3.\"CardCode\"\n" +
        //    "INNER JOIN OITM t4 ON t1.\"ItemCode\" = t4.\"ItemCode\"\n" +
        //    "WHERE t1.\"ItemCode\" NOT LIKE 'ZAM' AND t1.\"ItemCode\" NOT LIKE '#' AND t2.\"CardName\" LIKE '%" + CardCode + "%' AND RIGHT (t3.\"CardCode\", 5) LIKE '%" + Logo + "%' AND t1.\"Dscription\" LIKE '%" + Description + "%' AND t1.\"ItemCode\" LIKE '%" + ItemCode + "%' AND IFNULL (t4.\"U_DrawNoRaw\", '1') LIKE '%" + U_DrawNoRaw + "%' AND IFNULL (t4.\"U_DrawNoFinal\", '1') LIKE '%"+U_DrawNoFinal+"%' AND t2.\"DocDate\" BETWEEN '2017-01-01 00:00:00.0'  AND '2018-01-12 00:00:00.0'-- AND t1.\"Dscription\" LIKE 'FAPROXYD 620 CZARNA PŁM.RAL 9005'\n" +
        //    "GROUP BY \"Dscription\", t1.\"ItemCode\", t4.\"U_DrawNoFinal\", t4.\"U_DrawNoRaw\",t2.\"CardName\", t3.\"CardCode\"");
        //    gridPanel.DataTable = dataTable;
        //}


        public void sqaDeliveredTotal(SAPbouiCOM.Grid gridPanel, String CardName, String Logo, String DateFrom, String DateTo)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT \n" +
            "t2.\"CardName\" \"Dostawca\",\n" +
            "RIGHT (t3.\"CardCode\", 5) \"Logo\",\n" +
            "SUM (t1.\"Quantity\") \"Ilość dostarczona\"\n" +
            "FROM PDN1 T1\t\n" +
            "INNER JOIN OPDN t2 ON t1.\"DocEntry\" = t2.\"DocEntry\"\n" +
            "INNER JOIN OCRD t3 ON t2.\"CardCode\" = t3.\"CardCode\"\n" +
            "WHERE t2.\"CardName\" LIKE '%" + CardName + "%' AND t2.\"CardCode\" LIKE '%" + Logo + "%' AND CAST (LEFT(t2.\"DocDate\", 10) AS DATE)  BETWEEN '" + DateFrom + "' AND '" + DateTo + "' \n" +
            "GROUP BY t3.\"CardCode\", t2.\"CardName\"");
            gridPanel.DataTable = dataTable;
        }


        public void sqaSecondGrid(SAPbouiCOM.Grid gridPanel, String CardName, String DateFrom, String DateTo)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT DISTINCT\n" +
            "t0.\"DocNum\" \"Numer zamówienia\",\n" +
            "CAST (LEFT(t0.\"DocDate\", 10) AS DATE) \"Data\"\n" +
            "FROM OPDN t0\n" +
            "WHERE t0.\"CardName\" LIKE '" + CardName + "' AND CAST (LEFT(t0.\"DocDate\", 10) AS DATE) BETWEEN '" + DateFrom + "' AND '" + DateTo + "'");
            gridPanel.DataTable = dataTable;
        }


        public void orderStatusForCommoners(SAPbouiCOM.Grid gridPanel, String OrderNumber)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("WITH \n" +
            "\n" +
            "TEMP4 AS (\n" +
            "\n" +
            "SELECT\n" +
            " t0.\"WinUsrName\",\n" +
            " t1.\"U_NAME\",\n" +
            " t2.\"Name\",\n" +
            " t2.\"Code\"\n" +
            "FROM\n" +
            "USR5 T0 \n" +
            "INNER JOIN OUSR T1 ON T0.\"UserCode\" = t1.USER_CODE\n" +
            "INNER JOIN OUDP t2 ON t1.\"Department\" = t2.\"Code\"\n" +
            "INNER JOIN M_CONNECTIONS MC ON MC.logical_connection_id = CURRENT_CONNECTION AND T0.\"ProcessID\" = MC.CLIENT_PID \n" +
            "AND T0.\"SessionID\" = CURRENT_CONNECTION\n" +
            "\n" +
            "),\n" +
            "\n" +
            "TEMP AS (\n" +
            "\n" +
            "SELECT DISTINCT \n" +
            "t10.\"Name\",\n" +
            "t4.\"DocNum\" \"Numer zlecenia\",\n" +
            "t0.\"DocNum\" \"Numer zamówienia\",\n" +
            "t0.\"DocDate\"  \"Data zamówienia\",\n" +
            "t0.\"CardName\" \"Dostawca\",\n" +
            "SUM (t1.\"OpenQty\" * t1.\"Price\") AS \"Wartość zamówienia\",\n" +
            "t0.\"DocCur\" \"Waluta\",\n" +
            "t0.\"U_Purchase_Comments\" \"Uwagi\",\n" +
            "(CASE WHEN (t0.\"U_Status_Zam\" = '1') THEN 'Nowe zamówienie'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '2') THEN 'Dyr_Zakładu'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '3') THEN 'Dyr_Zak/Log'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '4') THEN 'Dyr_Finansowy'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '5') THEN 'Zarząd'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '6') THEN 'OK'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '7') THEN 'Zablokowane'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '8') THEN 'W toku'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '9') THEN 'Realizacja częściowa'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '10') THEN 'Zrealizowane'\n" +
            "\t  WHEN (t0.\"U_Status_Zam\" = '11') THEN 'Faktura' \n" +
            "      WHEN (t0.\"U_Status_Zam\" = '12') THEN 'Archiwum' \n" +
            "END) AS \"Status\",\n" +
            "\n" +
            "t0.\"BPLName\" \"Oddział\",\n" +
            "t2.\"SeriesName\" \"Typ zamówienia\",\n" +
            "t0.\"DocEntry\"\n" +
            "FROM OPOR t0\n" +
            "RIGHT OUTER JOIN POR1 t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
            "INNER JOIN NNM1 t2 ON t0.\"Series\" = t2.\"Series\"\n" +
            "RIGHT OUTER JOIN OPRQ t4 ON t1.\"BaseDocNum\" = t4.\"DocNum\"\n" +
            "INNER JOIN TEMP4 t10 ON t4.\"Department\" = t10.\"Code\"\n" +
            "GROUP BY t0.\"DocCur\", t0.\"DocNum\", t0.\"DocDate\", t0.\"CardName\", t0.\"U_Purchase_Comments\", t0.\"U_Status_Zam\", t0.\"BPLName\" , t2.\"SeriesName\", t0.\"DocEntry\", t4.\"DocNum\", t10.\"Name\"\n" +
            "\n" +
            "\n" +
            ")\n" +
            "\n" +
            "\n" +
            "SELECT * FROM\n" +
            " (SELECT DISTINCT \n" +
            "t1.\"Name\" \"Dział wystawiający\",\n" +
            "t1.\"Numer zlecenia\",\n" +
            "t1.\"Numer zamówienia\",\n" +
            "t1.\"Data zamówienia\",\n" +
            "t1.\"Dostawca\",\n" +
            "t1.\"Uwagi\",\n" +
            "IFNULL (CAST (t1.\"Status\" AS nvarchar(40)), 'Zakupy') AS \"Status\" ,\n" +
            "t1.\"Typ zamówienia\"\n" +
            "FROM TEMP t1 \n" +
            ") AS table\n" +
            "\n" +
            "WHERE (IFNULL (table.\"Typ zamówienia\", '1') LIKE '%%' OR IFNULL (table.\"Typ zamówienia\", '1') LIKE '%%')\n" +
            "AND IFNULL (table.\"Uwagi\", '1') LIKE '%%' AND IFNULL (table.\"Status\", '1') LIKE '%%' AND table.\"Numer zlecenia\" LIKE '%"+OrderNumber+"%'");
            
            gridPanel.DataTable = dataTable;
        }


        public void fillSecondGridWithOrderDetailsForCommoners(SAPbouiCOM.Grid gridPanel, String OrderNumber)
        {
            temporaryID = base.setRandom();
            form = (SAPbouiCOM.Form)SAPbouiCOM.Framework.Application.SBO_Application.Forms.ActiveForm;
            dataTable = form.DataSources.DataTables.Add(temporaryID.ToString());
            temporaryID++;
            dataTable.ExecuteQuery("SELECT \n" +
            "t1.\"DocNum\" \"Numer zlecenia\",\n" +
            "t0.\"FreeTxt\" \"Opis\",\n" +
            "t0.\"Quantity\" \"Ilość\",\n" +
            "t0.\"Price\" \"Cena jedn\",\n" +
            "(t0.\"Quantity\" * t0.\"Price\") AS \"Łącznie\"\n" +
            "FROM\n" +
            "PRQ1 t0\n" +
            "INNER JOIN OPRQ t1 ON t0.\"DocEntry\" = t1.\"DocEntry\"\n" +
            "WHERE t1.\"DocNum\" LIKE '"+OrderNumber+"'");
            gridPanel.DataTable = dataTable;
        }
    }
}

using ProjMiner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjMiner.Controller
{
    public class ExcelHelper
    {
        public void InsertToExcel(NebraskaCaseField Field,string ExcelPath)
        {

           var xlsConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;\"", ExcelPath);
           var xlsCon = new System.Data.OleDb.OleDbConnection(xlsConnectionString);
            xlsCon.Open();

            System.Data.DataTable xlsData = new System.Data.DataTable();

            System.Data.OleDb.OleDbCommand xlsCommand;


            for (int i = 0; i < Field.NebraskaPartyField.Count; i++)
            {
                NebraskaPartyField party=Field.NebraskaPartyField[i];
                string tmp = "Insert into [base$] ([Court],[CaseNumber],[FilingDate],[Status],[COA],[PartyType],[PartyName],[Address],[StateCityZip]) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
                string query = string.Format(tmp, Field.CourtType, Field.CaseNumber, Field.FilingDate, Field.Status, Field.SuitCOA.Replace("'", "''"), party.Type.Replace("'", "''"), party.Name.Replace("'", "''"), party.Address.Replace("'","''"), party.StateCityZip.Replace("'", "''"));
                xlsCommand = new System.Data.OleDb.OleDbCommand(query, xlsCon);
                xlsCommand.ExecuteNonQuery();


            }


            xlsCon.Close();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace analiza_wskaznikowa
{
    class Reader
    {

        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        Excel.Range oRng;

        public Reader(ref Excel.Application excelApp)
        {
            //Start Excel and get Application object.
            oXL = excelApp;
            //oXL.Visible = false;
          
        }

    
       
        public Sprawozdanie ReadRaport(string path)
        {
            string[,] dane = new string[30,300];
            Sprawozdanie raportSpolkiNotoria = new Sprawozdanie();
     
            oWB = (Excel._Workbook) (oXL.Workbooks.Open(path));

            //oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            oSheet = (Excel._Worksheet)oWB.Worksheets[6];
            oRng = oSheet.UsedRange;
    
          
            for (int row = 1; row < 231; row++)
            {
                Pozycja node = new Pozycja();
                node.name = (string)(oRng.Cells[row, 2] as Excel.Range).Value2;
                node.pos = row;
                for (int col = 17; col < 22; col++) // w 17 kolumnie mamy 2010 rok
                {
                    try
                    {
                        node.value[col - 17] = (int)(oRng.Cells[row, col] as Excel.Range).Value2;
                    }
                    catch { }
                }
                raportSpolkiNotoria.dane[row] = node;
            }

            oWB.Close();
            
            return raportSpolkiNotoria;
        }
    }
}

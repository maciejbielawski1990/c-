using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
namespace analiza_wskaznikowa
{
    class ExcelPrinter
    {
        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        Excel.Range oRng;
        List<Wskazniki> wskazniki;

        public ExcelPrinter(ref Excel.Application excelApp)
        {
            oXL = excelApp;
        }

        public void PrintWskazniki(List<Wskazniki> listaWskaznikow, string nazwaSpolki)
        {
            oWB = (Excel._Workbook)(oXL.Workbooks.Open("E:\\raporty\\wyniki.xlsx"));
        

            oSheet = (Excel._Worksheet)oWB.Sheets.Add();
            oSheet.Name = nazwaSpolki;
            //oRng = oSheet.UsedRange;

            oSheet.Activate();

         //   worksheet.get_Range(topLeftLetter, bottomRightLetter).EntireColumn.AutoFit();
           

            for (int j = 0; j < 5; j++)
            {
                int startRok = 2010;
                oSheet.Cells[1, j + 4] = (startRok + j).ToString();
            }
            
            for (int i = 0; i < listaWskaznikow.Count(); i++)
            {
                oSheet.Cells[i + 2, 1] = listaWskaznikow[i].category;
                oSheet.Cells[i + 2, 2] = listaWskaznikow[i].name;
                oSheet.Cells[i + 2, 3] = listaWskaznikow[i].wzor;

                for (int j = 0; j < 5; j++)
                {
                    oSheet.Cells[i + 2, j + 4] = listaWskaznikow[i].value[j];
                }
            }

            oWB.Save();
        }
    }
}

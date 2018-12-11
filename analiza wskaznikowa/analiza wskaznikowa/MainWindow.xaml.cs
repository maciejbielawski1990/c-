using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;

namespace analiza_wskaznikowa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] files;
    //    string directory = "E:\\raporty";
        List<Sprawozdanie> listaSprawozdanSpolek;
        Excel.Application excelApp;
        public MainWindow()
        {
            InitializeComponent();
            ReadFilesPath();
            StartExcel();
            Reader reader = new Reader(ref excelApp);
            listaSprawozdanSpolek = new List<Sprawozdanie>();

            for (int i = 0; i < files.Count() - 1; i++)
            {
                Sprawozdanie spr = reader.ReadRaport(files[i]);
                spr.nazwa_spolki = files[i].Split('.')[0];
                listaSprawozdanSpolek.Add(spr);

            }

            ExcelPrinter printer = new ExcelPrinter(ref excelApp);

            for (int i = 0; i < listaSprawozdanSpolek.Count(); i++)
            {
                KalkulatorWskaznikow kalk = new KalkulatorWskaznikow(listaSprawozdanSpolek[i]);
                kalk.GetAll();
                printer.PrintWskazniki(kalk.listaWskaznikow, listaSprawozdanSpolek[i].nazwa_spolki);
            }
            Closeexc();  
        }

        void Closeexc
            ()
        {
            excelApp.Quit();
        }
       

        void StartExcel()
        {
            excelApp = new Excel.Application();
            excelApp.Visible = false;
        }

        void ReadFilesPath()
        {
            OpenFileDialog dialg = new OpenFileDialog();
            dialg.Multiselect = true;
            //DirectoryInfo dir = new DirectoryInfo(directory);
            try
            {
                files = dialg.FileNames;
            }
            catch 

            {
                
            }
        }
    }
}

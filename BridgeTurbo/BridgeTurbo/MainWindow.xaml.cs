using System;
using System.Collections.Generic;
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
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.IO;
using MigraDoc.RtfRendering;
using MigraDoc.DocumentObjectModel.Shapes;
using Bridge;
using Bridge.Reading;

namespace BridgeTurbo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Settings.SettingStart();
            VugraphLin v = new VugraphLin("60570.lin");
            MainRoomLin m = new MainRoomLin("cerebro.lin");
            v.ReadLin();
            m.ReadLin();

             // Printer p = new Printer(v,m);

            int[,] lewy = new int[4, 5];
            int[] input = { 7, 7, 9, 6, 7, 7, 7, 9, 6, 7, 6, 6, 4, 6, 5, 6, 6, 4, 6, 5 };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    lewy[i, j] = input[i * 5 + j];
                }
            }

              BridgeInfo.zabawaMAX(lewy, vulnerabilties.none);


     BlassTrening bt = new BlassTrening(v, m);
           bt.Print();
        //    bbogame bg = new bbogame(m);
       //     bg.Print();

            RtfDocumentRenderer renderer = new RtfDocumentRenderer();
            renderer.Render(bt.document, "Tes2t.doc", null);

            Process.Start("Test.pdf");
        }
    }
}

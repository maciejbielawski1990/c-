using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service_Management_Projekt.Klasy
{
    public class Wspolrzedne
    {
        public double dlugosc;
        public double szerokosc;
        public Wspolrzedne()
        {
            szerokosc = 0;
            dlugosc = 0;
        }
        public Wspolrzedne(double d_, double s_)
        {
            dlugosc = d_;
            szerokosc = s_;
        }
        public double getDistance(Wspolrzedne w2)
        {
            double distance = 111.95 * 180 / Math.PI * Math.Acos((Math.Sin(szerokosc * Math.PI / 180.0) * Math.Sin(w2.szerokosc * Math.PI / 180.0)) + (Math.Cos(szerokosc * Math.PI / 180.0) * Math.Cos(w2.szerokosc * Math.PI / 180.0) * Math.Cos((w2.dlugosc - dlugosc) * Math.PI / 180.0)));
            return distance;
        }
        
    }

    public class Algorytm
    {
        public string[] miejsca_zlecen;
        public int[] czas_zlecen;
        public Wspolrzedne[] wspolrzedne_miejsca_zlecen;
        SA symulowaneWyzarzanie;
        
        /*
        public Algorytm()
        {
            miejsca_zlecen = new string[40];
            czas_zlecen = new int[40];
            wspolrzedne_miejsca_zlecen = new Wspolrzedne[40];
        }
        */
        public Algorytm()
        {
            miejsca_zlecen = new string[0];
            czas_zlecen = new int[0];
            wspolrzedne_miejsca_zlecen = new Wspolrzedne[0];
        }

        public Algorytm(int size)
        {
            miejsca_zlecen = new string[size];
            czas_zlecen = new int[size];
            wspolrzedne_miejsca_zlecen = new Wspolrzedne[size];
        }

        public void wypelnij_dane_z_bazy()
        {
            Algorytm dane;
            ObslugaBazy obs_baz = new ObslugaBazy();
            dane = obs_baz.wyszukaj_zlecenia();
            symulowaneWyzarzanie = obs_baz.wyszukaj_serwis_i_serwisantow();
            miejsca_zlecen = dane.miejsca_zlecen;
            czas_zlecen = dane.czas_zlecen;
            wspolrzedne_miejsca_zlecen = dane.wspolrzedne_miejsca_zlecen;

        }

        public double run(double Tmin, double deltaFrac) // deltaFrac greater or equal to 1
        {
            wypelnij_dane_z_bazy();
            symulowaneWyzarzanie.runSA(czas_zlecen, wspolrzedne_miejsca_zlecen, Tmin, deltaFrac);
            return symulowaneWyzarzanie.getBestTime();
        }

        public bool save(double cmax)
        {
            if (symulowaneWyzarzanie.getBestTime() < 0)
            {
                return false;
            }
            ObslugaBazy obs_baz = new ObslugaBazy();
            int[] route = symulowaneWyzarzanie.getBestRoute();
            obs_baz.zapiszHarmonogram(route, cmax);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analiza_wskaznikowa
{
    class KalkulatorWskaznikow
    {
        public List<Wskazniki> listaWskaznikow;
        Sprawozdanie sprawozdanie;

        public KalkulatorWskaznikow(Sprawozdanie sprawozdanie_)
        {
            listaWskaznikow = new List<Wskazniki>();
            sprawozdanie = sprawozdanie_;
        }
        public void GetAll()
        {
            listaWskaznikow.Add(GetWskaznikBiezacejPlynnosci());
            listaWskaznikow.Add(GetWskaznikDlugu());
            listaWskaznikow.Add(GetTrwaloscStrukturyFinansowania());
            listaWskaznikow.Add(GetWskaznikIIStopnia());
            listaWskaznikow.Add(GetWskaznikOgolnegoZadluzenia());
            listaWskaznikow.Add(GetWskaznikOgolnejSytuacjiFinansowej());
            listaWskaznikow.Add(GetWskaznikPoziomuKosztow());
            listaWskaznikow.Add(GetWskaznikRentownosciAktywow());
            listaWskaznikow.Add(GetWskaznikRentownosciAktywowObrotowych());
            listaWskaznikow.Add(GetWskaznikRentownosciAktywowTrwalych());
            listaWskaznikow.Add(GetWskaznikRentownosciKapitaluWlasnego());
            listaWskaznikow.Add(GetWskaznikRentownosciObrotowBrutto());
            listaWskaznikow.Add(GetWskaznikRentownosciObrotowNetto());
            listaWskaznikow.Add(GetWskaznikUdzialuZyskuNetto_w_PO());
            listaWskaznikow.Add(GetWskaznikWyplacalnosciGotowkowej());
            listaWskaznikow.Add(GetWskaznikZadluzeniaKW());
            listaWskaznikow.Add(GetWskaznikZadluzenieSrodkowTrwalych());
            listaWskaznikow.Add(GetWskaznikCykluNaleznosci());
            
        }

        #region wskazniki plynnosci

        Wskazniki GetWskaznikBiezacejPlynnosci()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Płynność";
            wskaznik.name = "Wskaźnik Bieżącej Płynności";
            wskaznik.wzor = "aktywa obrotowe / zobowiazania krótkoterminowe";
            
            // poz. 15 / 18
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[15].value[i] / sprawozdanie.dane[18].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikWyplacalnosciGotowkowej()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Płynność";
            wskaznik.name = "Wskaznik Wyplacalnosci Gotowkowej";
            wskaznik.wzor = "srodki pieniężne / zobowiazania krótkoterminowe";

            // poz. 50 / 18
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[50].value[i] / sprawozdanie.dane[18].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikIIStopnia()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Płynność";
            wskaznik.name = "Wskaznik płynności III stopnia";
            wskaznik.wzor = "(srodki pieniężne + należności) / zobowiazania krótkoterminowe";

            // poz. 50 + 47 / 18
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[50].value[i] + sprawozdanie.dane[47].value[i]) / sprawozdanie.dane[18].value[i];

            return wskaznik;
        }


        #endregion

        
        #region wskazniki rentownosci

        Wskazniki GetWskaznikRentownosciAktywow()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik rentowności aktywów";
            wskaznik.wzor = "zysk netto / aktywa ogółem";

            // poz. 116 / 13
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[116].value[i] / sprawozdanie.dane[13].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikRentownosciAktywowTrwalych()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik rentowności aktywów trwałych";
            wskaznik.wzor = "zysk netto / aktywa trwałe";

            // poz. 116 / 14
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[116].value[i] / sprawozdanie.dane[14].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikRentownosciAktywowObrotowych()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik rentowności aktywów obrotowych";
            wskaznik.wzor = "zysk netto / aktywa obrotowe";

            // poz. 116 / 15
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[116].value[i] / sprawozdanie.dane[15].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikRentownosciKapitaluWlasnego()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik rentowności kapitału własnego";
            wskaznik.wzor = "zysk netto / kapitał własny";

            // poz. 116 / 16
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[116].value[i] / sprawozdanie.dane[16].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikRentownosciObrotowNetto()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik rentowności obrotów netto";
            wskaznik.wzor = "zysk netto / przychody ogołem";

            // poz. 116 / (90+106)
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[116].value[i] / (sprawozdanie.dane[90].value[i] + sprawozdanie.dane[106].value[i]);

            return wskaznik;
        }

        Wskazniki GetWskaznikRentownosciObrotowBrutto()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik rentowności obrotów brutto";
            wskaznik.wzor = "zysk brutto / przychody ogołem";

            // poz. 111 / (90+106)
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[111].value[i] / (sprawozdanie.dane[90].value[i] + sprawozdanie.dane[106].value[i]);

            return wskaznik;
        }

        Wskazniki GetWskaznikPoziomuKosztow()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Rentownosc";
            wskaznik.name = "Wskaźnik poziomu kosztów";
            wskaznik.wzor = "Koszty własny sprzedaży / przychody netto ze sprzedazy";

            // poz. 94 / 90
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[94].value[i] / sprawozdanie.dane[90].value[i];

            return wskaznik;
        }

        #endregion

        #region wskazniki zadluzenia

        Wskazniki GetWskaznikOgolnegoZadluzenia()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Zadłużenie";
            wskaznik.name = "Wskaźnik ogólnego zadłużenia";
            wskaznik.wzor = "zobowiązania razem / Aktywa Razem";

            // poz. (67+76) / 30
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[67].value[i] + sprawozdanie.dane[76].value[i]) / sprawozdanie.dane[30].value[i];


            return wskaznik;
        }

        Wskazniki GetWskaznikZadluzeniaKW()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Zadłużenie";
            wskaznik.name = "Wskaźnik zadłużenia kapitału własnego";
            wskaznik.wzor = "zobowiązania razem / kapitał własny";

            // poz. (67+76) / 16
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[67].value[i] + sprawozdanie.dane[76].value[i]) / sprawozdanie.dane[16].value[i];


            return wskaznik;
        }

        Wskazniki GetWskaznikDlugu()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Zadłużenie";
            wskaznik.name = "Wskaźnik długu";
            wskaznik.wzor = "zobowiązania długoterminowe / kapitał własny";

            // poz. 67 / 16
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[67].value[i]) / sprawozdanie.dane[16].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikZadluzenieSrodkowTrwalych()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Zadłużenie";
            wskaznik.name = "Wskaźnik zadłużenia środków trwałych";
            wskaznik.wzor = "aktywa trwale / zobowiazania dlugoterminowe";

            // poz. 31 / 67
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[31].value[i]/ sprawozdanie.dane[67].value[i];

            return wskaznik;
        }

        Wskazniki GetTrwaloscStrukturyFinansowania()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Zadłużenie";
            wskaznik.name = "Wskaźnik trwałości struktury finansowania";
            wskaznik.wzor = "(zobowiązania długoterminowe + kapitał własny) / aktywa razem";

            // poz. (67 + 16) / 13
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[67].value[i] + sprawozdanie.dane[16].value[i]) / sprawozdanie.dane[13].value[i];

            return wskaznik;
        }

        Wskazniki GetWskaznikOgolnejSytuacjiFinansowej()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Zadłużenie";
            wskaznik.name = "Wskaźnik ogolnej sytuacji finansowej";
            wskaznik.wzor = "kapitał własny * aktywa obrotowe / zobowiązania razem * aktywa trwałe";

            // poz. (16/14) / ((17+18)/15)
            for (int i = 0; i < 5; i++)
            {
                double licznik = sprawozdanie.dane[16].value[i] / sprawozdanie.dane[14].value[i];
                double mianownik = (sprawozdanie.dane[17].value[i] + sprawozdanie.dane[18].value[i]) / sprawozdanie.dane[15].value[i];

                wskaznik.value[i] = licznik / mianownik;
            }
 
            return wskaznik;
        }


        #endregion

        #region wskazniki przeplywow pienieznych

        Wskazniki GetWskaznikUdzialuZyskuNetto_w_PO()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Przepływy pieniężne";
            wskaznik.name = "Wskaźnik udziału zysku netto w przepływach operacyjnych";
            wskaznik.wzor = "zysk netto / przeplywy operacyjne";

            // poz. 116 / 143
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = sprawozdanie.dane[116].value[i] / sprawozdanie.dane[143].value[i];

            return wskaznik;
        }


        #endregion

        #region wskazniki aktywnosci

        Wskazniki GetWskaznikCykluNaleznosci()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Aktywność";
            wskaznik.name = "Długość cyklu należności";
            wskaznik.wzor = "(Należności krótkoterminowe / Przychody ze sprzedaży ) * 365";

            // poz. 47 / 90
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[47].value[i] / sprawozdanie.dane[90].value[i]) * 365;

            return wskaznik;
        }

        Wskazniki GetWskaznikCykluZobowiazan()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Aktywność";
            wskaznik.name = "Długość cyklu zobowiązań";
            wskaznik.wzor = "(Zobowiązania krótkoterminowe / Przychody ze sprzedaży ) * 365";

            // poz. 18 / 90
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[18].value[i] / sprawozdanie.dane[90].value[i]) * 365;

            return wskaznik;
        }

        Wskazniki GetWskaznikCykluZapasów()
        {
            Wskazniki wskaznik = new Wskazniki();
            wskaznik.category = "Aktywność";
            wskaznik.name = "Długość cyklu zapasów";
            wskaznik.wzor = "(Zapasy / Koszt wytworzenia zapasów)";
            // poz. 45 / ??
            for (int i = 0; i < 5; i++)
                wskaznik.value[i] = (sprawozdanie.dane[47].value[i] / sprawozdanie.dane[90].value[i]) * 365;

            return wskaznik;
        }
        #endregion
    }
}

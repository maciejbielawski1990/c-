using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service_Management_Projekt.Klasy
{
    public class SA
    {
        private int _iloscSerwisow;
        private int _iloscSerwisantow;
        private int[] _current;
        private int[] _best;
        private int[] _endPositionID;
        private int[] _startPositionID;
        private double _T;
        private double _Tmax = 1000;
        private Wspolrzedne _startPos;
        private Wspolrzedne _endPos;
        private double _sredniaPredkosc = 90;
        private double _bestTime;
        private double _currentTime;
        public double getBestTime()
        {
            return _bestTime;
        }
        public SA(int iloscSerwisow, int iloscSerwisantow, string miastoFirmy = "Łódź")
        {
            ObslugaBazy ob = new ObslugaBazy();
            _startPos = ob.getWspolrzedne(miastoFirmy);
            _endPos = _startPos;
            _bestTime = -1;
            _T = _Tmax;
            _iloscSerwisantow = iloscSerwisantow;
            _iloscSerwisow = iloscSerwisow;
            _current = new int[_iloscSerwisow + _iloscSerwisantow];
            _best = new int[_iloscSerwisantow + _iloscSerwisow];
            _startPositionID = new int[_iloscSerwisantow];
            _endPositionID = new int[_iloscSerwisantow];
            
            int sredniaDlugosc = (_iloscSerwisow+_iloscSerwisantow)/_iloscSerwisantow;
            int iloscDoUstalenia = _iloscSerwisow;
            int iter = 0;
            int iloscBezrobotnychSerwisantow = _iloscSerwisantow;
            _startPositionID[0] = 0;
            while(iloscDoUstalenia>0)
            {
                int copyIloscDoUstalenia = iloscDoUstalenia;
                
                for(int i=0; i < copyIloscDoUstalenia/iloscBezrobotnychSerwisantow; i++)
                {
                    iloscDoUstalenia--;
                    _current[iter] = _iloscSerwisow - iloscDoUstalenia;
                    iter++;
                }
                _current[iter] = 0;
                if (iloscBezrobotnychSerwisantow > 0)
                {
                    _endPositionID[_iloscSerwisantow - iloscBezrobotnychSerwisantow] = iter;
                }
                iter++; 
                if (iloscBezrobotnychSerwisantow > 1)
                {
                    _startPositionID[1 + _iloscSerwisantow - iloscBezrobotnychSerwisantow] = iter;
                }
                iloscBezrobotnychSerwisantow--;
            }
            for (int it = 0; it < _current.Length; it++)
            {
                _best[it] = _current[it];
            }
            
            _endPositionID[_iloscSerwisantow - 1] = iter - 1;
           
        }
        public double countPermutationTime(int[] czas_zlecen, 
            Wspolrzedne[] wspolrzedne_miejsca_zlecen)
        {
            double cmax = 0;
            double c = 0;
            Wspolrzedne lastCity = _startPos;
            foreach(int x in _current)
            {
                if (x != 0)
                {
                    c += lastCity.getDistance(wspolrzedne_miejsca_zlecen[x - 1]) / _sredniaPredkosc;
                    lastCity = wspolrzedne_miejsca_zlecen[x - 1];
                    c += czas_zlecen[x - 1]; //trial
                }
                else //if (x == 0)
                {
                    c += lastCity.getDistance(_endPos) / _sredniaPredkosc;
                    if(c>cmax)
                    {
                        cmax = c;
                    }
                    c = 0;
                    lastCity = _startPos;
                }
            }
            return cmax;
        }
        public void mixAndCheckPermutation(int[] czas_zlecen,
            Wspolrzedne[] wspolrzedne_miejsca_zlecen)
        {
            int[] currenttmp = new int[_iloscSerwisow + _iloscSerwisantow];
            int i = 0;
            foreach(int x in _current)
            {
                currenttmp[i] = x;
                i++;
            }
            //two kinds of permutations :) equally distributed?
            Random r = new Random();
            double choice = r.NextDouble();
            if(choice > 0.5 || _iloscSerwisantow == 1) // we will mix two paths (or permutate one path)
            {
                //Console.Write("PIERWSZA METODA!");
                int firstPath = r.Next(_iloscSerwisantow);
                while (_startPositionID[firstPath] == _endPositionID[firstPath])
                {
                    firstPath = r.Next(_iloscSerwisantow);
                }
                int firstTaskID = r.Next(_endPositionID[firstPath] - _startPositionID[firstPath]) + _startPositionID[firstPath];
                int secondPath = r.Next(_iloscSerwisantow);
                while (_startPositionID[secondPath] == _endPositionID[secondPath])
                {
                    secondPath = r.Next(_iloscSerwisantow);
                } 
                int secondTaskID = r.Next(_endPositionID[secondPath] - _startPositionID[secondPath]) + _startPositionID[secondPath];
                int tmp = _current[firstTaskID];
                _current[firstTaskID] = _current[secondTaskID];
                _current[secondTaskID] = tmp;
            }
            else if(_iloscSerwisantow > 1) // we will move one task from one agent to another one
            {
                //Console.Write("DRUGA METODA!");
                int firstPath = r.Next(_iloscSerwisantow);
                int firstTaskID = r.Next(_endPositionID[firstPath] - _startPositionID[firstPath]) + _startPositionID[firstPath];
                int firstTask = _current[firstTaskID];
                int secondPath = r.Next(_iloscSerwisantow);
                while(secondPath == firstPath)
                {
                    secondPath = r.Next(_iloscSerwisantow);
                }
                int newPlace = r.Next(_endPositionID[secondPath] - _startPositionID[secondPath]) + _startPositionID[secondPath];
               
                int oldTask = -1;
                
                for (int iter = 0; iter < _current.Length; iter++ )
                {
                    if (firstTaskID < newPlace)
                    {
                        if (iter >= firstTaskID && iter < newPlace)
                        {
                            _current[iter] = _current[iter + 1];
                        }
                        if (iter == newPlace && _current[newPlace] == 0)
                        {
                            _current[newPlace - 1] = firstTask;
                        }
                        else if ( iter == newPlace)
                        {
                            _current[newPlace] = firstTask;
                        }
                    }
                    else
                    {
                        if (iter == newPlace)
                        {
                            oldTask = _current[newPlace];
                            _current[newPlace] = firstTask;
                        }
                        if (iter > newPlace && iter <= firstTaskID)
                        {
                            int tmp = _current[iter];
                            _current[iter] = oldTask;
                            oldTask = tmp;
                        }
                    }
                }
               
                
                _startPositionID[0] = 0;
                int it = 0;
                int it2 = 0;
                foreach (int x in _current)
                {
                    
                    if (x == 0)
                    {
                        _endPositionID[it] = it2;
                        it++;
                        if (it < _iloscSerwisantow)
                        {
                            _startPositionID[it] = it2 + 1;
                        }
                    }
                    it2++;
                }
            }
            double time = countPermutationTime(czas_zlecen, wspolrzedne_miejsca_zlecen);
            if (time < _currentTime)
            {
                _currentTime = time;
                if (time < _bestTime)
                {
                    for (int it = 0; it < _current.Length; it++)
                    {
                        _best[it] = _current[it];
                    }
                    _bestTime = time;
                    
                    Console.WriteLine("NEW bestTIME = " +_bestTime +", _T = " +_T);
                }
            }
            else
            {
                double prob = r.NextDouble();
                if (prob >= Math.Exp(-(time - _bestTime) / _T)) // nie akceptujemy zmiany
                {
                    _current = currenttmp;
                    _startPositionID[0] = 0;
                    int it = 0;
                    int it2 = 0;
                    foreach (int x in _current)
                    {

                        if (x == 0)
                        {
                            _endPositionID[it] = it2;
                            it++;
                            if (it < _iloscSerwisantow)
                            {
                                _startPositionID[it] = it2 + 1;
                            }
                        }
                        it2++;
                    }
                }
                else
                {
                    _currentTime = time;
                }
            }
        }
        public void runSA(int[] czas, Wspolrzedne[] wspolrzedne, double Tmin, double deltaT)
        {
            _currentTime = _bestTime = countPermutationTime(czas, wspolrzedne);

           

            Console.WriteLine("=====");
            while (_T > Tmin)
            {
                mixAndCheckPermutation(czas, wspolrzedne);
                _T /= deltaT;
              
            }
            foreach (int x in _best)
            {
                Console.Write(x);
            }
            Console.Write(", besttime = ");
            Console.WriteLine(_bestTime);
            
        }
        public int[] getBestRoute()
        {
            return _best;
        }
    }
}

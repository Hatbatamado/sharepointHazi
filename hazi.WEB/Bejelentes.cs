using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB
{
    public class Bejelentes
    {
        //az oldal újratöltések / validátorok miatt példányosítással elveszne a bool értéke
        //ezért statikus változóként így megmarad
        static bool ujBejelentes;
        static DateTime kezdeti;
        static DateTime vege;

        public static DateTime Vege
        {
            get { return Bejelentes.vege; }
            set { Bejelentes.vege = value; }
        }

        public static DateTime Kezdeti
        {
            get { return Bejelentes.kezdeti; }
            set { Bejelentes.kezdeti = value; }
        }

        public static bool UjBejelentes
        {
            get { return Bejelentes.ujBejelentes; }
            set { Bejelentes.ujBejelentes = value; }
        }
    }
}
using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    /// <summary>
    /// Áttekintő elem: dátum, jóváhagyási státusz, jogcim, jogcím név és jogcím szín adatokkal
    /// </summary>
    public class AttekintoElem
    {
        public DateTime Datum { get; set; }
        public string JovaStatusz { get; set; }
        public Jogcim Jogcim { get; set; }
        public char JogcimNev { get; set; }
        public string Szin { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class UjJogcim
    {
        public int ID { get; set; }
        public string Cim { get; set; }
        public Nullable<bool> Inaktiv { get; set; }
        public string RogzitveSzin { get; set; }
        public string JovahagySzin { get; set; }
        public string ElutasitvaSzin { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class HaviAttekintoElem
    {
        public string UserName { get; set; }
        public DateTime Datum { get; set; }
        public string JogcimNev { get; set; }
        public string JovahagyasiStatusz { get; set; }
        public string Szin { get; set; }

        [NotMapped]
        public List<HaviAttekintoElem> UsersLista { get; set; }
    }
}
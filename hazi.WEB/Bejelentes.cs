using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB
{
    public class Bejelentes
    {
        DateTime kezdeti, vege;

        public DateTime Vege
        {
            get { return vege; }
            set { vege = value; }
        }
        public DateTime Kezdeti
        {
            get { return kezdeti; }
            set { kezdeti = value; }
        }

        public Bejelentes (DateTime kezdeti, DateTime vege)
        {
            this.kezdeti = kezdeti;
            this.vege = vege;
        }
    }
}
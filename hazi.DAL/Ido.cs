using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hazi.DAL
{
    public class Ido
    {
        TimeSpan ts;

        public TimeSpan Ts
        {
            get { return ts; }
        }

        public Ido(DateTime start, DateTime end)
        {
            ts = new TimeSpan(end.Hour - start.Hour, end.Minute - start.Minute, 0);
        }

        public override string ToString()
        {
            return ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;
        }
    }
}

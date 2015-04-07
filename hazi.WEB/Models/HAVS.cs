using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Models
{
    public class HAVS
    {
        private static List<HaviAttekintoViewModel> hvRep;

        public static List<HaviAttekintoViewModel> HvRep
        {
            get { return HAVS.hvRep; }
            set { HAVS.hvRep = value; }
        }

        public HAVS(List<HaviAttekintoElem> hv, DateTime date)
        {
            hvRep = new List<HaviAttekintoViewModel>();
            foreach (var item in hv)
            {
                if (item.UsersLista.Count != 0)
                    hvRep.Add(new HaviAttekintoViewModel(date.Year, date.Month, item.UserName)
                    {
                        Nev = item.UserName,
                        RangVezeto = '+'
                    });
                else
                    hvRep.Add(new HaviAttekintoViewModel(date.Year, date.Month, item.UserName)
                    {
                        Nev = item.UserName,
                        RangNormal = 'o'
                    });
            }
        }
    }
}
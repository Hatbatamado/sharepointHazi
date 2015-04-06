using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class HaviAttekintoViewModel
    {
        public char RangVezeto { get; set; }
        public char RangNormal { get; set; }
        public string Nev { get; set; }
        public List<HaviAttekintoElem> BelsoLista { get; set; }

        public HaviAttekintoViewModel(int year, int month, string user)
        {
            BelsoLista = new List<HaviAttekintoElem>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                DateTime temp = new DateTime(year, month, i);
                HaviAttekintoElem elem = JovahagyBLL.GetJovahagyByHaviAttekinto(temp, user);
                BelsoLista.Add(elem);
            } 
        }
    }
}
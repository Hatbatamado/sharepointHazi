using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class AttekintoViewModel
    {
        public List<AttekintoElem> BelsoLista { get; set; }
        public string HonapNeve { get; set; }

        /// <summary>
        /// Lista feltöltése áttekintő elemekkel év, hónap és felhasználó által
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="user"></param>
        public AttekintoViewModel(int year, int month, string user)
        {
            BelsoLista = new List<AttekintoElem>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++ )
            {
                DateTime temp = new DateTime(year, month, i);
                AttekintoElem elem = JovahagyBLL.GetJovahagyByEvByUser(temp, user);
                BelsoLista.Add(elem);     
            }                
        }
    }
}
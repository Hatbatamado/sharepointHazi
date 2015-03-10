using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hazi.DAL;

namespace hazi.BLL
{
    public class JogcimBLL
    {
        public static List<Jogcim> GetById()
        {
            List<Jogcim> jogcimek = new List<Jogcim>();
            using(hazi2Entities db = new hazi2Entities())
            {
                var jog = (from j in db.Jogcims
                           orderby j.Cim
                           select j);

                foreach (var item in jog)
                {
                    jogcimek.Add(item);
                }
                return jogcimek;
            }
        }
    }
}

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
        /// <summary>
        /// Jógcímek kiolvása db-ből
        /// </summary>
        /// <param name="all">True-ra minden jogcímet visszaad, False-ra csak az aktívakat</param>
        /// <returns></returns>
        public static List<Jogcim> GetJogcimek(bool all)
        {
            List<Jogcim> jogcimek = new List<Jogcim>();
            using(hazi2Entities db = new hazi2Entities())
            {
                if (all)
                {
                    jogcimek = (from j in db.Jogcims
                                select j).ToList();
                }
                else
                {
                    jogcimek = (from j in db.Jogcims
                                where j.Inaktiv == false
                                select j).ToList();
                }

                foreach (var item in jogcimek)
                {
                    if (item.Inaktiv == null)
                        item.Inaktiv = false;
                }
                return jogcimek;
            }
        }

        /// <summary>
        /// Az adott ID-jű jogcímet adja vissza
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetJogcimById(int id)
        {
            using(hazi2Entities db = new hazi2Entities())
            {
                try
                {
                    var jogcim = (from j in db.Jogcims
                                  where j.ID == id
                                  select j).Single();
                    return jogcim.Cim;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Admin megváltoztatja egy jogcím nevét / aktív státuszát
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nev"></param>
        /// <param name="inaktiv"></param>
        public static void JogcimMentes(int? id, string nev, bool inaktiv, string szin)
        {
            if (id.HasValue && id > 0)
            {
                using(hazi2Entities db = new hazi2Entities())
                {
                    Jogcim jogcim = (from j in db.Jogcims
                                     where j.ID == id
                                     select j).Single();
                    if (nev != string.Empty)
                        jogcim.Cim = nev;

                    jogcim.Inaktiv = inaktiv;

                    jogcim.Szin = szin;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// jogcím kiolvása db-ből név alapján
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Jogcim GetJogcimByName(string name)
        {
            if (name != string.Empty)
            {
                using(hazi2Entities db = new hazi2Entities())
                {
                    return (from j in db.Jogcims
                                where j.Cim == name
                                select j).Single();
                }
            }
            return null;
        }
    } 
}

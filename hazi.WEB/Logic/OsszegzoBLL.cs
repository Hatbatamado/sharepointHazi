using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class OsszegzoBLL
    {
        /// <summary>
        /// Az adott hónapú bejelentések lekérdezése és időtartamok összegzése jogcím és jóváhagyási státusz alapján
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetOsszegzes(string name, DateTime time)
        {
            List<UjBejelentes> jogcimList;
            using (hazi2Entities db = new hazi2Entities())
            {
                jogcimList = (from b in db.Jogcims
                          select new UjBejelentes
                          {
                              JogcimNev = b.Cim
                          }).ToList();
            }


            List<UjBejelentes> listaByName;
            using (hazi2Entities db = new hazi2Entities())
            {
                listaByName = (from b in db.IdoBejelentes1
                               where b.UserName == name &&
                               b.KezdetiDatum.Year == time.Year && b.KezdetiDatum.Month == time.Month
                               select new UjBejelentes
                               {
                                   Statusz = b.Statusz,
                                   KezdetiDatum = b.KezdetiDatum,
                                   VegeDatum = b.VegeDatum,
                                   JogcimNev = b.Jogcim.Cim
                               }).ToList();
            }

            foreach (var elem in listaByName)
            {
                elem.Ido = new Ido(elem.KezdetiDatum, elem.VegeDatum);

                string[] seged = elem.Statusz.Split('&');
                if (seged.Length > 1)
                {
                    elem.JovaStatus = seged[1];
                }

                for (int i = 0; i < jogcimList.Count; i++)
                {
                    Osszeadas(elem, jogcimList, i);
                }
            }

            return jogcimList;
        }

        /// <summary>
        /// adott jogcím jóváhagyási státusz szerinti összegzése
        /// </summary>
        /// <param name="listaByName"></param>
        /// <param name="jogcimLista"></param>
        /// <param name="index"></param>
        private static void Osszeadas(UjBejelentes listaByName, List<UjBejelentes> jogcimLista, int index)
        {
            if (jogcimLista[index].JogcimNev == listaByName.JogcimNev)
            {
                if (listaByName.JovaStatus == "Rogzitve")
                    jogcimLista[index].OsszRogzitet += listaByName.Ido.Ts.TotalHours;
                else if (listaByName.JovaStatus == "Jovahagyva")
                    jogcimLista[index].OsszJovahagyott += listaByName.Ido.Ts.TotalHours;
                else if (listaByName.JovaStatus == "Elutasitva")
                    jogcimLista[index].OsszElutasitott += listaByName.Ido.Ts.TotalHours;
            }
        }
    }
}
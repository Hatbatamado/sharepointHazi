using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class OsszegzoBLL
    {
        public static List<DateYearMonth> GetAllOsszegzes()
        {
            List<UjBejelentes> lista;
            using(hazi2Entities db = new hazi2Entities())
            {
                lista = (from b in db.IdoBejelentes1
                             select new UjBejelentes
                             {
                                 KezdetiDatum = b.KezdetiDatum
                             }).ToList();
            }

            List<DateYearMonth> dym = new List<DateYearMonth>();
            if (lista.Count > 0)
            {    
                dym.Add(new DateYearMonth(lista[0].KezdetiDatum.Year, lista[0].KezdetiDatum.Month));

                DateYearMonth dymElem;
                foreach (UjBejelentes item in lista)
                {
                    dymElem = new DateYearMonth(item.KezdetiDatum.Year, item.KezdetiDatum.Month);
                    int j = 0;
                    while (j < dym.Count)
                    {
                        if (dym[j].Year == dymElem.Year && dym[j].Month == dymElem.Month)
                            break;
                        j++;
                    }

                    if (j == dym.Count)
                        dym.Add(dymElem);
                }
            }

            return dym;
        }

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

        private static void Osszeadas(UjBejelentes listaByName, List<UjBejelentes> jogcimLista, int index)
        {
            if (jogcimLista[index].JogcimNev == listaByName.JogcimNev)
            {
                if (listaByName.JovaStatus == "Rogzitve")
                    jogcimLista[index].OsszRogzitet += listaByName.Ido.Ts.Hours + listaByName.Ido.Ts.Minutes / 60;
                else if (listaByName.JovaStatus == "Jovahagyva")
                    jogcimLista[index].OsszJovahagyott += listaByName.Ido.Ts.Hours + listaByName.Ido.Ts.Minutes / 60;
                else if (listaByName.JovaStatus == "Elutasitva")
                    jogcimLista[index].OsszElutasitott += listaByName.Ido.Ts.Hours + listaByName.Ido.Ts.Minutes / 60;
            }
        }
    }
}
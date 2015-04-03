using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class UjJogcimBLL
    {
        /// <summary>
        /// Jogcímeket ad vissza színekkel együtt
        /// </summary>
        /// <returns></returns>
        public static List<UjJogcim> GetAllJogcim()
        {
            List<UjJogcim> jogcimek = new List<UjJogcim>();
            using(hazi2Entities db = new hazi2Entities())
            {
                jogcimek = (from j in db.Jogcims
                            select new UjJogcim
                            {
                                ID = j.ID,
                                Cim = j.Cim,
                                Inaktiv = j.Inaktiv,
                                RogzitveSzin = j.Szin
                            }).ToList();

                foreach (var item in jogcimek)
                {
                    if (item.RogzitveSzin != null)
                    {
                        string[] seged = item.RogzitveSzin.Split('#');
                        if (seged.Length > 1)
                        {
                            item.RogzitveSzin = "#" + seged[1];
                            if (seged.Length > 2)
                                item.JovahagySzin = "#" + seged[2];
                        }
                    }
                }
            }
            return jogcimek;
        }

        /// <summary>
        /// Jel magyarázat listát ad vissza, jel magyarázat nevekkel és színekkel ellátva
        /// </summary>
        /// <returns></returns>
        internal static List<JelMagy> GetJelMagy()
        {
            List<JelMagy> lista = new List<JelMagy>();
            List<Jogcim> jogcimek;
            using (hazi2Entities db = new hazi2Entities())
            {
                jogcimek = (from j in db.Jogcims
                                select j).ToList();
            }
            foreach (var item in jogcimek)
            {
                if (item.Szin != null)
                {
                    string[] seged = item.Szin.Split('#');
                    string rogszin = "";
                    string jovaszin = "";
                    if (seged.Length > 1)
                        rogszin = '#' + seged[1];
                    if (seged.Length > 2)
                        jovaszin = '#' + seged[2];

                    if (rogszin.Length <= 1 || jovaszin.Length <= 1)
                    {
                        if (rogszin.Length <= 1)
                            rogszin = Konstansok.RogzitveAlapSzin;
                        if (jovaszin.Length <= 1)
                            jovaszin = Konstansok.JovahagyvaAlapSzin;

                        item.Szin = rogszin + jovaszin;

                        using (hazi2Entities db = new hazi2Entities())
                        {
                            var query = (from j in db.Jogcims
                                         where item.ID == j.ID
                                         select j).Single();
                            query.Szin = item.Szin;

                            db.SaveChanges();             
                        }
                    }
                    lista.Add(new JelMagy() {
                        BetuJel = item.Cim[0], JelNev = item.Cim + " " +
                        JovaHagyasStatus.Rogzitve.ToDisplayString().ToLower(), Szin = rogszin });
                    lista.Add(new JelMagy() {
                        BetuJel = item.Cim[0], JelNev = item.Cim + " " +
                            JovaHagyasStatus.Jovahagyva.ToDisplayString().ToLower(), Szin = jovaszin });
                }
            }
            lista.Add(new JelMagy() {
                BetuJel = 'T', JelNev = "Több bejelentés 1 nap", Szin = Konstansok.TobbBejelenesAlapszin });

            return lista;
        }
    }
}
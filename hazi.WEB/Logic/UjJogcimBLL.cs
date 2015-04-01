﻿using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class UjJogcimBLL
    {
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
                            rogszin = "#FFFF00";
                        if (jovaszin.Length <= 1)
                            jovaszin = "#006400";

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
                BetuJel = 'T', JelNev = "Több bejelentés 1 nap", Szin = "#c9c9ee" });

            return lista;
        }
    }
}
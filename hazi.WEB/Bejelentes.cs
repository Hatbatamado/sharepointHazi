using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB
{
    public class Bejelentes
    {
        //az oldal újratöltések / validátorok miatt példányosítással elveszne a bool értéke
        //ezért statikus változóként így megmarad
        static bool ujBejelentes;
        static DateTime kezdeti;
        static DateTime vege;

        public static DateTime Vege
        {
            get { return Bejelentes.vege; }
            set { Bejelentes.vege = value; }
        }

        public static DateTime Kezdeti
        {
            get { return Bejelentes.kezdeti; }
            set { Bejelentes.kezdeti = value; }
        }

        public static bool UjBejelentes
        {
            get { return Bejelentes.ujBejelentes; }
            set { Bejelentes.ujBejelentes = value; }
        }

        public static List<UjBejelentes> GetIdoBejelentesek(string role, string name, DateTime start, DateTime end)
        {
            string admin = RegisterUserAs.Admin.ToString();
            string normal = RegisterUserAs.NormalUser.ToString();

            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                //Admin összes bejelentés
                if (role == admin)
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        return (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim
                                }).ToList();
                    }
                }
                else if (role == normal)
                {
                    //NormalUser összes bejelentés
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        return (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                where b.UserName == name
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim
                                }).ToList();
                    }
                }
            }
            else
            {
                //Admin megadott időszakon belüli jelentések
                if (role == admin)
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        return (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                where b.KezdetiDatum >= start && b.VegeDatum <= end
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim
                                }).ToList();
                    }
                }
                else if (role == normal)
                {
                    //NormalUser megadott időszakon belüli jelentések
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        return (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                where b.UserName == name && b.KezdetiDatum >= start && b.VegeDatum <= end
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim
                                }).ToList();
                    }
                }
            }
            return null;
        }
    }
}
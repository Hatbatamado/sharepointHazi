using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    public class UjBejelentesBLL
    {
        /// <summary>
        /// Listás / Naptári nézethez időbejelentések kiolvasása admin / más felhasználó szerint
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetIdoBejelentesek(string role, string name, DateTime start, DateTime end)
        {
            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                //Admin összes bejelentés LISTÁS nézetben
                //minden törlési státusszal rendelkezőt megjelenítűnk
                if (role == Konstansok.admin)
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        List<UjBejelentes> bejelentesek = (from b in db.IdoBejelentes1
                                                           select new UjBejelentes
                                                           {
                                                               ID = b.ID,
                                                               KezdetiDatum = b.KezdetiDatum,
                                                               VegeDatum = b.VegeDatum,
                                                               JogcimID = b.JogcimID,
                                                               UserName = b.UserName,
                                                               LastEdit = b.UtolsoModosito,
                                                               LastEditTime = b.UtolsoModositas.HasValue ?
                                                                        b.UtolsoModositas.Value : DateTime.MinValue,
                                                               JogcimNev = b.Jogcim.Cim,
                                                               Statusz = b.Statusz
                                                           }).ToList();

                        DDLFeltoltese(bejelentesek, true);

                        return bejelentesek;
                    }
                }
                else if (role == Konstansok.normal || role == Konstansok.jovahagy)
                {
                    //NormalUser összes bejelentés LISTÁS nézetben
                    //csak azokat listázzuk ki, aminek a státuszát még a felhasználó nem állította át
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string segedStatus = TorlesStatus.Inaktiv.ToString();
                        List<UjBejelentes> bejelentesek = (from b in db.IdoBejelentes1
                                                           where b.UserName == name && (b.Statusz.Contains(segedStatus) ||
                                                           b.Statusz == null)
                                                           select new UjBejelentes
                                                           {
                                                               ID = b.ID,
                                                               KezdetiDatum = b.KezdetiDatum,
                                                               VegeDatum = b.VegeDatum,
                                                               JogcimID = b.JogcimID,
                                                               UserName = b.UserName,
                                                               LastEdit = b.UtolsoModosito,
                                                               LastEditTime = b.UtolsoModositas.HasValue ?
                                                                        b.UtolsoModositas.Value : DateTime.MinValue,
                                                               JogcimNev = b.Jogcim.Cim,
                                                               Statusz = b.Statusz
                                                           }).ToList();

                        foreach (UjBejelentes item in bejelentesek)
                        {
                            item.TorlesStatuszList = new List<ListItem>();
                            item.TorlesStatuszList.Add(
                                new ListItem { Value = TorlesStatus.Inaktiv.ToString(), Text = TorlesStatus.Inaktiv.ToDisplayString() });

                            //db-ben statusz nélküli elemek kapnak inaktív státuszt
                            if (item.TorlesStatus == null)
                                item.TorlesStatus = TorlesStatus.Inaktiv.ToString();
                        }

                        JovahagyBLL.StatuszBeallitasok(bejelentesek, false);

                        return bejelentesek;
                    }
                }
            }
            else
            {
                //Admin megadott időszakon belüli jelentések NAPTÁRI nézetben
                //minden törlési státusszal rendelkezőt megjelenítűnk
                //így az admin naptári nézett alapján is meg tudja nézni,
                //hogy hol találhatóak a felhasználó által törlésre kért bejelentések
                if (role == Konstansok.admin)
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        return (from b in db.IdoBejelentes1
                                where b.KezdetiDatum >= start && b.VegeDatum <= end
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.UtolsoModosito,
                                    JogcimNev = b.Jogcim.Cim,
                                    Statusz = b.Statusz
                                }).ToList();
                    }
                }
                else if (role == Konstansok.normal || role == Konstansok.jovahagy)
                {
                    //NormalUser megadott időszakon belüli jelentések NAPTÁRI nézetben
                    //csak azokat listázzuk ki, aminek a státuszát még a felhasználó nem állította át
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string segedStatus = TorlesStatus.Inaktiv.ToString();
                        return (from b in db.IdoBejelentes1
                                where (b.UserName == name && b.KezdetiDatum >= start
                                && b.VegeDatum <= end) && (b.Statusz.Contains(segedStatus) || b.Statusz == null)
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.UtolsoModosito,
                                    JogcimNev = b.Jogcim.Cim,
                                    Statusz = b.Statusz,
                                }).ToList();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// LISTÁS nézet szűrővel jogcimre, csak adminnak
        /// </summary>
        /// <param name="jogcim"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetIdoBejelentesByFilerJogcim(string jogcim)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                var bejelentesek = (from b in db.IdoBejelentes1
                                    where b.Jogcim.Cim.Contains(jogcim)
                                    select new UjBejelentes
                                    {
                                        ID = b.ID,
                                        KezdetiDatum = b.KezdetiDatum,
                                        VegeDatum = b.VegeDatum,
                                        JogcimID = b.JogcimID,
                                        UserName = b.UserName,
                                        LastEdit = b.UtolsoModosito,
                                        JogcimNev = b.Jogcim.Cim,
                                        Statusz = b.Statusz,
                                        LastEditTime = b.UtolsoModositas.HasValue ?
                                                         b.UtolsoModositas.Value : DateTime.MinValue,
                                    }).ToList();

                DDLFeltoltese(bejelentesek, true);

                return bejelentesek;
            }
        }

        /// <summary>
        /// LISTÁS nézet szűrővel státuszra, csak adminnak
        /// </summary>
        /// <param name="torles"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetIdoBejelentesByFilerStatus(string torles)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                var bejelentesek = (from b in db.IdoBejelentes1
                                    where b.Statusz.Contains(torles)
                                    select new UjBejelentes
                                    {
                                        ID = b.ID,
                                        KezdetiDatum = b.KezdetiDatum,
                                        VegeDatum = b.VegeDatum,
                                        JogcimID = b.JogcimID,
                                        UserName = b.UserName,
                                        LastEdit = b.UtolsoModosito,
                                        JogcimNev = b.Jogcim.Cim,
                                        Statusz = b.Statusz,
                                        LastEditTime = b.UtolsoModositas.HasValue ?
                                                        b.UtolsoModositas.Value : DateTime.MinValue,
                                    }).ToList();

                DDLFeltoltese(bejelentesek, true);

                return bejelentesek;
            }
        }

        /// <summary>
        /// LISTÁS nézet szűrővel felhasználóra, csak adminnak
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetIdoBejelentesByFilerFelhasznalo(string username)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                var bejelentesek = (from b in db.IdoBejelentes1
                                    where b.UserName.Contains(username)
                                    select new UjBejelentes
                                    {
                                        ID = b.ID,
                                        KezdetiDatum = b.KezdetiDatum,
                                        VegeDatum = b.VegeDatum,
                                        JogcimID = b.JogcimID,
                                        UserName = b.UserName,
                                        LastEdit = b.UtolsoModosito,
                                        JogcimNev = b.Jogcim.Cim,
                                        Statusz = b.Statusz,
                                        LastEditTime = b.UtolsoModositas.HasValue ?
                                                            b.UtolsoModositas.Value : DateTime.MinValue,
                                    }).ToList();

                DDLFeltoltese(bejelentesek, true);

                return bejelentesek;
            }
        }

        /// <summary>
        /// LISTÁS nézet szűrővel lasteditre, csak adminnak
        /// </summary>
        /// <param name="lastedit"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetIdoBejelentesByFilerLastEdit(string lastedit)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                var bejelentesek = (from b in db.IdoBejelentes1
                                    where b.UtolsoModosito.Contains(lastedit)
                                    select new UjBejelentes
                                    {
                                        ID = b.ID,
                                        KezdetiDatum = b.KezdetiDatum,
                                        VegeDatum = b.VegeDatum,
                                        JogcimID = b.JogcimID,
                                        UserName = b.UserName,
                                        LastEdit = b.UtolsoModosito,
                                        JogcimNev = b.Jogcim.Cim,
                                        Statusz = b.Statusz,
                                        LastEditTime = b.UtolsoModositas.HasValue ?
                                                            b.UtolsoModositas.Value : DateTime.MinValue,
                                    }).ToList();

                DDLFeltoltese(bejelentesek, true);

                return bejelentesek;
            }
        }

        /// <summary>
        /// DDL értékek beállítása és Jóváhagyási státusz beállítása
        /// </summary>
        /// <param name="bejelentesek"></param>
        /// <param name="AdminListasNezet"></param>
        private static void DDLFeltoltese(List<UjBejelentes> bejelentesek, bool AdminListasNezet)
        {
            JovahagyBLL.StatuszBeallitasok(bejelentesek, AdminListasNezet);

            //mivel a normál user és az admin ugyanazt az oldalt használja
            //ezért, hogy ne szálljon el a program a DDL rész miatt, normal usernek is be kell állítani
            foreach (UjBejelentes item in bejelentesek)
            {
                //DDL lista elemek
                item.TorlesStatuszList = ListItems();                

                //db-ben statusz nélküli elemek kapnak inaktív státuszt
                if (item.TorlesStatus == null)
                    item.TorlesStatus = TorlesStatus.Inaktiv.ToString();

                /*string[] seged = item.TorlesStatus.Split('&');
                item.TorlesStatus = ((TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0])).ToDisplayString();*/
            }
        }

        /// <summary>
        /// Törlési státuszok beállítása listitem listában
        /// </summary>
        /// <returns></returns>
        public static List<ListItem> ListItems()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem { Value = TorlesStatus.Inaktiv.ToString(), Text = TorlesStatus.Inaktiv.ToDisplayString() });
            items.Add(new ListItem { Value = TorlesStatus.BejelentettKerelem.ToString(), Text = TorlesStatus.BejelentettKerelem.ToDisplayString() });
            items.Add(new ListItem { Value = TorlesStatus.ElfogadottKerelem.ToString(), Text = TorlesStatus.ElfogadottKerelem.ToDisplayString() });
            items.Add(new ListItem { Value = TorlesStatus.Elutasitott.ToString(), Text = TorlesStatus.Elutasitott.ToDisplayString() });
            items.Add(new ListItem { Value = TorlesStatus.Torles.ToString(), Text = TorlesStatus.Torles.ToDisplayString() });

            return items;
        }

        /// <summary>
        /// Időbejelentés kiolvása db-ből, adminnak mindet, más felhasználókat csak azt, amit nem kért törlésre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mind"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static UjBejelentes GetIdoBejelentesById(int? id, bool mind, string name)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    if (mind)
                    {
                        try
                        {
                            return (from b in db.IdoBejelentes1
                                    where b.ID == id
                                    select new UjBejelentes
                                    {
                                        ID = b.ID,
                                        KezdetiDatum = b.KezdetiDatum,
                                        VegeDatum = b.VegeDatum,
                                        JogcimID = b.JogcimID,
                                        UserName = b.UserName,
                                        LastEdit = b.UtolsoModosito,
                                        LastEditTime = (DateTime)b.UtolsoModositas,
                                        JogcimStatusz = (bool)b.Jogcim.Inaktiv
                                    }).Single();
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        try
                        {
                            return (from b in db.IdoBejelentes1
                                    where b.ID == id &&
                                    !b.Statusz.Contains("RogzitettKerelem") && b.UserName == name
                                    select new UjBejelentes
                                    {
                                        ID = b.ID,
                                        KezdetiDatum = b.KezdetiDatum,
                                        VegeDatum = b.VegeDatum,
                                        JogcimID = b.JogcimID,
                                        UserName = b.UserName,
                                        LastEdit = b.UtolsoModosito,
                                        LastEditTime = (DateTime)b.UtolsoModositas,
                                        JogcimStatusz = (bool)b.Jogcim.Inaktiv
                                    }).Single();
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }
    }
}
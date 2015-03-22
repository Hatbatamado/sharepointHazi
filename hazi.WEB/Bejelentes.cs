using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Globalization;

namespace hazi.WEB
{
    public class Bejelentes
    {
        static string admin = RegisterUserAs.Admin.ToString();
        static string normal = RegisterUserAs.NormalUser.ToString();
        static string jovahagy = RegisterUserAs.Jovahagyok.ToString();

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
            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                //Admin összes bejelentés LISTÁS nézetben
                //minden törlési státusszal rendelkezőt megjelenítűnk
                if (role == admin)
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
                else if (role == normal || role == jovahagy)
                {
                    //NormalUser összes bejelentés LISTÁS nézetben
                    //csak azokat listázzuk ki, aminek a státuszát még a felhasználó nem állította át
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string segedStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
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
                                new ListItem { Value = TorlesStatus.NincsTorlesiKerelem.ToString(), Text = "Nincs törlési kérelem" });

                            //db-ben statusz nélküli elemek kapnak nincs törlési kérelem státuszt
                            if (item.TorlesStatus == null)
                                item.TorlesStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
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
                if (role == admin)
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
                                    JogcimNev = b.Jogcim.Cim
                                }).ToList();
                    }
                }
                else if (role == normal || role == jovahagy)
                {
                    //NormalUser megadott időszakon belüli jelentések NAPTÁRI nézetben
                    //csak azokat listázzuk ki, aminek a státuszát még a felhasználó nem állította át
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string segedStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
                        var bejelentesek = (from b in db.IdoBejelentes1
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

                        int d = 0;// Statuszok(bejelentesek);

                        return bejelentesek;
                    }
                }
            }
            return null;
        }

        //LISTÁS nézet szűrővel jogcimre, csak adminnak
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

        //LISTÁS nézet szűrővel törlés státuszra, csak adminnak
        public static List<UjBejelentes> GetIdoBejelentesByFilerTorlesStatus(string torles)
        {
            using (hazi2Entities db = new hazi2Entities())
            { //minden törlés és státusz nélküli
                if (torles == TorlesStatus.NincsTorlesiKerelem.ToString())
                {
                    var bejelentesek = (from b in db.IdoBejelentes1
                                        where b.Statusz.Contains(torles) || b.Statusz == null
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
                else
                { //csak a megadott státuszú jelentések
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
        }

        //LISTÁS nézet szűrővel felhasználóra, csak adminnak
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

        //LISTÁS nézet szűrővel lasteditre, csak adminnak
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

        private static void DDLFeltoltese(List<UjBejelentes> bejelentesek, bool AdminListasNezet)
        {
            JovahagyBLL.StatuszBeallitasok(bejelentesek, AdminListasNezet);

            //mivel a normál user és az admin ugyanazt az oldalt használja
            //ezért, hogy ne szálljon el a program a DDL rész miatt, normal usernek is be kell állítani
            foreach (UjBejelentes item in bejelentesek)
            {
                //DDL lista elemek
                item.TorlesStatuszList = new List<ListItem>();
                item.TorlesStatuszList.Add(
                    new ListItem { Value = TorlesStatus.NincsTorlesiKerelem.ToString(), Text = "Nincs törlési kérelem" });
                item.TorlesStatuszList.Add(
                        new ListItem { Value = TorlesStatus.ElfogadottKerelem.ToString(), Text = "Elfogadott kérelem" });
                item.TorlesStatuszList.Add(
                        new ListItem { Value = TorlesStatus.RegisztraltKerelem.ToString(), Text = "Regisztrált kérelem" });
                item.TorlesStatuszList.Add(
                        new ListItem { Value = TorlesStatus.Torles.ToString(), Text = "Törlés" });
                item.TorlesStatuszList.Add(
                        new ListItem { Value = TorlesStatus.Elutasitott.ToString(), Text = "Elutasított" });

                //db-ben statusz nélküli elemek kapnak nincs törlési kérelem státuszt
                if (item.TorlesStatus == null)
                    item.TorlesStatus = TorlesStatus.NincsTorlesiKerelem.ToString();

                string[] seged = item.TorlesStatus.Split('&');
                item.TorlesStatus = seged[0];
            }
        }
    }
}
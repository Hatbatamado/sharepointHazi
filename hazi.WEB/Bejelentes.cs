using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB
{
    public class Bejelentes
    {
        static string admin = RegisterUserAs.Admin.ToString();
        static string normal = RegisterUserAs.NormalUser.ToString();

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
                //Admin összes bejelentés listás nézetben
                //minden törlési státusszal rendelkezőt megjelenítűnk
                if (role == admin)
                {
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string torlesStatus = TorlesStatus.ElfogadottKerelem.ToString();
                        List<UjBejelentes> bejelentesek = (from b in db.IdoBejelentes1
                                                           join p in db.Jogcims on b.JogcimID equals p.ID
                                                           select new UjBejelentes
                                                           {
                                                               ID = b.ID,
                                                               KezdetiDatum = b.KezdetiDatum,
                                                               VegeDatum = b.VegeDatum,
                                                               JogcimID = b.JogcimID,
                                                               UserName = b.UserName,
                                                               LastEdit = b.LastEdit,
                                                               JogcimNev = p.Cim,
                                                               TorlesStatus = b.TorlesStatus
                                                           }).ToList();

                        foreach (UjBejelentes item in bejelentesek)
                        {
                            //DDL lista elemek
                            item.StatusList = new List<ListItem>();
                            item.StatusList.Add(
                                new ListItem { Value = TorlesStatus.ElfogadottKerelem.ToString(), Text = TorlesStatus.ElfogadottKerelem.ToString() });
                            item.StatusList.Add(
                                new ListItem { Value = TorlesStatus.RegisztraltKerelem.ToString(), Text = TorlesStatus.RegisztraltKerelem.ToString() });
                            item.StatusList.Add(
                                new ListItem { Value = TorlesStatus.NincsTorlesiKerelem.ToString(), Text = TorlesStatus.NincsTorlesiKerelem.ToString() });
                            item.StatusList.Add(
                                new ListItem { Value = TorlesStatus.Torles.ToString(), Text = TorlesStatus.Torles.ToString() });
                            item.StatusList.Add(
                                new ListItem { Value = TorlesStatus.Elutasitott.ToString(), Text = TorlesStatus.Elutasitott.ToString() });

                            //db-ben statusz nélküli elemek kapnak nincs törlési kérelem státuszt
                            if (item.TorlesStatus == null)
                                item.TorlesStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
                        }

                        return bejelentesek;
                    }
                }
                else if (role == normal)
                {
                    //NormalUser összes bejelentés listás nézetben
                    //csak azokat listázzuk ki, aminek a státuszát még a felhasználó nem állította át
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string segedStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
                        List<UjBejelentes> bejelentesek = (from b in db.IdoBejelentes1
                                                           join p in db.Jogcims on b.JogcimID equals p.ID
                                                           where b.UserName == name && (b.TorlesStatus == segedStatus ||
                                                           b.TorlesStatus == null)
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
                        
                        foreach (UjBejelentes item in bejelentesek)
                        {
                            item.StatusList = new List<ListItem>();
                            item.StatusList.Add(
                                new ListItem { Value = TorlesStatus.NincsTorlesiKerelem.ToString(), Text = TorlesStatus.NincsTorlesiKerelem.ToString() });

                            //db-ben statusz nélküli elemek kapnak nincs törlési kérelem státuszt
                            if (item.TorlesStatus == null)
                                item.TorlesStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
                        }

                        return bejelentesek;
                    }
                }
            }
            else
            {
                //Admin megadott időszakon belüli jelentések naptári nézetben
                //minden törlési státusszal rendelkezőt megjelenítűnk
                //így az admin naptári nézett alapján is meg tudja nézni,
                //hogy hol találhatóak a felhasználó által törlésre kért bejelentések
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
                    //NormalUser megadott időszakon belüli jelentések naptári nézetben
                    //csak azokat listázzuk ki, aminek a státuszát még a felhasználó nem állította át
                    using (hazi2Entities db = new hazi2Entities())
                    {
                        string segedStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
                        var bejelentesek = (from b in db.IdoBejelentes1
                                join p in db.Jogcims on b.JogcimID equals p.ID
                                where (b.UserName == name && b.KezdetiDatum >= start
                                && b.VegeDatum <= end) && (b.TorlesStatus == segedStatus || b.TorlesStatus == null)
                                select new UjBejelentes
                                {
                                    ID = b.ID,
                                    KezdetiDatum = b.KezdetiDatum,
                                    VegeDatum = b.VegeDatum,
                                    JogcimID = b.JogcimID,
                                    UserName = b.UserName,
                                    LastEdit = b.LastEdit,
                                    JogcimNev = p.Cim,
                                    TorlesStatus = b.TorlesStatus
                                }).ToList();

                        return bejelentesek;
                    }
                }
            }
            return null;
        }

        //listás nézet szűrővel
        public static List<UjBejelentes> GetIdoBejelentesByFilerJogcim(string jogcim, string role, string username)
        {
            if (role == admin)
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
                                            LastEdit = b.LastEdit,
                                            JogcimNev = b.Jogcim.Cim
                                        }).ToList();

                    DDLFeltoltese(bejelentesek, role);

                    return bejelentesek;
                }
            }
            else if (role == normal)
            {

            }
            return null;
        }

        private static void DDLFeltoltese(List<UjBejelentes> bejelentesek, string role)
        {
            //mivel a normál user és az admin ugyanazt az oldalt használja
            //ezért, hogy ne szálljon el a program a DDL rész miatt, normal usernek is be kell állítani
            foreach (UjBejelentes item in bejelentesek)
            {
                //DDL lista elemek
                item.StatusList = new List<ListItem>();
                item.StatusList.Add(
                    new ListItem { Value = TorlesStatus.NincsTorlesiKerelem.ToString(), Text = TorlesStatus.NincsTorlesiKerelem.ToString() });
                
                if (role == admin)
                {
                    item.StatusList.Add(
                        new ListItem { Value = TorlesStatus.ElfogadottKerelem.ToString(), Text = TorlesStatus.ElfogadottKerelem.ToString() });
                    item.StatusList.Add(
                        new ListItem { Value = TorlesStatus.RegisztraltKerelem.ToString(), Text = TorlesStatus.RegisztraltKerelem.ToString() });

                    item.StatusList.Add(
                        new ListItem { Value = TorlesStatus.Torles.ToString(), Text = TorlesStatus.Torles.ToString() });
                    item.StatusList.Add(
                        new ListItem { Value = TorlesStatus.Elutasitott.ToString(), Text = TorlesStatus.Elutasitott.ToString() });
                }

                //db-ben statusz nélküli elemek kapnak nincs törlési kérelem státuszt
                if (item.TorlesStatus == null)
                    item.TorlesStatus = TorlesStatus.NincsTorlesiKerelem.ToString();
            }
        }
    }
}
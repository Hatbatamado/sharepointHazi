using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    public class JovahagyBLL
    {
        /// <summary>
        /// Minden bejelentés megjelenítése, amit nem kértek törlésre
        /// </summary>
        /// <param name="statusz"></param>
        /// <returns></returns>
        public static List<UjBejelentes> GetJovahagyAll(string statusz)
        {
            if (statusz == "Mind")
                statusz = string.Empty;

            List<UjBejelentes> lista;
            using (hazi2Entities db = new hazi2Entities())
            {

                lista = (from b in db.IdoBejelentes1
                         where !b.Statusz.Contains("RogzitettKerelem") && b.Statusz.Contains(statusz)
                         select new UjBejelentes
                         {
                             ID = b.ID,
                             UserName = b.UserName,
                             KezdetiDatum = b.KezdetiDatum,
                             VegeDatum = b.VegeDatum,
                             JogcimNev = b.Jogcim.Cim,
                             Statusz = b.Statusz,
                             LastEdit = b.UtolsoModosito,
                             LastEditTime = b.UtolsoModositas.HasValue ? b.UtolsoModositas.Value : DateTime.MinValue,
                             JogcimID = b.JogcimID,
                         }).ToList();

            }

            foreach (UjBejelentes item in lista)
            {
                item.JovaStatuszList = new List<ListItem>();
                item.JovaStatuszList.Add(
                    new ListItem { Value = JovaHagyasStatus.Rogzitve.ToString(), Text = JovaHagyasStatus.Rogzitve.ToDisplayString() });
                item.JovaStatuszList.Add(
                    new ListItem { Value = JovaHagyasStatus.Jovahagyva.ToString(), Text = JovaHagyasStatus.Jovahagyva.ToDisplayString() });
                item.JovaStatuszList.Add(
                    new ListItem { Value = JovaHagyasStatus.Elutasitva.ToString(), Text = JovaHagyasStatus.Elutasitva.ToDisplayString() });

                item.JovaStatus = StatuszDarabolas(item.Statusz, 1);
                item.TorlesStatus = StatuszDarabolas(item.Statusz, 0);

                item.Ido = new Ido(item.KezdetiDatum, item.VegeDatum);
            }

            UsersOsszIdohoz(lista);

            StatuszBeallitasok(lista, false);

            return lista;
        }

        /// <summary>
        /// statusz mező értékének 2 részre osztása
        /// </summary>
        /// <param name="statusz"></param>
        /// <param name="melyik"></param>
        /// <returns></returns>
        private static string StatuszDarabolas(string statusz, int melyik)
        {
            string[] seged;
            if (statusz != null)
            {
                seged = statusz.Split('&');
                if (melyik == 0)
                    return seged[0];
                if (seged.Length != 1 && melyik == 1)
                    return seged[1];
            }
            return string.Empty;
        }

        /// <summary>
        /// státuszok beállítása
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="AdminListasNezet"></param>
        public static void StatuszBeallitasok(List<UjBejelentes> lista, bool AdminListasNezet)
        {
            int i = 0;
            while (i < lista.Count && lista.Count != 0)
            {
                StatuszVizsgalat(lista[i], lista, ref i, AdminListasNezet);
                i++;
            }
        }

        /// <summary>
        /// Megfelelő jóváhagyási és törlési státuszok beállítása
        /// </summary>
        /// <param name="item"></param>
        /// <param name="lista"></param>
        /// <param name="i"></param>
        /// <param name="AdminListasNezet"></param>
        private static void StatuszVizsgalat(UjBejelentes item, List<UjBejelentes> lista, ref int i, bool AdminListasNezet)
        {
            string[] seged;
            if (item.Statusz != null)
                seged = item.Statusz.Split('&');
            else
                seged = new string[] { TorlesStatus.Inaktiv.ToString() };

            //ha még nincs 2 fajta státusza
            if (seged.Length == 1)
            {
                if (seged[0] == TorlesStatus.Inaktiv.ToString())
                {
                    if (item.JogcimNev == "Rendes szabadság")
                    {
                        JovahagyStatuszBeallit(item, (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]), JovaHagyasStatus.Rogzitve);                      
                    }
                    else if (item.JogcimNev == "Jelenlét")
                    {
                        if (item.OsszIdo < 40)
                        {
                            if (item.OsszIdo != 0)
                            {
                                JovahagyStatuszBeallit(item, (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]), JovaHagyasStatus.Jovahagyva);
                            }
                            else
                            {
                                foreach (UjBejelentes ujb in lista)
                                {
                                    if (item.UserName == ujb.UserName)
                                        ujb.Ido = new Ido(ujb.KezdetiDatum, ujb.VegeDatum);
                                }
                                OsszIdo(lista, item.UserName);
                                if (item.OsszIdo < 40)
                                JovahagyStatuszBeallit(item, (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]), JovaHagyasStatus.Jovahagyva);
                                else
                                JovahagyStatuszBeallit(item, (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]), JovaHagyasStatus.Rogzitve);
                            }
                        }
                        else
                        {
                            JovahagyStatuszBeallit(item, (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]), JovaHagyasStatus.Rogzitve);
                        }
                    }
                    else
                    {
                        JovahagyStatuszBeallit(item, (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]), JovaHagyasStatus.Jovahagyva);
                    }
                }
                else
                {
                    if (!AdminListasNezet)
                    {
                        lista.Remove(item);
                        if (lista.Count != 0 && i != lista.Count)
                            i--;
                    }
                    else
                    {
                        item.TorlesStatus = ((TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0])).ToString();
                    }
                }
            }
            else //ha van 2 fajta státusz
            {
                if (seged[0] == TorlesStatus.Inaktiv.ToString())
                {
                    JovaHagyasStatus jova = (JovaHagyasStatus)Enum.Parse(typeof(JovaHagyasStatus), seged[1]);
                    item.JovaStatus = jova.ToString();
                    item.JovaStatusMegjelenes = jova.ToDisplayString();
                }
                TorlesStatus torles = (TorlesStatus)Enum.Parse(typeof(TorlesStatus), seged[0]);
                item.TorlesStatus = torles.ToString();
            }
        }

        /// <summary>
        /// DB-be az új státusz elmentése a paraméterekkel átadott értékek szerint
        /// </summary>
        /// <param name="item"></param>
        /// <param name="torlesStatusz"></param>
        /// <param name="jovaStatusz"></param>
        private static void JovahagyStatuszBeallit(UjBejelentes item, TorlesStatus torlesStatusz, JovaHagyasStatus jovaStatusz)
        {
            item.Statusz = torlesStatusz.ToString() + "&" + jovaStatusz.ToString();
            UjStatusz(item.ID, item.Statusz);
            item.TorlesStatus = torlesStatusz.ToString();
            item.JovaStatus = jovaStatusz.ToString();
            item.JovaStatusMegjelenes = jovaStatusz.ToDisplayString();
        }

        /// <summary>
        /// DB-be az új státusz elmentése
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ujStatusz"></param>
        private static void UjStatusz(int? id, string ujStatusz)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {

                    IdoBejelentes ib = (from b in db.IdoBejelentes1
                                        where b.ID == id
                                        select b).Single();

                    ib.Statusz = ujStatusz;

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Jelenlét jogcímhez a heti idő kiszámítása
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="username"></param>
        private static void OsszIdo(List<UjBejelentes> lista, string username)
        {
            List<int> index;
            lista = HanyadikHet(lista); //hét értékek feltöltése és ez alapján lista rendezése

            int i = 0;
            while (i < lista.Count)
            {
                if (lista[i].UserName == username) //keresendő felhasználó
                {
                    int keresendo = lista[i].HanyadikHet; //rendezett listában keresendő hét
                    int j = i;
                    double osszeg = 0;
                    index = new List<int>();
                    while (j < lista.Count && lista[j].HanyadikHet == keresendo)
                    {
                        if (lista[j].UserName == username)
                        {
                            if (lista[j].JogcimNev == "Jelenlét")
                            {
                                //összeadjuk a jelenlétnél, keresett felhasználónál,
                                //keresett héten bejelentett időtartamokat
                                //és ezek indexét lementjük, hogy mindnak az össz idő mezőjéhez
                                //be tudjuk írni, így nagy tábla méretnél nem kell
                                //külön összeadni 1-1 felhasználóhoz
                                osszeg += lista[j].Ido.Ts.TotalHours;
                                index.Add(j);
                            }
                        }
                        j++;
                    }
                    foreach (int item in index)
                    {
                        lista[item].OsszIdo = osszeg;
                    }
                    i = j - 1;
                }
                i++;
            }
        }

        /// <summary>
        /// Minden felhasználóra meghívjuk a jelenlét heti értékének beállító fv-t
        /// </summary>
        /// <param name="lista"></param>
        public static void UsersOsszIdohoz(List<UjBejelentes> lista)
        {
            List<Users> users = UsersBLL.UserList();
            foreach (Users user in users) //felhasználók
            {
                OsszIdo(lista, user.Name);
            }
        }

        /// <summary>
        /// Bejelentési kezdeti dátum hetének kiszámítása, +- 52/53 hét évszámtól függően
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private static List<UjBejelentes> HanyadikHet(List<UjBejelentes> lista)
        {
            foreach (UjBejelentes item in lista)
            {
                int id = item.ID;
                if (item.HanyadikHet == 0)
                {
                    item.HanyadikHet = GetIso8601WeekOfYear(item.KezdetiDatum);
                    if (item.KezdetiDatum.Year != DateTime.Now.Year)
                    {
                        int kulonbseg = item.KezdetiDatum.Year - DateTime.Now.Year;
                        int pluszhet = 0;
                        if (kulonbseg >= 1)
                        {
                            for (int i = 0; i < kulonbseg; i++)
                            {
                                pluszhet += GetIso8601WeekOfYear(new DateTime(DateTime.Now.Year + i, 12, 31));
                            }
                            item.HanyadikHet += pluszhet;
                        }
                        else if (kulonbseg <= -1)
                        {
                            for (int i = kulonbseg; i < 0; i++)
                            {
                                pluszhet += GetIso8601WeekOfYear(new DateTime(DateTime.Now.Year + i, 12, 31));
                            }
                            item.HanyadikHet -= pluszhet;
                        }
                    }
                }
            }
            lista.Sort((x, y) => x.HanyadikHet.CompareTo(y.HanyadikHet));

            return lista;
        }

        /// <summary>
        /// hányadik hét kiszámítása: http://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DateTime timeSeged = time;
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
                if (time.Year != timeSeged.Year)
                {
                    time = new DateTime(timeSeged.Year, timeSeged.Month, timeSeged.Day - 4);
                }
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// DB-be új jóváhagyási státusz mentése
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusz"></param>
        public static void JovahagyasMentes(int? id, string statusz)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    var query = (from b in db.IdoBejelentes1
                                 where b.ID == id
                                 select b).Single();

                    string[] seged = query.Statusz.Split('&');
                    query.Statusz = seged[0] + '&' + statusz;

                    db.SaveChanges();
                }
            }
        }


        internal static AttekintoElem GetJovahagyByEvByUser(DateTime date, string user)
        {
            AttekintoElem elem = null;
            using (hazi2Entities db = new hazi2Entities())
            {
                try
                {
                    elem = (from b in db.IdoBejelentes1
                            where b.KezdetiDatum.Year == date.Year &&
                            b.KezdetiDatum.Month == date.Month &&
                            b.KezdetiDatum.Day == date.Day && b.UserName == user &&
                            !b.Statusz.Contains("Elutasitva")
                            select new AttekintoElem
                            {
                                Datum = b.KezdetiDatum,
                                JovaStatusz = b.Statusz,
                                Jogcim = b.Jogcim
                            }).SingleOrDefault();
                }
                catch (InvalidOperationException)
                {
                    //több elem 1 nap
                    elem = new AttekintoElem {
                        Datum = date,
                        JogcimNev = 'T',
                        Szin = Konstansok.TobbBejelenesAlapszin };

                    return elem;
                }
                if (elem != null)
                {
                    elem.JogcimNev = elem.Jogcim.Cim.ToUpper()[0];

                    string[] seged = elem.JovaStatusz.Split('&');
                    if (seged.Length < 2)
                        return null;
                    else
                        elem.JovaStatusz = seged[1];

                    string[] segedszin = elem.Jogcim.Szin.Split('#');
                    string rogszin = "";
                    string jovaszin = "";
                    if (segedszin.Length >= 1)
                        rogszin = '#' + segedszin[1];
                    if (segedszin.Length >= 2)
                        jovaszin = '#' + segedszin[2];

                    if (elem.JovaStatusz == JovaHagyasStatus.Rogzitve.ToString())
                        elem.Szin = rogszin;
                    else if (elem.JovaStatusz == JovaHagyasStatus.Jovahagyva.ToString())
                        elem.Szin = jovaszin;
                }
                else
                {
                    //Szöveges tartalom kell a divekbe, mert különben elcsúszik a többi div ahol van
                    elem = new AttekintoElem() { JogcimNev = '.' };
                }
            }

            return elem;
        }
    }
}
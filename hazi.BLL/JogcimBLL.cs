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
        //jogcimek kiolvasása db-ből
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

        //jogcim kiolvasása ID alapján
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

        //db-be mentés
        public static string IdoBejelentesMentes(int? ID, DateTime Kezdeti, DateTime Vege,
            int JogcimId, string UserName, string LastEditUser, string torlesStatus)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                IdoBejelentes ib = null;
                if (ID == null)
                    ib = new IdoBejelentes();
                else
                {
                    ib = (from b in db.IdoBejelentes1
                          where b.ID == ID
                          select b).Single();
                }
                if (ib.Jogcim.Inaktiv == true)
                    return "A jogcím amire elmentette a bejelentését ezelőtt megváltozott! Így a mentési joga ennél a bejelentésnél elveszett! Csak megtekintési joga van!";

                ib.KezdetiDatum = Kezdeti;
                ib.VegeDatum = Vege;
                ib.Jogcim = (from b in db.Jogcims
                             where b.ID == JogcimId
                             select b).Single();
                if (UserName != "")
                    ib.UserName = UserName;
                ib.UtolsoModosito = LastEditUser;
                ib.UtolsoModositas = DateTime.Now;

                if (ID == null)
                    db.IdoBejelentes1.Add(ib);

                if (torlesStatus == string.Empty)
                    ib.Statusz = "NincsTorlesiKerelem";
                else
                    ib.Statusz = torlesStatus;          

                db.SaveChanges();
            }
            return string.Empty;
        }

        //DDL-be kiválasztott eleme ID-je
        public static int GetIDbyName(string Cim)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                var jog = (from b in db.Jogcims
                           where b.Cim == Cim
                           select b).Single();
                return jog.ID;
            }
        }

        public static List<UjBejelentes> GetIdoBejelentesById(int? id, string name, bool admin)
        {
            List<UjBejelentes> bejelentesek = new List<UjBejelentes>();

            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    if (admin == false) //nem admin
                    {
                        bejelentesek = (from b in db.IdoBejelentes1
                                        where b.ID == id && b.UserName == name
                                        select new UjBejelentes
                                        {
                                            ID = b.ID,
                                            KezdetiDatum = b.KezdetiDatum,
                                            VegeDatum = b.VegeDatum,
                                            LastEdit = b.UtolsoModosito,
                                            LastEditTime = b.UtolsoModositas.HasValue ?
                                                     b.UtolsoModositas.Value : DateTime.MinValue,
                                            JogcimNev = b.Jogcim.Cim,
                                            Statusz = b.Statusz
                                        }).ToList();

                    }
                    else //admin
                    {
                        bejelentesek = (from b in db.IdoBejelentes1
                                        where b.ID == id
                                        select new UjBejelentes
                                        {
                                            ID = b.ID,
                                            KezdetiDatum = b.KezdetiDatum,
                                            VegeDatum = b.VegeDatum,
                                            LastEdit = b.UtolsoModosito,
                                            LastEditTime = b.UtolsoModositas.HasValue ?
                                                     b.UtolsoModositas.Value : DateTime.MinValue,
                                            JogcimNev = b.Jogcim.Cim,
                                            Statusz = b.Statusz
                                        }).ToList();
                    }
                }

                foreach (var item in bejelentesek)
                {
                    string[] seged = item.Statusz.Split('&');
                    if (seged.Length > 1)
                        item.JovaStatus = seged[1];
                }
            }

            return bejelentesek;
        }

        //időbejelentések kiolvasása DB-ből
        public static IdoBejelentes GetIdoBejelentesById(int? id, bool mind, string name)
        {
            if (id.HasValue && id > 0)
            {
                using(hazi2Entities db = new hazi2Entities())
                {
                    if (mind)
                    {
                        try
                        {
                            var bej = (from b in db.IdoBejelentes1
                                       where b.ID == id
                                       select b).Single();
                            return bej;
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
                            var bej = (from b in db.IdoBejelentes1
                                       where b.ID == id &&
                                       !b.Statusz.Contains("RegisztraltKerelem") && b.UserName == name
                                       select b).Single();
                            return bej;
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

        //admini törlés db-ből bármelyik bejelentést
        public static string IdoBejelentesTorles(int? id)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    try
                    {
                        var torolni = (from b in db.IdoBejelentes1
                                       where b.ID == id
                                       select b).FirstOrDefault();
                        if (torolni != null)
                        {
                            db.IdoBejelentes1.Remove(torolni);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                        return "A törlés sikertelen";
                    }
                }
            }
            return string.Empty;
        }

        //admini törlés db-ből felhasználó által kért bejelentést
        public static string IdoBejelentesTorles(int? id, string status)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    try
                    {
                        var torolni = (from b in db.IdoBejelentes1
                                       where b.ID == id && b.Statusz.Contains(status)
                                       select b).FirstOrDefault();
                        if (torolni != null)
                        {
                            db.IdoBejelentes1.Remove(torolni);
                            db.SaveChanges();
                        }
                        else
                            return "A felhasználó nem kérte a(z) "+ id + " ID-jű jelentésnek a törlését, így a törlés sikertelen";
                    }
                    catch (Exception)
                    {
                        return "Hiba történt a törlés közben";
                    }
                }
            }
            return string.Empty;
        }

        //törlés elutasítva, bejegyzés újra megjelenítése a felhasználónak
        public static string TorlesElutasitva(int? id, string status, string ujStatus)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    try
                    {
                        IdoBejelentes ib = (from b in db.IdoBejelentes1
                                            where b.ID == id && b.Statusz.Contains(status)
                                            select b).Single();
                        
                        string[] seged = ib.Statusz.Split('&');
                        if (seged.Length > 1)
                            ib.Statusz = ujStatus + "&" + seged[1];
                        else
                            ib.Statusz = ujStatus;

                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return "A felhasználó nem kérte a(z)" + id + " ID-jű bejelentés törlésését, így azt nem lehet elutasítani";
                    }
                }
            }
            return string.Empty;
        }

        //felhasználó törlésre regisztrál egy bejelentést
        public static void TorlesRegisztracio(int? id, string status)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    try
                    {
                        IdoBejelentes ib = (from b in db.IdoBejelentes1
                                            where b.ID == id
                                            select b).Single();

                        ib.Statusz = status;

                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        public static void JogcimMentes(int? id, string nev, bool inaktiv)
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

                    db.SaveChanges();
                }
            }
        }
    } 
}

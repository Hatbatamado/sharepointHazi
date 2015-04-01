using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazi.BLL
{
    public class IdoBejelentesBLL
    {
        /// <summary>
        /// IdoBejelentés db-be mentése, admin bármikor, más felhasználó csak az aktív jogcímekre
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Kezdeti"></param>
        /// <param name="Vege"></param>
        /// <param name="JogcimId"></param>
        /// <param name="UserName"></param>
        /// <param name="LastEditUser"></param>
        /// <param name="torlesStatus"></param>
        /// <returns></returns>
        public static void IdoBejelentesMentes(int? ID, DateTime Kezdeti, DateTime Vege,
            string JogcimNev, string UserName, string LastEditUser, string torlesStatus, bool admin)
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

                ib.KezdetiDatum = Kezdeti;
                ib.VegeDatum = Vege;
                ib.Jogcim = (from b in db.Jogcims
                             where b.Cim == JogcimNev
                             select b).Single();
                if (UserName != "")
                    ib.UserName = UserName;
                ib.UtolsoModosito = LastEditUser;
                ib.UtolsoModositas = DateTime.Now;

                if (ID == null)
                    db.IdoBejelentes1.Add(ib);

                if (torlesStatus == string.Empty)
                    ib.Statusz = "Inaktiv";
                else
                    ib.Statusz = torlesStatus;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Időbejelentés kiolvása db-ből, adminnak mindet, más felhasználókat csak azt, amit nem kért törlésre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mind"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IdoBejelentes GetIdoBejelentesById(int? id, bool mind, string name)
        {
            if (id.HasValue && id > 0)
            {
                using (hazi2Entities db = new hazi2Entities())
                {
                    if (mind)
                    {
                        try
                        {
                            var bej = (from b in db.IdoBejelentes1
                                       where b.ID == id
                                       select b).Single();
                            bej.JogcimStatusz = (bool)bej.Jogcim.Inaktiv;
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
                                       !b.Statusz.Contains("RogzitettKerelem") && b.UserName == name
                                       select b).Single();
                            bej.JogcimStatusz = (bool)bej.Jogcim.Inaktiv;
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

        /// <summary>
        /// Admini törlés időbejelentés db-ből id alapján
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Admini törlés db-ből felhasználó által kért bejelentést
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
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
                            return "A felhasználó nem kérte a(z) " + id + " ID-jű jelentésnek a törlését, így a törlés sikertelen";
                    }
                    catch (Exception)
                    {
                        return "Hiba történt a törlés közben";
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Törlés elutasítva, bejegyzés újra megjelenítése a felhasználónak
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="ujStatus"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Felhasználó törlésre regisztrál egy bejelentést
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
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
    }
}

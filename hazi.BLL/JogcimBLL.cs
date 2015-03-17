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
        public static List<Jogcim> GetJogcimek()
        {
            List<Jogcim> jogcimek = new List<Jogcim>();
            using(hazi2Entities db = new hazi2Entities())
            {
                var jog = (from j in db.Jogcims
                           orderby j.ID
                           select j);

                foreach (var item in jog)
                {
                    jogcimek.Add(item);
                }
                return jogcimek;
            }
        }

        //jogcim kiolvasása ID alapján
        public static string GetJogcimById(int id)
        {
            using(hazi2Entities db = new hazi2Entities())
            {
                var jogcim = (from j in db.Jogcims
                              where j.ID == id
                              select j).Single();
                return jogcim.Cim;
            }
        }

        //db-be mentés
        public static void IdoBejelentesMentes(int? ID, DateTime Kezdeti, DateTime Vege,
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

                ib.KezdetiDatum = Kezdeti;
                ib.VegeDatum = Vege;
                ib.Jogcim = (from b in db.Jogcims
                             where b.ID == JogcimId
                             select b).Single();
                if (UserName != "")
                    ib.UserName = UserName;
                ib.LastEdit = LastEditUser;

                if (ID == null)
                    db.IdoBejelentes1.Add(ib);

                if (torlesStatus == string.Empty)
                    ib.TorlesStatus = "NincsTorlesiKerelem";
                else
                    ib.TorlesStatus = torlesStatus;

                db.SaveChanges();
            }
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

        //időbejelentések kiolvasása DB-ből
        public static IdoBejelentes GetIdoBejelentesById(int? id)
        {
            if (id.HasValue && id > 0)
            {
                using(hazi2Entities db = new hazi2Entities())
                {
                    var bej = (from b in db.IdoBejelentes1
                               where b.ID == id
                               select b).Single();
                    return bej;
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
                                       where b.ID == id && b.TorlesStatus == status
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
    }
}

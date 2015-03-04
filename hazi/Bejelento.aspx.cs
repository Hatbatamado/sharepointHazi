using hazi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi
{
    enum hibak { nincsHiba,
                 elsoDatumRegebbiMainal,
                 masodikDatumRegebbiMainal,
                 elsoDatumNincsKivalasztva,
                 masodikDatumNincsKivalasztva,
                 befejezesIdejeKezdesElottVan };

    public partial class Bejelento : System.Web.UI.Page
    {
        int idDB = 1;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Jogcimek kiolvasása DB-ből
        public IQueryable<Jogcim> GetJogcimek()
        {
            var _db = new hazi.Models.BejelentesContext();
            IQueryable<Jogcim> query = _db.Jogcimek;
            return query;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            //Home-ra navigálás
            HttpContext.Current.Response.Redirect("./");
        }

        protected void save_Click(object sender, EventArgs e)
        {
            hibak hiba = hibak.nincsHiba;
            DateTime datum1 = new DateTime();
            DateTime datum2 = new DateTime();

            //van-e kiválasztva dátum midenhol
            if (datepicker1.Text != "" && datepicker2.Text != "")
            {
                string[] seged;
                
                //az első dátum régebbi-e mainál
                seged = datepicker1.Text.Split('/');
                datum1 = new DateTime(Int32.Parse(seged[2]), Int32.Parse(seged[0]), Int32.Parse(seged[1]));
                if (datum1 < DateTime.Now)
                    hiba = hibak.elsoDatumRegebbiMainal;

                //ha még hiba nem történt
                if (hiba == hibak.nincsHiba)
                {
                    //a második dátum régebbi-e mainál
                    seged = datepicker2.Text.Split('/');
                    datum2 = new DateTime(Int32.Parse(seged[2]), Int32.Parse(seged[0]), Int32.Parse(seged[1]));
                    if (datum2 < DateTime.Now)
                        hiba = hibak.masodikDatumRegebbiMainal;
                }

                //ha még hiba nem történt
                if (hiba == hibak.nincsHiba)
                {
                    //befejezés ideje kezdés után van-e
                    if (datum1 > datum2)
                        hiba = hibak.befejezesIdejeKezdesElottVan;
                }
            }
            else
            {
                //első dátum ki van-e választva
                if (datepicker1.Text == "")
                    hiba = hibak.elsoDatumNincsKivalasztva;

                //ha még hiba nem történt
                if (hiba == hibak.nincsHiba)
                {
                    //második dátum ki van-e választva
                    if (datepicker2.Text == "")
                        hiba = hibak.masodikDatumNincsKivalasztva;
                }
            }

            //van-e valamilyen hiba mentés előtt
            if (hiba == hibak.nincsHiba)
            {
                var ujBejelentes = new IdoBejelentes();
                //adatok feltöltése
                ujBejelentes.ID = idDB++; //ID adása 1-ről indulva
                ujBejelentes.JogcimID = DropDownList1.SelectedIndex + 1; //listában lévő index + 1 = jogcim ID-vel
                ujBejelentes.KezdetiDatum = datum1; //fentebb beállított kezdei dátum
                ujBejelentes.VegeDatum = datum2; //fentebb beállított vége dátum

                //mentés
                using (var dbBejelentes = new BejelentesContext())
                {
                    dbBejelentes.IdoBejelentesek.Add(ujBejelentes);
                    dbBejelentes.SaveChanges();

                    string sikeresMentes = "<script language='javascript'>alert('A mentés sikeresen megtörtént!')</script>";
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "Register", sikeresMentes);
                }
            }
            else
            {
                //hibák ismertetése a felhasználónak
                string hibaUzenet = "";
                switch (hiba)
                {
                    case hibak.elsoDatumNincsKivalasztva:
                        hibaUzenet = "Az első dátum nincs kiválasztva, így a mentés sikertelen!";
                        break;
                    case hibak.masodikDatumNincsKivalasztva:
                        hibaUzenet = "A második dátum nincs kiválasztva, így a mentés sikertelen!";
                        break;
                    case hibak.elsoDatumRegebbiMainal:
                        hibaUzenet = "A folyamat kezdeti dátuma, a mai dátumnál régebbi, így a mentés sikertelen!";
                        break;
                    case hibak.masodikDatumRegebbiMainal:
                        hibaUzenet = "A folyamat befejező dátuma, a mai dátumnál régebbi, így a mentés sikertelen!";
                        break;
                    case hibak.befejezesIdejeKezdesElottVan:
                        hibaUzenet = "A folyamat befejező dátuma a kezdeti dátum előtt van, így a mentés sikertelen!";
                        break;
                }
                string hibaTortent = "<script language='javascript'>alert('" + hibaUzenet + "')</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "Register", hibaTortent);
            }
        }

    }
}
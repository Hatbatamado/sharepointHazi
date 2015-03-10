using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using hazi.BLL;
using hazi.DAL;

namespace hazi.WEB.Pages
{
    enum hibak
    {
        nincsHiba,
        elsoDatumRegebbiMostaninal,
        masodikDatumRegebbiMostaninal,
        elsoDatumNincsKivalasztva,
        masodikDatumNincsKivalasztva,
        befejezesIdejeKezdesElottVan
    };

    public partial class Bejelento : System.Web.UI.Page
    {
        int idDB = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                DropDownListFeltoltes();
        }


        private void DropDownListFeltoltes()
        {
            List<Jogcim> jogcimek = JogcimBLL.GetById();
            foreach (var item in jogcimek)
            {
                DropDownList1.Items.Add(item.Cim);
            }

        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            //Home-ra navigálás
            HttpContext.Current.Response.Redirect("./");
        }

        //readonly miatt a felhasználó nem tud hibás formátum-ú adatot megadni
        protected void save_Click(object sender, EventArgs e)
        {
            hibak hiba = hibak.nincsHiba;
            DateTime datum1 = new DateTime();
            DateTime datum2 = new DateTime();

            //van-e kiválasztva dátum midenhol
            if (datetimepicker1.Text != "" && datetimepicker2.Text != "")
            {
                string[] seged, segedido;

                //az első dátum régebbi-e mostaninál
                seged = datetimepicker1.Text.Split(' ');
                segedido = seged[1].Split(':');
                seged = seged[0].Split('/');
                datum1 = new DateTime(Int32.Parse(seged[0]), Int32.Parse(seged[1]), Int32.Parse(seged[2]),
                    Int32.Parse(segedido[0]), Int32.Parse(segedido[1]), 0);
                if (datum1 < DateTime.Now)
                    hiba = hibak.elsoDatumRegebbiMostaninal;

                //ha még hiba nem történt
                if (hiba == hibak.nincsHiba)
                {
                    //a második dátum régebbi-e mostaninál
                    seged = datetimepicker2.Text.Split(' ');
                    segedido = seged[1].Split(':');
                    seged = seged[0].Split('/');
                    datum2 = new DateTime(Int32.Parse(seged[0]), Int32.Parse(seged[1]), Int32.Parse(seged[2]),
                        Int32.Parse(segedido[0]), Int32.Parse(segedido[1]), 0);
                    if (datum2 < DateTime.Now)
                        hiba = hibak.masodikDatumRegebbiMostaninal;
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
                if (datetimepicker1.Text == "")
                    hiba = hibak.elsoDatumNincsKivalasztva;

                //ha még hiba nem történt
                if (hiba == hibak.nincsHiba)
                {
                    //második dátum ki van-e választva
                    if (datetimepicker2.Text == "")
                        hiba = hibak.masodikDatumNincsKivalasztva;
                }
            }

        //    //van-e valamilyen hiba mentés előtt
        //    if (hiba == hibak.nincsHiba)
        //    {
        //        var ujBejelentes = new IdoBejelentes();
        //        //adatok feltöltése
        //        ujBejelentes.ID = idDB++; //ID adása 1-ről indulva
        //        ujBejelentes.JogcimID = DropDownList1.SelectedIndex + 1; //listában lévő index + 1 = jogcim ID-vel
        //        ujBejelentes.KezdetiDatum = datum1; //fentebb beállított kezdei dátum
        //        ujBejelentes.VegeDatum = datum2; //fentebb beállított vége dátum

        //        //mentés
        //        using (var dbBejelentes = new BejelentesContext())
        //        {
        //            dbBejelentes.IdoBejelentesek.Add(ujBejelentes);
        //            dbBejelentes.SaveChanges();

        //            string sikeresMentes = "<script language='javascript'>alert('A mentés sikeresen megtörtént!')</script>";
        //            Page.ClientScript.RegisterClientScriptBlock(GetType(), "Register", sikeresMentes);
        //        }
        //    }
        //    else
        //    {
        //        //hibák ismertetése a felhasználónak
        //        string hibaUzenet = "";
        //        switch (hiba)
        //        {
        //            case hibak.elsoDatumNincsKivalasztva:
        //                hibaUzenet = "Az első dátum nincs kiválasztva, így a mentés sikertelen!";
        //                break;
        //            case hibak.masodikDatumNincsKivalasztva:
        //                hibaUzenet = "A második dátum nincs kiválasztva, így a mentés sikertelen!";
        //                break;
        //            case hibak.elsoDatumRegebbiMostaninal:
        //                hibaUzenet = "A folyamat kezdeti dátuma a mostani dátumnál / időnél régebbi, így a mentés sikertelen!";
        //                break;
        //            case hibak.masodikDatumRegebbiMostaninal:
        //                hibaUzenet = "A folyamat befejező dátuma a mostani dátumnál / időnél régebbi, így a mentés sikertelen!";
        //                break;
        //            case hibak.befejezesIdejeKezdesElottVan:
        //                hibaUzenet = "A folyamat befejező dátuma a kezdeti dátum előtt van, így a mentés sikertelen!";
        //                break;
        //        }
        //        string hibaTortent = "<script language='javascript'>alert('" + hibaUzenet + "')</script>";
        //        Page.ClientScript.RegisterClientScriptBlock(GetType(), "Register", hibaTortent);
        //    }
        }

    }
}
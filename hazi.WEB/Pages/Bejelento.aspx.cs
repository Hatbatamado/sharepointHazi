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
        HibasDatum,
        KezdetiDatumRegebbiMainal,
        HibasKezdetiIdo,
        HibasVegeIdo,
        HibasKezdetiErtekek,
        HibasVegeErtekek,
        VegeKezdetiElott
    };

    enum vizsgalat
    {
        MasodpercNulla,
        CsakDatum
    };

    public partial class Bejelento : System.Web.UI.Page
    {
        int idDB = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DropDownListFeltoltes();
                AlapAdatokFeltoltese();
            }
        }


        private void DropDownListFeltoltes()
        {
            List<Jogcim> jogcimek = JogcimBLL.GetById();
            foreach (var item in jogcimek)
            {
                DropDownList1.Items.Add(item.Cim);
            }
        }

        private void AlapAdatokFeltoltese()
        {
            //órák feltöltése DropDownList-be
            for (int i = 1; i < 25; i++)
            {
                ora1.Items.Add(i.ToString());
                ora2.Items.Add(i.ToString());
            }
            //percek feltöltése DropDownList-be 
            for (int i = 0; i < 60; i++)
            {
                perc1.Items.Add(i.ToString());
                perc2.Items.Add(i.ToString());
            }

            //Folyamat kezdése: alap értéknek a mostani idő beállítása
            ora1.SelectedIndex = DateTime.Now.Hour - 1;
            perc1.SelectedIndex = DateTime.Now.Minute;

            //Folyamat vége: alap értéknek a mostani +1 óra idő beállítása
            ora2.SelectedIndex = DateTime.Now.Hour;
            perc2.SelectedIndex = DateTime.Now.Minute;

            //alap értéknek a mai dátum beállítása
            datepicker.Text = DateTimeTosringMegfeleloModra(DateTime.Now);
        }

        private string DateTimeTosringMegfeleloModra(DateTime ido)
        {
            return ido.Year + "." + ido.Month + "." + ido.Day;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            //Home-ra navigálás
            HttpContext.Current.Response.Redirect("./");
        }

        private hibak Hibakereses()
        {
            string seged;
            //Kiválasztott dátum dátum-e
            DateTime datumKezdeti, datumVege;
            if (!DateTime.TryParse(datepicker.Text, out datumKezdeti))
                return hibak.HibasDatum;
            else
                datumVege = datumKezdeti;

            //Kiválasztott dátum régebbi-e a mai dátumnál
            if (IdoVizsgalat(vizsgalat.CsakDatum, DateTime.Now) > IdoVizsgalat(vizsgalat.CsakDatum, datumKezdeti))
                return hibak.KezdetiDatumRegebbiMainal;

            //Folyamat kezdeti óra és perc helyes-e
            seged = DateTimeTosringMegfeleloModra(datumKezdeti);
            seged += " " + ora1.SelectedItem.Text + ":" + perc1.SelectedItem.Text + ":00";
            if (!DateTime.TryParse(seged, out datumKezdeti))
                return hibak.HibasKezdetiIdo;

            //Folyamat vége óra és perc helyes-e
            seged = DateTimeTosringMegfeleloModra(datumVege);
            seged += " " + ora2.SelectedItem.Text + ":" + perc2.SelectedItem.Text + ":00";
            if (!DateTime.TryParse(seged, out datumVege))
                return hibak.HibasVegeIdo;

            //Folyamat kezdete régebbi-e a mostaninál
            if (IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > datumKezdeti)
                return hibak.HibasKezdetiErtekek;

            //Folyamat vége régebbi-e a mostaninál
            if (IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > datumVege)
                return hibak.HibasVegeErtekek;

            //Folyamat vége a kezdeti előtt van-e vagy megegyezik-e
            if (datumVege <= datumKezdeti)
                return hibak.VegeKezdetiElott;

            return hibak.nincsHiba;
        }

        //másodpercek nullázása
        private DateTime IdoVizsgalat(vizsgalat v, DateTime date)
        {
            TimeSpan ts = new TimeSpan();

            if (v == vizsgalat.MasodpercNulla)
                ts = new TimeSpan(date.Hour, date.Minute, 0);
            else if (v == vizsgalat.CsakDatum)
                ts = new TimeSpan(0, 0, 0);

            date = date.Date + ts;

            return date;
        }

        //hiba esetén a felhasználó értesítése
        private void HibaUzenetFelhasznalonak(hibak hiba)
        {
            string hibaUzenet = "";
            switch (hiba)
            {
                case hibak.HibasDatum:
                    hibaUzenet = "Hibás a dátum értéke, így a mentés sikertelen!";
                    break;
                case hibak.HibasKezdetiIdo:
                    hibaUzenet = "Hibás a folyamat kezdeti óra / perc értéke, így a mentés sikertelen!";
                    break;
                case hibak.HibasVegeIdo:
                    hibaUzenet = "Hibás a folyamat vége óra / perc értéke, így a mentés sikertelen!";
                    break;
                case hibak.HibasKezdetiErtekek:
                    hibaUzenet = "A folyamat kezdeti értékei a jelenlegi időpont előttiek, így a mentés sikertelen!";
                    break;
                case hibak.HibasVegeErtekek:
                    hibaUzenet = "A folyamat vége értékei a jelenlegi időpont előttiek, így a mentés sikertelen!";
                    break;
                case hibak.VegeKezdetiElott:
                    hibaUzenet = "A folyamat vége ideje a folyamat kezdeti előtt található vagy a két időpont megegyezik, így a mentés sikertelen!";
                    break;
                case hibak.KezdetiDatumRegebbiMainal:
                    hibaUzenet = "A kiválasztott dátum régebbi a mai dátumnál, így a mentés sikertelen!";
                    break;
            }
            hibaLabel.Visible = true;
            hibaLabel.Text = hibaUzenet;
        }

        //readonly miatt a felhasználó nem tud hibás formátum-ú adatot megadni
        protected void save_Click(object sender, EventArgs e)
        {
            //ha már volt előzőleg, egy hiba azt rejtsük el
            hibaLabel.Visible = false;

            //hibák keresése mentés előtt
            hibak hiba = Hibakereses();

            //hiba esetén a felhasználó értesítése
            if (hiba != hibak.nincsHiba)
                HibaUzenetFelhasznalonak(hiba);
            else
            {

            }


            //hibak hiba = hibak.nincsHiba;
            //DateTime datum1 = new DateTime();
            //DateTime datum2 = new DateTime();

            ////van-e kiválasztva dátum midenhol
            //if (datetimepicker1.Text != "" && datetimepicker2.Text != "")
            //{
            //    string[] seged, segedido;

            //    //az első dátum régebbi-e mostaninál
            //    seged = datetimepicker1.Text.Split(' ');
            //    segedido = seged[1].Split(':');
            //    seged = seged[0].Split('/');
            //    datum1 = new DateTime(Int32.Parse(seged[0]), Int32.Parse(seged[1]), Int32.Parse(seged[2]),
            //        Int32.Parse(segedido[0]), Int32.Parse(segedido[1]), 0);
            //    if (datum1 < DateTime.Now)
            //        hiba = hibak.elsoDatumRegebbiMostaninal;

            //    //ha még hiba nem történt
            //    if (hiba == hibak.nincsHiba)
            //    {
            //        //a második dátum régebbi-e mostaninál
            //        seged = datetimepicker2.Text.Split(' ');
            //        segedido = seged[1].Split(':');
            //        seged = seged[0].Split('/');
            //        datum2 = new DateTime(Int32.Parse(seged[0]), Int32.Parse(seged[1]), Int32.Parse(seged[2]),
            //            Int32.Parse(segedido[0]), Int32.Parse(segedido[1]), 0);
            //        if (datum2 < DateTime.Now)
            //            hiba = hibak.masodikDatumRegebbiMostaninal;
            //    }

            //    //ha még hiba nem történt
            //    if (hiba == hibak.nincsHiba)
            //    {
            //        //befejezés ideje kezdés után van-e
            //        if (datum1 > datum2)
            //            hiba = hibak.befejezesIdejeKezdesElottVan;
            //    }
            //}
            //else
            //{
            //    //első dátum ki van-e választva
            //    if (datetimepicker1.Text == "")
            //        hiba = hibak.elsoDatumNincsKivalasztva;

            //    //ha még hiba nem történt
            //    if (hiba == hibak.nincsHiba)
            //    {
            //        //második dátum ki van-e választva
            //        if (datetimepicker2.Text == "")
            //            hiba = hibak.masodikDatumNincsKivalasztva;
            //    }
            //}

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
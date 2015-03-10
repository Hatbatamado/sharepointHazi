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
        VegeKezdetiElott,
        Ismeretlen
    };

    enum vizsgalat
    {
        MasodpercNulla,
        CsakDatum
    };

    public partial class Bejelento : System.Web.UI.Page
    {
        Bejelentes bejelentes = null;
        hibak hiba = hibak.nincsHiba;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DropDownListFeltoltes();
                AlapAdatokFeltoltese();
            }
        }

        //DDL feltöltése jogcímekkel
        private void DropDownListFeltoltes()
        {
            List<Jogcim> jogcimek = JogcimBLL.GetJogcimek();
            foreach (var item in jogcimek)
            {
                DropDownList1.Items.Add(item.Cim);
            }
        }

        //dátum és idő mezők feltöltése és alap értékek beállítása
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

        //DateTime parse-hoz a dátum átalakítása
        private string DateTimeTosringMegfeleloModra(DateTime ido)
        {
            return ido.Year + "." + ido.Month + "." + ido.Day;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            //Home-ra navigálás
            HttpContext.Current.Response.Redirect("./../");
        }

        private hibak Hibakereses()
        {
            //Folyamat vége a kezdeti előtt van-e vagy megegyezik-e
            if (bejelentes.Vege <= bejelentes.Kezdeti)
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
                    //ennek sose szabadna lefutnia
                case hibak.Ismeretlen:
                    hibaUzenet = "Ismeretlen hiba történt, így a mentés sikertelen! Kérem próbálja újra!";
                    break;
            }
            hibaLabel.Visible = true;
            hibaLabel.Text = hibaUzenet;
        }

        protected void save_Click(object sender, EventArgs e)
        {
            //ha már volt előzőleg, egy hiba azt rejtsük el
            hibaLabel.Visible = false;
            //ha már volt egy sikeres mentésünk és most újabb lesz, azt rejtsük el
            mentesLabel.Visible = false;

            if (Page.IsValid)
            {
                //maradék hiba ellenőrzése
                hiba = Hibakereses();

                //hiba esetén a felhasználó értesítése
                if (hiba != hibak.nincsHiba)
                    HibaUzenetFelhasznalonak(hiba);
                else
                {
                    //db-be mentés
                    if (bejelentes != null)
                    {
                        Mentes();
                    }
                    //ennek sose szabadna lefutnia
                    else
                        HibaUzenetFelhasznalonak(hibak.Ismeretlen);
                }
            }
            else
                HibaUzenetFelhasznalonak(hiba);
        }

        private void Mentes()
        {
            JogcimBLL.IdoBejelentesMentes(null, bejelentes.Kezdeti, bejelentes.Vege,
                            JogcimBLL.GetIDbyName(DropDownList1.SelectedValue));
            mentesLabel.Visible = true;
            mentesLabel.Text = "A mentés sikeres!";

            //Sikeres mentés esetén a felhasználó egyszerűen visszatudjon navigálni a rendszer főoldalára
            Label5.Visible = false;
            datepicker.Visible = false;
            Label1.Visible = false;
            ora1.Visible = false;
            Label6.Visible = false;
            perc1.Visible = false;
            Label2.Visible = false;
            ora2.Visible = false;
            Label7.Visible = false;
            perc2.Visible = false;
            Label3.Visible = false;
            DropDownList1.Visible = false;
            save.Visible = false;

            cancel.Text = "Vissza";
        }

        //dátum validátor
        protected void CustomValidatorDatum_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                DateTime datumKezdeti = new DateTime();
                //Kiválasztott dátum dátum-e
                try
                {
                    datumKezdeti = DateTime.Parse(datepicker.Text);
                    bejelentes = new Bejelentes(datumKezdeti, datumKezdeti);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasDatum;
                    args.IsValid = false;
                    return;
                }

                //Kiválasztott dátum régebbi-e a mai dátumnál
                if (IdoVizsgalat(vizsgalat.CsakDatum, DateTime.Now) > IdoVizsgalat(vizsgalat.CsakDatum, datumKezdeti))
                {
                    hiba = hibak.KezdetiDatumRegebbiMainal;
                    args.IsValid = false;
                }
            }
        }

        //folyamat kezdeti időpont validátor
        protected void CustomValidatorIdopont1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                string seged;

                //Folyamat kezdeti óra és perc helyes-e
                seged = DateTimeTosringMegfeleloModra(bejelentes.Kezdeti);
                seged += " " + ora1.SelectedItem.Text + ":" + perc1.SelectedItem.Text + ":00";
                try
                {
                    bejelentes.Kezdeti = DateTime.Parse(seged);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasKezdetiIdo;
                    args.IsValid = false;
                    return;
                }

                //Folyamat kezdete régebbi-e a mostaninál
                if (IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > bejelentes.Kezdeti)
                    hiba = hibak.HibasKezdetiErtekek;
            }
        }

        //folyamat vége időpont validátor
        protected void CustomValidatorIdopont2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                string seged;

                //Folyamat vége óra és perc helyes-e
                seged = DateTimeTosringMegfeleloModra(bejelentes.Vege);
                seged += " " + ora2.SelectedItem.Text + ":" + perc2.SelectedItem.Text + ":00";
                try
                {
                    bejelentes.Vege = DateTime.Parse(seged);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasVegeIdo;
                    args.IsValid = false;
                    return;
                }

                //Folyamat vége régebbi-e a mostaninál
                if (IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > bejelentes.Vege)
                    hiba = hibak.HibasVegeErtekek;
            }
        }

    }
}
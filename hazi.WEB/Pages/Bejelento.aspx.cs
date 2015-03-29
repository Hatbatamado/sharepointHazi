using hazi.BLL;
using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    #region enum
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
        IbNincsDBben,
        NemEngedelyezettEleres,
        Ismeretlen
    };

    enum vizsgalat
    {
        MasodpercNulla,
        CsakDatum
    };
    #endregion

    public partial class Bejelento : System.Web.UI.Page
    {
        public int? Id
        {
            get
            {
                return (int?)ViewState["Id"];
            }
            set
            {
                ViewState["Id"] = value;
            }
        }

        hibak hiba = hibak.nincsHiba;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    BejelentoForm.Visible = true;
                    DropDownListFeltoltes();
                    AlapAdatokFeltoltese();
                    
                    if (Request.QueryString["ID"] != null)
                    {
                        Bejelentes.UjBejelentes = false;
                        Id = Int32.Parse(Request.QueryString["ID"]);
                        IdoBejelentes ib;
                        if (RoleActions.IsInRole(User.Identity.Name, RegisterUserAs.Admin.ToString()))
                            ib = IdoBejelentesBLL.GetIdoBejelentesById(Id.Value, true, string.Empty);
                        else
                            ib = IdoBejelentesBLL.GetIdoBejelentesById(Id.Value, false, User.Identity.Name);
                        if (ib == null)
                            HibaUzenetFelhasznalonak(hibak.IbNincsDBben);
                        else
                            AdatokFeltolteseIdAlapjan(ib);
                    }
                    else
                    {
                        Bejelentes.UjBejelentes = true;
                        AlapErtekekBeallitasa();
                    }
                    if (Request.QueryString["status"] == "popup")
                    {
                        Master.HeaderDiv.Visible = false;
                    }
                }
                else
                {
                    BejelentoForm.Visible = false;
                    Response.Redirect("/Account/Login.aspx");
                }
            }
        }

        #region DDL-ek feltoltese + alaperteke
        //DDL feltöltése jogcímekkel
        private void DropDownListFeltoltes()
        {
            List<Jogcim> jogcimek = JogcimBLL.GetJogcimek(false);
            foreach (var item in jogcimek)
            {
                DropDownList1.Items.Add(item.Cim);
            }
        }

        //dátum és idő mezők feltöltése
        private void AlapAdatokFeltoltese()
        {
            //órák feltöltése DropDownList-be
            for (int i = 0; i < 24; i++)
            {
                ora1.Items.Add(i.ToString("00"));
                ora2.Items.Add(i.ToString("00"));
            }
            //percek feltöltése DropDownList-be 
            for (int i = 0; i < 60; i++)
            {
                perc1.Items.Add(i.ToString("00"));
                perc2.Items.Add(i.ToString("00"));
            }
        }

        private void AdatokFeltolteseIdAlapjan(IdoBejelentes ib)
        {
            //dátum
            datepicker.Text = DateTimeTosringMegfeleloModra(ib.KezdetiDatum);

            //folyamat kezdeti ideje
            ora1.SelectedValue = ib.KezdetiDatum.Hour.ToString("00");
            perc1.SelectedValue = ib.KezdetiDatum.Minute.ToString("00");

            //folyamat vége ideje
            ora2.SelectedValue = ib.VegeDatum.Hour.ToString("00");
            perc2.SelectedValue = ib.VegeDatum.Minute.ToString("00");

            //jogcim kiválasztása
            DropDownList1.SelectedValue = JogcimBLL.GetJogcimById(ib.JogcimID);
        }

        //alap értékek beállítása
        private void AlapErtekekBeallitasa()
        {
            //Folyamat kezdése: alap értéknek a mostani idő beállítása
            ora1.SelectedValue = DateTime.Now.Hour.ToString("00");
            perc1.SelectedValue = DateTime.Now.Minute.ToString("00");

            //Folyamat vége: alap értéknek a mostani idő +1 óra beállítása
            int segedOra = DateTime.Now.Hour;
            if ((segedOra + 1) < 24)
                ora2.SelectedValue = (segedOra + 1).ToString("00");
            else
                ora2.SelectedValue = (segedOra - 23).ToString("00");
            perc2.SelectedValue = DateTime.Now.Minute.ToString("00");

            //alap értéknek a mai dátum beállítása
            datepicker.Text = DateTimeTosringMegfeleloModra(DateTime.Now);
        }
        #endregion

        #region Maradek hiba kereses es kezeles
        private hibak Hibakereses()
        {
            //Folyamat vége a kezdeti előtt van-e vagy megegyezik-e
            if (Bejelentes.Vege <= Bejelentes.Kezdeti)
                return hibak.VegeKezdetiElott;

            return hibak.nincsHiba;
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
                    hibaUzenet = "Múltbeli folyamat rögzítése nem engedélyezett!A folyamat kezdeti értékei a jelenlegi időpont előttiek, így a mentés sikertelen!";
                    break;
                case hibak.HibasVegeErtekek:
                    hibaUzenet = "Múltbeli / 2 napos folyamat rögzítése nem engedélyezett! A folyamat vége értékei a jelenlegi időpont előttiek vagy átnyúlik a következő napra, így a mentés sikertelen!";
                    break;
                case hibak.VegeKezdetiElott:
                    hibaUzenet = "A folyamat vége ideje a folyamat kezdeti előtt található, a két időpont megegyezik, " +
                                    "vagy átnyúlik a következő napra, így a mentés sikertelen!";
                    break;
                case hibak.KezdetiDatumRegebbiMainal:
                    hibaUzenet = "Múltbeli folyamat rögzítése nem engedélyezett! A kiválasztott dátum régebbi a mai dátumnál, így a mentés sikertelen!";
                    break;
                //ezeknek sose szabadna lefutnia
                case hibak.Ismeretlen:
                    hibaUzenet = "Ismeretlen hiba történt, így a mentés sikertelen! Kérem próbálja újra!";
                    break;
                case hibak.IbNincsDBben:
                    ElemekElrejtese();
                    hibaUzenet = "A kiválasztott elem nem található a db-ben! Vagy nincs megtekintési joga a megadott bejelentésehez!";
                    break;
            }
            Master.Uzenet.Visible = true;
            Master.Uzenet.Text = hibaUzenet;
        }
        #endregion

        #region datum metodusok
        //DateTime parse-hoz a dátum átalakítása
        private string DateTimeTosringMegfeleloModra(DateTime ido)
        {
            return ido.Year + "." + ido.Month.ToString("00") + "." + ido.Day;
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
        #endregion

        #region save / cancel klikk
        protected void cancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["status"] == "popup")
            {
                Response.Write("<script>window.close();</" + "script>");
                Response.End();
            }
            else
                //Home-ra navigálás
                Response.Redirect("/Default.aspx");
        }

        protected void save_Click(object sender, EventArgs e)
        {
            //ha már volt előzőleg, egy hiba azt rejtsük el
            Master.Uzenet.Visible = false;
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
                    if (Bejelentes.Kezdeti != null && Bejelentes.Vege != null)
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
        #endregion

        #region Mentes + elemek elrejtese
        private void Mentes()
        {
            string ujBejelentoUser;
            if (Bejelentes.UjBejelentes)
                ujBejelentoUser = User.Identity.Name;
            else
                ujBejelentoUser = "";
            string uzenet = string.Empty;
            if (!RoleActions.IsInRole(User.Identity.Name, RegisterUserAs.Admin.ToString()))
                uzenet = IdoBejelentesBLL.IdoBejelentesMentes(Id, Bejelentes.Kezdeti, Bejelentes.Vege,
                             DropDownList1.SelectedValue, ujBejelentoUser, User.Identity.Name, TorlesStatus.Inaktiv.ToString(), false);
            else
                uzenet = IdoBejelentesBLL.IdoBejelentesMentes(Id, Bejelentes.Kezdeti, Bejelentes.Vege,
                             DropDownList1.SelectedValue, ujBejelentoUser, User.Identity.Name, TorlesStatus.Inaktiv.ToString(), true);

            if (uzenet != string.Empty)
            {
                Master.Uzenet.Visible = true;
                Master.Uzenet.Text = uzenet;
                return;
            }

            //jóváhagyásos státuszok megfelelő beállítása:
            JovahagyBLL.GetJovahagyAll(string.Empty);

            mentesLabel.Visible = true;
            mentesLabel.Text = "A mentés sikeres!";

            //Sikeres mentés esetén a felhasználó egyszerűen visszatudjon navigálni a rendszer főoldalára
            ElemekElrejtese();
        }

        private void ElemekElrejtese()
        {
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
        #endregion

        #region validatorok
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
                    Bejelentes.Kezdeti = datumKezdeti;
                    Bejelentes.Vege = datumKezdeti;
                }
                catch (Exception)
                {
                    hiba = hibak.HibasDatum;
                    args.IsValid = false;
                    return;
                }

                //Kiválasztott dátum régebbi-e a mai dátumnál (csak új bejelentés esetén)
                if (Bejelentes.UjBejelentes && IdoVizsgalat(vizsgalat.CsakDatum, DateTime.Now) > IdoVizsgalat(vizsgalat.CsakDatum, datumKezdeti))
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
                seged = DateTimeTosringMegfeleloModra(Bejelentes.Kezdeti);
                seged += " " + ora1.SelectedItem.Text + ":" + perc1.SelectedItem.Text + ":00";
                try
                {
                    Bejelentes.Kezdeti = DateTime.Parse(seged);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasKezdetiIdo;
                    args.IsValid = false;
                    return;
                }

                //Folyamat kezdete régebbi-e a mostaninál (csak új bejelentés esetén)
                if (Bejelentes.UjBejelentes && IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > Bejelentes.Kezdeti)
                {
                    hiba = hibak.HibasKezdetiErtekek;
                    args.IsValid = false;
                }
            }
        }

        //folyamat vége időpont validátor
        protected void CustomValidatorIdopont2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                string seged;

                //Folyamat vége óra és perc helyes-e
                seged = DateTimeTosringMegfeleloModra(Bejelentes.Vege);
                seged += " " + ora2.SelectedItem.Text + ":" + perc2.SelectedItem.Text + ":00";
                try
                {
                    Bejelentes.Vege = DateTime.Parse(seged);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasVegeIdo;
                    args.IsValid = false;
                    return;
                }

                //Folyamat vége régebbi-e a mostaninál (csak új bejelentés esetén)
                if (Bejelentes.UjBejelentes && IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > Bejelentes.Vege)
                {
                    hiba = hibak.HibasVegeErtekek;
                    args.IsValid = false;
                }
            }
        }
        #endregion
    }
}
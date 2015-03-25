using hazi.BLL;
using hazi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    

    public class Bejelento
    {
        //DDL feltöltése jogcímekkel
        public static void DropDownListFeltoltes(DropDownList ddl)
        {
            List<Jogcim> jogcimek = JogcimBLL.GetJogcimek(false);
            foreach (var item in jogcimek)
            {
                ddl.Items.Add(item.Cim);
            }
        }

        //dátum és idő mezők feltöltése
        public static void AlapAdatokFeltoltese(DropDownList ora1, DropDownList ora2, DropDownList perc1, DropDownList perc2)
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

        public static void AdatokFeltolteseIdAlapjan(IdoBejelentes ib, TextBox datepicker, DropDownList ora1, DropDownList ora2,
            DropDownList perc1, DropDownList perc2, DropDownList ddl)
        {
            //dátum
            datepicker.Text = DateTimeTosringMegfeleloModra(ib.KezdetiDatum);

            //folyamat kezdeti ideje
            ora1.SelectedIndex = ib.KezdetiDatum.Hour;
            perc1.SelectedIndex = ib.KezdetiDatum.Minute;

            //folyamat vége ideje
            ora2.SelectedIndex = ib.VegeDatum.Hour;
            perc2.SelectedIndex = ib.VegeDatum.Minute;

            //jogcim kiválasztása
            ddl.SelectedValue = JogcimBLL.GetJogcimById(ib.JogcimID);
        }

        //alap értékek beállítása
        public static void AlapErtekekBeallitasa(TextBox datepicker, DropDownList ora1, DropDownList ora2, DropDownList perc1, DropDownList perc2)
        {
            //Folyamat kezdése: alap értéknek a mostani idő beállítása
            ora1.SelectedIndex = DateTime.Now.Hour;
            perc1.SelectedIndex = DateTime.Now.Minute;

            //Folyamat vége: alap értéknek a mostani idő +1 óra beállítása
            int segedOra = DateTime.Now.Hour;
            if ((segedOra + 1) < 24)
                ora2.SelectedIndex = segedOra + 1;
            else
                ora2.SelectedIndex = segedOra - 23;
            perc2.SelectedIndex = DateTime.Now.Minute;

            //alap értéknek a mai dátum beállítása
            datepicker.Text = DateTimeTosringMegfeleloModra(DateTime.Now);
        }

        //DateTime parse-hoz a dátum átalakítása
        public static string DateTimeTosringMegfeleloModra(DateTime ido)
        {
            return ido.Year + "." + ido.Month + "." + ido.Day;
        }

        //másodpercek nullázása
        public static DateTime IdoVizsgalat(vizsgalat v, DateTime date)
        {
            TimeSpan ts = new TimeSpan();

            if (v == vizsgalat.MasodpercNulla)
                ts = new TimeSpan(date.Hour, date.Minute, 0);
            else if (v == vizsgalat.CsakDatum)
                ts = new TimeSpan(0, 0, 0);

            date = date.Date + ts;

            return date;
        }

        public static hibak Hibakereses()
        {
            //Folyamat vége a kezdeti előtt van-e vagy megegyezik-e
            if (Bejelentes.Vege <= Bejelentes.Kezdeti)
                return hibak.VegeKezdetiElott;

            return hibak.nincsHiba;
        }

        //hiba esetén a felhasználó értesítése
        public static void HibaUzenetFelhasznalonak(hibak hiba, Label uzenet)
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
                    hibaUzenet = "A kiválasztott elem nem található a db-ben! Vagy nincs megtekintési joga a megadott bejelentésehez!";
                    break;
            }
            uzenet.Visible = true;
            uzenet.Text = hibaUzenet;
        }

        public static void Mentes(int? Id, DropDownList ddl, Label Uzenet, Label mentesLabel)
        {
            string ujBejelentoUser;
            if (Bejelentes.UjBejelentes)
                ujBejelentoUser = HttpContext.Current.User.Identity.Name;
            else
                ujBejelentoUser = "";
            string uzenet = JogcimBLL.IdoBejelentesMentes(Id, Bejelentes.Kezdeti, Bejelentes.Vege,
                            JogcimBLL.GetIDbyName(ddl.SelectedValue),
                            ujBejelentoUser, HttpContext.Current.User.Identity.Name, TorlesStatus.NincsTorlesiKerelem.ToString());
            if (uzenet != string.Empty)
            {
                Uzenet.Visible = true;
                Uzenet.Text = uzenet;
                return;
            }

            //jóváhagyásos státuszok megfelelő beállítása:
            JovahagyBLL.GetJovahagyAll();

            mentesLabel.Visible = true;
            mentesLabel.Text = "A mentés sikeres!";
        }
    }
}
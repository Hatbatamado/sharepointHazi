using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class Attekinto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                AdatokFeltoltese(DateTime.Now.Year, User.Identity.Name);
            }
            else if (!IsPostBack && !User.Identity.IsAuthenticated)
                Error404.HibaDobas(Response);
            if (Request["__EVENTARGUMENT"] == "TextChangedJobbra")
            {
                int ev = Convert.ToInt32(evLabel.Text) + 1;
                AdatokFeltoltese(ev, AttekintoUserKeresoTB.Text);
                evLabel.Text = ev.ToString();
            }
            else if (Request["__EVENTARGUMENT"] == "TextChangedBalra")
            {
                int ev = Convert.ToInt32(evLabel.Text) - 1;
                AdatokFeltoltese(ev, AttekintoUserKeresoTB.Text);
                evLabel.Text = ev.ToString();
            }
        }

        /// <summary>
        /// Áttekintő feltöltése adatokkal év és felhasználó alapján
        /// </summary>
        /// <param name="year"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string AdatokFeltoltese(int year, string user)
        {
            if (user == "" || user == null)
                user = User.Identity.Name;
            else if (!UsersBLL.FelhasznaloIsInDB(user))
                return "nincs";

            if (!RoleActions.IsInRole(User.Identity.Name, Konstansok.normal))
                AttekintoUserKereso.Visible = true;
            else
                AttekintoUserKereso.Visible = false;

            Jelmagyarazat();

            List<AttekintoViewModel> kulsoLista = new List<AttekintoViewModel>();
            for (int i = 1; i <= 12; i++)
            {
                kulsoLista.Add(new AttekintoViewModel(year, i, user)
                {
                    HonapNeve = new DateTime(year, i, 1).
                      ToString("MMMM", CultureInfo.CreateSpecificCulture("hu-HU"))
                });
            }

            KulsoRepeater.DataSource = kulsoLista;
            KulsoRepeater.DataBind();

            List<Napokszama> napok = new List<Napokszama>();
            for (int i = 1; i <= 31; i++)
            {
                napok.Add(new Napokszama() { napokSzama = i });
            }
            napokSzama.DataSource = napok;
            napokSzama.DataBind();
            
            return string.Empty;
        }

        /// <summary>
        /// Jelmagyarázat repeater feltöltése
        /// </summary>
        private void Jelmagyarazat()
        {
            List<JelMagy> jelmagy = UjJogcimBLL.GetJelMagy();
            JelmagyarazatRepeater.DataSource = jelmagy;
            JelmagyarazatRepeater.DataBind();
        }

        /// <summary>
        /// kockák kiszínezése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BelsoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string szin = (e.Item.DataItem as AttekintoElem).Szin;
            string fontcolor = "";
            if (szin == "" || szin == null)
            {
                szin = Konstansok.alapSzin;
                fontcolor = Konstansok.alapSzin;
            }
            else
                fontcolor = Konstansok.alapFontSzin;
            HtmlGenericControl divRog = (HtmlGenericControl)(e.Item.FindControl("bejelentesKocka"));

            if (divRog != null)
                divRog.Attributes["style"] += ("background:" + szin + "; color: " + fontcolor + ";)"); 
        }

        /// <summary>
        /// jelmagyarázat kockák kiszínezése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void JelmagyarazatRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlGenericControl div = (HtmlGenericControl)(e.Item.FindControl("jelSzin"));
            string szin = (e.Item.DataItem as JelMagy).Szin;
            if (div != null)
                div.Attributes["style"] += ("background:" + szin + "; color: " + Konstansok.alapFontSzin + ";)");
        }

        /// <summary>
        /// felhasználó keresése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void kereses_Click(object sender, EventArgs e)
        {
            //ha már volt sikeretelen keresés
            Master.Uzenet.Visible = false;

            int ev = Convert.ToInt32(evLabel.Text);
            string nincs = AdatokFeltoltese(ev, AttekintoUserKeresoTB.Text);
            if (nincs == "nincs")
            {
                Master.Uzenet.Visible = true;
                Master.Uzenet.Text = "A keresett felhasználó nem található!";
            }

        }
    }
}
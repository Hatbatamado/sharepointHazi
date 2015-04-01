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
            if (!IsPostBack)
            {
                AdatokFeltoltese(DateTime.Now.Year);
            }
            if (Request["__EVENTARGUMENT"] == "TextChangedJobbra")
            {
                int ev = Convert.ToInt32(evLabel.Text) + 1;
                AdatokFeltoltese(ev);
                evLabel.Text = ev.ToString();
            }
            else  if (Request["__EVENTARGUMENT"] == "TextChangedBalra")
            {
                int ev = Convert.ToInt32(evLabel.Text) - 1;
                AdatokFeltoltese(ev);
                evLabel.Text = ev.ToString();
            }
        }

        private void AdatokFeltoltese(int year)
        {
            Jelmagyarazat();

            string user = User.Identity.Name;

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

        }

        private void Jelmagyarazat()
        {
            List<JelMagy> jelmagy = UjJogcimBLL.GetJelMagy();
            JelmagyarazatRepeater.DataSource = jelmagy;
            JelmagyarazatRepeater.DataBind();
        }

        protected void BelsoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string szin = (e.Item.DataItem as AttekintoElem).Szin;
            string fontcolor = "";
            if (szin == "" || szin == null)
            {
                szin = "lightgreen";
                fontcolor = "lightgreen";
            }
            else
                fontcolor = "black";
            HtmlGenericControl divRog = (HtmlGenericControl)(e.Item.FindControl("bejelentesKocka"));

            if (divRog != null)
                divRog.Attributes["style"] += ("background:" + szin + "; color: " + fontcolor + ";)"); 
        }

        protected void JelmagyarazatRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlGenericControl div = (HtmlGenericControl)(e.Item.FindControl("jelSzin"));
            string szin = (e.Item.DataItem as JelMagy).Szin;
            if (div != null)
                div.Attributes["style"] += ("background:" + szin + "; color: black;)");   
        }
    }
}
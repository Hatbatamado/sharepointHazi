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
            int year = DateTime.Now.Year;
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

        protected void BelsoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string RogzitveColor = "yellow";
            string JovahagyvaColor = "darkgreen";
            string ElutasitvaColor = "red";
            HtmlGenericControl divRog = (HtmlGenericControl)(e.Item.FindControl("bejelentesKocka"));

            if (divRog != null && divRog.Attributes["class"].Contains(JovaHagyasStatus.Rogzitve.ToString()))
                divRog.Attributes["style"] += ("background:" + RogzitveColor + "; color: black;)");
            else if (divRog != null && divRog.Attributes["class"].Contains(JovaHagyasStatus.Jovahagyva.ToString()))
                divRog.Attributes["style"] += ("background:" + JovahagyvaColor + "; color: black;)");
            else if (divRog != null && divRog.Attributes["class"].Contains(JovaHagyasStatus.Elutasitva.ToString()))
                divRog.Attributes["style"] += ("background:" + ElutasitvaColor + "; color: black;)");
        }
    }
}
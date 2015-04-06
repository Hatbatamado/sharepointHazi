using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using hazi.WEB.Models;

namespace hazi.WEB.Pages
{
    public partial class HaviAttekinto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Error404.HibaDobas(Response);
                else
                {
                    bool vezeto = UsersBLL.IsManager(User.Identity.Name);
                    if (!vezeto)
                        Error404.HibaDobas(Response);
                    else
                        HaviMegjelenites(vezeto);
                }
            }
        }

        private void HaviMegjelenites(bool vezeto)
        {
            List<HiearchiaOsszeg> lista = new List<HiearchiaOsszeg>();
            List<string> userlista = UsersBLL.GetUserNames();
            userlista.RemoveAt(0);
            while (userlista.Count > 0)
                JovahagyBLL.Lekerdezes2(userlista[0], lista, userlista);
            
            lista = lista.OrderBy(o => o.UserCount).ToList();

            List<HaviAttekintoElem> hv = new List<HaviAttekintoElem>();
            while (lista.Count > 0)
                hv.Add(JovahagyBLL.Lekerdezes(lista[lista.Count - 1].UserName, lista));

            List<HaviAttekintoViewModel> hvRep = new List<HaviAttekintoViewModel>();
            foreach (var item in hv)
            {
                if (item.UsersLista.Count != 0)
                    hvRep.Add(new HaviAttekintoViewModel(2015, 04, item.UserName)
                    {
                        Nev = item.UserName,
                        RangVezeto = '+'
                    });
                else
                    hvRep.Add(new HaviAttekintoViewModel(2015, 04, item.UserName)
                    {
                        Nev = item.UserName,
                        RangNormal = '&'
                    });
            }
            KulsoRepeater.DataSource = hvRep;
            KulsoRepeater.DataBind();
        }

        protected void RangLinkB_Command(object sender, CommandEventArgs e)
        {
            var asd = e.CommandArgument;
            ((LinkButton)sender).Text = "-";
        }
    }
}
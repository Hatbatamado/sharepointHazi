using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            HaviAttekintoElem hv = JovahagyBLL.Lekerdezes(User.Identity.Name);
        }
    }
}
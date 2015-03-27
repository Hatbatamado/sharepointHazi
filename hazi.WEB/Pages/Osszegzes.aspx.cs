using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class Osszegzes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Uzenet.Text = "Az Ön " + DateTime.Now.Month + ". havi bejelentéseink összegzése:";
                    OsszegzesGV.DataSource = OsszegzoBLL.GetOsszegzes(User.Identity.Name, DateTime.Now);
                    OsszegzesGV.DataBind();
                }
                else
                    Error404.HibaDobas(Response);
            }
        }
    }
}
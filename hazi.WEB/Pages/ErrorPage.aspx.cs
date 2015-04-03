using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        /// <summary>
        /// 404-es hiba dobása a felhasználónak
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMsg = Request.QueryString["msg"];
            if (errorMsg == "404")
            {
                Master.Uzenet.Visible = true;
                Master.Uzenet.Text = "404 - A keresett oldal nem található!";
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class Naptar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Clear();
                Response.StatusCode = 404;
                Response.End();
            }
            //ControlCollection asd = Parent.Controls;
            int a = 0;
        }
    }
}
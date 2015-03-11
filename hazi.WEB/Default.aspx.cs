using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public IQueryable GetIdoBejelentesek()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString())
                {
                    var db = new hazi.DAL.hazi2Entities();
                    IQueryable query = db.IdoBejelentes1;
                    return query;
                }
            }
            return null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Bejelento");
        }
    }
}
using hazi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi
{
    public partial class Bejelento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Jogcim> GetJogcimek()
        {
            var _db = new hazi.Models.BejelentesContext();
            IQueryable<Jogcim> query = _db.Jogcimek;
            return query;
        }
    }
}
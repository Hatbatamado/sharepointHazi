using hazi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi
{
    enum hibak { nincsHiba,
                 elsoDatumRegebbiMainal,
                 masodikDatumRegebbiMainal,
                 elsoDatumNincsKivalasztva,
                 masodikDatumNincsKivalasztva,
                 befejezesIdejeKezdesElottVan };

    public partial class Bejelento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //csak olvasható lehet a dátum kiválasztva, így több hibát is elkerülünk
                datepicker1.ReadOnly = true;
                datepicker2.ReadOnly = true;
            }
        }

        //Jogcimek kiolvasása DB-ből
        public IQueryable<Jogcim> GetJogcimek()
        {
            var _db = new hazi.Models.BejelentesContext();
            IQueryable<Jogcim> query = _db.Jogcimek;
            return query;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            //Home-ra navigálás
            HttpContext.Current.Response.Redirect("./");
        }

        protected void save_Click(object sender, EventArgs e)
        {
            hibak hiba = hibak.nincsHiba;
            //van-e kiválasztva dátum midenhol
            if (datepicker1.Text != null && datepicker2.Text != null)
            {
                DateTime datum1, datum2;
                string[] seged;
                
                //az első dátum régebbi-e mainál
                seged = datepicker1.Text.Split('/');
                datum1 = new DateTime(Int32.Parse(seged[2]), Int32.Parse(seged[1]), Int32.Parse(seged[0]));
                if (datum1 < DateTime.Now)
                    hiba = hibak.elsoDatumRegebbiMainal;

                //a második dátum régebbi-e mainál
                seged = datepicker2.Text.Split('/');
                datum2 = new DateTime(Int32.Parse(seged[2]), Int32.Parse(seged[1]), Int32.Parse(seged[0]));
                if (datum2 < DateTime.Now)
                    hiba = hibak.masodikDatumRegebbiMainal;

                //befejezés ideje kezdés után van-e
                if (datum1 > datum2)
                    hiba = hibak.befejezesIdejeKezdesElottVan;
            }
            else
            {
                //első dátum ki van-e választva
                if (datepicker1.Text == null)
                    hiba = hibak.elsoDatumNincsKivalasztva;
                
                //második dátum ki van-e választva
                if (datepicker2.Text == null)
                    hiba = hibak.masodikDatumNincsKivalasztva;
            }

            //van-e valamilyen hiba mentés előtt
            if (hiba == hibak.nincsHiba)
            {

            }
        }

    }
}
using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace hazi.WEB
{
    /// <summary>
    /// Summary description for Services
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Services : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetEventsBetween(string start, string end)
        {
            DateTime segedStart = DateTime.ParseExact(start, "ddd MMM dd yyyy HH:mm:ss 'GMT'K",
                CultureInfo.InvariantCulture).AddHours(-1); //-1 óra = átalakítás után éjfél
            DateTime segedEnd = DateTime.ParseExact(end, "ddd MMM dd yyyy HH:mm:ss 'GMT'K",
                CultureInfo.InvariantCulture).AddHours(-2); //-2 óra = átalakítás után éjfél

            List<UjBejelentes> bejelentesek = ListaAdat(User.Identity.Name, segedStart, segedEnd);

            IList<CalendarDTO> tasksList = TaskFeltoltes(bejelentesek);

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
             new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(tasksList);
            return sJSON;         
        }

        private List<UjBejelentes> ListaAdat(string UName, DateTime start, DateTime end)
        {
            string admin = RegisterUserAs.Admin.ToString();
            string normal = RegisterUserAs.NormalUser.ToString();
            string jovahagy = RegisterUserAs.Jovahagyok.ToString();
            List<UjBejelentes> lista = new List<UjBejelentes>();
            if (RoleActions.IsInRole(UName, admin))
            {
                lista = UjBejelentesBLL.GetIdoBejelentesek(admin, UName, start, end);
                JovahagyBLL.StatuszBeallitasok(lista, true);
            }
            else if (RoleActions.IsInRole(UName, normal))
            {
                lista = UjBejelentesBLL.GetIdoBejelentesek(normal, UName, start, end);
                JovahagyBLL.StatuszBeallitasok(lista, false);
            }
            else if (RoleActions.IsInRole(UName, jovahagy))
            {
                lista = UjBejelentesBLL.GetIdoBejelentesek(jovahagy, UName, start, end);
                JovahagyBLL.StatuszBeallitasok(lista, false);
            }           

            return lista;
        }

        public class CalendarDTO
        {
            public int id { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string color { get; set; }
            public bool allDay { get; set; }
            public string className { get; set; }
        }

        private IList<CalendarDTO> TaskFeltoltes(List<UjBejelentes> bejelentList)
        {
            IList<CalendarDTO> naptarList = new List<CalendarDTO>();
            foreach (UjBejelentes item in bejelentList)
            {
                string jovahagyva = "#01DF3A";
                string rogzitve = "#ee9629";
                string elutasitva = "#ee2121";
                string torles = "#813a4c";
                string kivalasztott = "";

                if (item.TorlesStatus == TorlesStatus.BejelentettKerelem.ToString())
                    kivalasztott = torles;
                else if (item.JovaStatus == JovaHagyasStatus.Rogzitve.ToDisplayString())
                    kivalasztott = rogzitve;
                else if (item.JovaStatus == JovaHagyasStatus.Jovahagyva.ToDisplayString())
                    kivalasztott = jovahagyva;
                else if (item.JovaStatus == JovaHagyasStatus.Elutasitva.ToDisplayString())
                    kivalasztott = elutasitva;
                else
                    kivalasztott = "#989090";

                naptarList.Add(new CalendarDTO
                    {
                        id = item.ID,
                        title = item.JogcimNev,
                        start = item.KezdetiDatum.ToString("s"),
                        end = item.VegeDatum.ToString("s"),
                        color = kivalasztott,
                        allDay = false,
                        className = string.Empty
                    });
            }
            return naptarList;
        }
    }
}

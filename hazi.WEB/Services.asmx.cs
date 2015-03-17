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

            List<UjBejelentes> bejelentesek = Bejelentes.GetIdoBejelentesek(
                RoleActions.GetRole(User.Identity.Name), User.Identity.Name, segedStart, segedEnd);

            IList<CalendarDTO> tasksList = TaskFeltoltes(bejelentesek);

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
             new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(tasksList);
            return sJSON;         
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
                naptarList.Add(new CalendarDTO
                    {
                        id = item.ID,
                        title = item.JogcimNev,
                        start = item.KezdetiDatum.ToString("s"),
                        end = item.VegeDatum.ToString("s"),
                        color = "#01DF3A",
                        allDay = false,
                        className = string.Empty
                    });
            }
            return naptarList;
        }
    }
}

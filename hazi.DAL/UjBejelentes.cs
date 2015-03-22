using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.DAL
{
    public class UjBejelentes
    {
        public int ID { get; set; }
        public System.DateTime KezdetiDatum { get; set; }
        public System.DateTime VegeDatum { get; set; }
        public int JogcimID { get; set; }
        public string UserName { get; set; }
        public string LastEdit { get; set; }
        public System.DateTime LastEditTime { get; set; }
        public string JogcimNev { get; set; }
        public string TorlesStatus { get; set; }
        public string JovaStatus { get; set; }
        public string Statusz { get; set; }
        public Ido Ido { get; set; }
        public double OsszIdo { get; set; }
        public int HanyadikHet { get; set; }
        public double OsszRogzitet { get; set; }
        public double OsszJovahagyott { get; set; }
        public double OsszElutasitott { get; set; }

        [NotMapped]
        public List<ListItem> JovaStatuszList { get; set; }
        [NotMapped]
        public List<ListItem> TorlesStatuszList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string JogcimNev { get; set; }
    }
}

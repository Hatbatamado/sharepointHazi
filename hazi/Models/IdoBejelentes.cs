using System;
using System.ComponentModel.DataAnnotations;

namespace hazi.Models
{
    public class IdoBejelentes
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        public string KezdetiDatum { get; set; }
        public string VegeDatum { get; set; }
        public int? JogcimID { get; set; }
        public virtual Jogcim Jogcim { get; set; }
    }
}
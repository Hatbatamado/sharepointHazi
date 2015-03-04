using System;
using System.ComponentModel.DataAnnotations;

namespace hazi.Models
{
    public class IdoBejelentes
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        public DateTime KezdetiDatum { get; set; }
        public DateTime VegeDatum { get; set; }
        public int? JogcimID { get; set; }
        public virtual Jogcim Jogcim { get; set; }
    }
}
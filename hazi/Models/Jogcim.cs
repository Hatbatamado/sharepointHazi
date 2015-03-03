using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hazi.Models
{
    public class Jogcim
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        public string Cim { get; set; }
        public virtual ICollection<IdoBejelentes> Bejelentesek { get; set; }
    }
}
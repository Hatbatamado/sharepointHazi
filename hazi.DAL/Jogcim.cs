//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace hazi.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Jogcim
    {
        public Jogcim()
        {
            this.IdoBejelentes = new HashSet<IdoBejelentes>();
        }
    
        public int ID { get; set; }
        public string Cim { get; set; }
        public Nullable<bool> Inaktiv { get; set; }
        public string Szin { get; set; }
    
        public virtual ICollection<IdoBejelentes> IdoBejelentes { get; set; }
    }
}

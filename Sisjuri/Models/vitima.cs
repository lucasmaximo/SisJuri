//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sisjuri.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vitima
    {
        public int id_vitima { get; set; }
        public string nome_vitima { get; set; }
        public int fk_id_processo { get; set; }
    
        public virtual processo processo { get; set; }
    }
}

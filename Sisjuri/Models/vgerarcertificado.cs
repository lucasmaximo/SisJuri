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
    
    public partial class vgerarcertificado
    {
        public int id_juri { get; set; }
        public int id_presenca { get; set; }
        public int id_usuario { get; set; }
        public string nome_aluno { get; set; }
        public string cpf_usuario { get; set; }
        public string nome_funcao { get; set; }
        public string nome_juri { get; set; }
        public string local_juri { get; set; }
        public System.DateTime data_hora_juri { get; set; }
        public string horas_certif { get; set; }
    }
}

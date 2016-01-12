using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sisjuri.Models
{
    public partial class PerfilModel
    {
        [Display(Name = "Perfil")]
        public int Id_Perfil { get; set; }

    }
}
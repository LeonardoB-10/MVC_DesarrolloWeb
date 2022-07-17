using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCCrud.Models.ViewModels
{
    public class ClienteViewModel
    {

        public int id_cliente { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]

        public string nombre_cli { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Cédula")]
        public string cedula_cli { get; set; }
        [Required]
        [StringLength(80)]
        [EmailAddress]
        [Display(Name = "Correo electronico")]
        public string correo_cli { get; set; }

        [Required]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaNacimiento { get; set; }

        [Display (Name = "Foto")]
        public byte[] foto { get; set; }
    }
}
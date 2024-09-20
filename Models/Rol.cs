using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Models
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }
        [Required(ErrorMessage ="El campo Nombre es obligatorio!")]
        [StringLength(50)]
        public string? Nombre { get; set; }//El signo de interrogacion significa que la variable acepta valores nulos.
    }
}

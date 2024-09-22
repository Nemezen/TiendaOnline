using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Direccion
    {
        [Key]
        public int DireccionId { get; set; }
        [Required(ErrorMessage = "El campo Direccion es obligatorio!")]
        [StringLength(50)]
        public required string Domicilio { get; set; }
        public required string Estado { get; set; }
        [Required]
        [StringLength(10)]
        public required string CodigoPostal { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        
    }
}

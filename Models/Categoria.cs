using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Required(ErrorMessage ="El campo Nombre es obligatorio!")]
        [StringLength(50)]
        public required string Nombre { get; set;}
        [Required]
        [StringLength(500)]
        public required string Descripcion {  get; set;}
        
        public required ICollection<Producto> Productos { get; set; }
    }
}
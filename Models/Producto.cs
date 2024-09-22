using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }
        [Required]
        [StringLength(50)]
        public required string Codigo { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio!")]
        [StringLength(255)]
        public required string Nombre { get; set; }
        [Required]
        [StringLength(255)]
        public required string Modelo { get; set; }
        [Required]
        [StringLength(500)]
        public required string Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        [StringLength(255)]
        public required string Imagen { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public required Categoria Categoria { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        [StringLength(100)]
        public required string Marca {  get; set; }
        [Required]
        public bool Activo { get; set; }
        [Required]
        [StringLength(100)]
        public required ICollection<Detalle_Pedido> Detalles_Pedido { get; set; }
    }
}

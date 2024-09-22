using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public required Usuario Usuario { get; set; }
        [Required]
        public DateTime Fecha {  get; set; }    
        [Required]
        public required string Estado { get; set; }
        [Required]
        public int DireccionIdSeleccionada { get; set; }
        [Required]
        public required Direccion Direccion { get; set; }
        [Required]
        public decimal Total { get; set; }
        public ICollection<Detalle_Pedido>? Detalles_Pedido { get; set; }
    }
}

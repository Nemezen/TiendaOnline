using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Detalle_Pedido
    {
        [Key]
        public int DetallePedidoId { get; set; }
        [Required]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public required Pedido Pedido { get; set; }
        [Required]
        public int ProductoId { get; set; } 
        [ForeignKey("ProductoId")]
        public required Producto Producto { get; set; }
        public int Cantidad { get; set; }   
        [Required]
        public decimal Precio { get; set; }

    }
}
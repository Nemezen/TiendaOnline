﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }
        [Required]
        [StringLength(100)]
        public string? Codigo { get; set; }
        [Required]
        [StringLength(255)]
        public string? Nombre { get; set; }
        [Required]
        [StringLength(255)]
        public string? Modelo { get; set; }
        [Required]
        [StringLength(1000)]
        public string? Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        [StringLength(255)]
        public string? Imagen { get; set; }
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        [StringLength(100)]
        [Required]
        public bool Activo { get; set; }
        [Required]
        [StringLength(100)]
        public ICollection<Detalle_Pedido>? Detalle_Pedido { get; set; }
    }
}
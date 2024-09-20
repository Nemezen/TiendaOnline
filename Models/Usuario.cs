using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class Usuario
    {
        public Usuario() 
        { 
            Pedidos = new List<Pedido>();            
        }
        [Key]
        public int UsuarioId { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }
        [Required]
        [StringLength(15)]
        public string? Telefono{ get; set; }
        [Required]
        [StringLength(50)]
        public string? NombreUsuario{ get; set; }
        [Required]
        [StringLength(255)]
        public string? Contrasenia { get; set; }
        [Required]
        [StringLength(255)]
        public string? Correo { get; set; }
        [Required]
        [StringLength(50)]
        public string? Direccion { get; set; }
        [Required]
        [StringLength(50)]
        public string? Estado { get; set; }
        [Required]
        [StringLength(10)]
        public string? CodigoPostal { get; set; }
        [Required]
        public int RolId { get; set; }
        
        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        [InverseProperty("Usuario")]
        public ICollection<Direccion>? Direcciones   { get; set; }   
    }
}

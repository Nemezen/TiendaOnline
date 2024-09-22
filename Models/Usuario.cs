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
        [Required(ErrorMessage = "El campo Nombre es obligatorio!")]
        [StringLength(50)]
        public required string Nombre { get; set; }
        [Required]
        [StringLength(15)]
        public required string Telefono{ get; set; }
        [Required]
        [StringLength(50)]
        public required string NombreUsuario { get; set; }
        [Required]
        [StringLength(255)]
        public required string Contrasenia { get; set; }
        [Required]
        [StringLength(255)]
        public required string Correo { get; set; }
        [Required]
        [StringLength(50)]
        public required string Domicilio { get; set; }
        [Required]
        [StringLength(50)]
        public required string Estado { get; set; }
        [Required]
        [StringLength(10)]
        public required string CodigoPostal { get; set; }
        [Required]
        public int RolId { get; set; }
        
        [ForeignKey("RolId")]
        public required Rol Rol { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        [InverseProperty("Usuario")]
        public required ICollection<Direccion> Direcciones   { get; set; }   
    }
}

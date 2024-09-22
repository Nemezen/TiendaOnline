using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using TiendaOnline.Models;

namespace TiendaOnline.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Detalle_Pedido> DetallePedidos { get; set; }
        public DbSet<Categoria> Categorias  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pedidos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);//Esta propiedad elimina los pedidos al eliminar al usuario

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Detalles_Pedido)//Referencia a ICollection de Producto
                .WithOne(dp => dp.Producto)
                .HasForeignKey(dp => dp.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pedido>()
              .HasMany(p => p.Detalles_Pedido)
              .WithOne(dp => dp.Pedido)
              .HasForeignKey(dp => dp.PedidoId )
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pedido>()
              .Ignore(p => p.Direccion);

            modelBuilder.Entity<Categoria>()
                .HasMany(c=>c.Productos)
                .WithOne(p=>p.Categoria)
                .HasForeignKey(p=>p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);//Al eliminar una categoria, no se eliminan los productos asociados a categoria
   
        }
        public DbSet<TiendaOnline.Models.Rol> Rol { get; set; } = default!;
    }
}

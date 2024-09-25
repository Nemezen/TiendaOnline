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
        public DbSet<Rol> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Usuario -> Pedidos
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pedidos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);//Esta propiedad elimina los pedidos al eliminar al usuario
            //Configura la precision y escala de la propiedad balance para la tabla Usuarios
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18,2)");

            // Relación Producto -> Detalles_Pedido
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.DetallesPedido)//Referencia a ICollection de Producto
                .WithOne(dp => dp.Producto)
                .HasForeignKey(dp => dp.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            // Relación Pedido -> Detalles_Pedido
            modelBuilder.Entity<Pedido>()
              .HasMany(p => p.DetallesPedido)
              .WithOne(dp => dp.Pedido)
              .HasForeignKey(dp => dp.PedidoId)
              .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Pedido>()
                .Ignore(p => p.Direccion);
            modelBuilder.Entity<Pedido>()
                .Property(pd => pd.Total)
                .HasColumnType("decimal(18,2)");

            // Relación Categoria -> Productos
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);//Al eliminar una categoria, no se eliminan los productos asociados a categoria

            //Configura la precision y escala de la propiedad precio para la tabla Detalle_pedido, al igual que todas estas configuraciones para el resto de tablas.
            //Su principal funcion es evitar el truncamiento de los valores o que se redondee de manera inesperada.
            modelBuilder.Entity <Detalle_Pedido>()
                .Property(dp => dp.Precio)
                .HasColumnType("decimal(18,2)");


            
        }

    }
}

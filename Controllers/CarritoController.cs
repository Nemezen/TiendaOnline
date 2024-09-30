using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Models.ViewModels;

namespace TiendaOnline.Controllers
{
    public class CarritoController : BaseController
    {
        public CarritoController(ApplicationDbContext context) : base(context)
        {

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var carritoViewModel = await GetCarritoViewModelAsync();

            foreach (var item in carritoViewModel.Items)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    item.Producto = producto;
                    if (!producto.Activo)
                        item.Cantidad = 0;
                    else
                        item.Cantidad = Math.Min(item.Cantidad, producto.Stock);
                    if (item.Cantidad == 0)
                        item.Cantidad = 0;
                }
                else
                    item.Cantidad = 0;
            }
            //Si es usuario esta autenticado, se asigna a la variable usuarioId su identificador, en caso contrario, se asigna 0
            var usuarioId = User.Identity?.IsAuthenticated == true ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : 0;
            //Esto asgina a una variable global una lista de direcciones que coincida con el usuarioId que tenemos almacenado, en caso contrario se crea una lista vacia de direcciones.
            var direcciones = User.Identity?.IsAuthenticated == true ? _context.Direcciones.Where(d => d.UsuarioId == usuarioId).ToList() : new List<Direccion>();

            var ProcederConCompraViewModel = new ProcederConCompraViewModel
            {
                Carrito = carritoViewModel,
                Direcciones = direcciones,
            };
            return View(ProcederConCompraViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int id, int cantidad)
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var carritoItem= carritoViewModel.Items.FirstOrDefault(i=>i.ProductoId==id);
            if (carritoItem != null)
            {
                carritoItem.Cantidad = cantidad;
                var producto = await _context.Productos.FindAsync(id);
                if(producto != null && producto.Activo && producto.Stock>0) 
                    carritoItem.Cantidad= Math.Min(cantidad, producto.Stock);
                await UpdateCarritoViewModelAsync(carritoViewModel);
                
            }
            return RedirectToAction("Index", "Carrito");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var carritoItem = carritoViewModel.Items.FirstOrDefault(i => i.ProductoId == id);

            if (carritoItem != null)
            {
                carritoViewModel.Items.Remove(carritoItem);

                await UpdateCarritoViewModelAsync(carritoViewModel);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> VaciarCarrito()
        {
            await RemoveCarritoViewModelAsync();
            return RedirectToAction("Index");
        }

        private async Task RemoveCarritoViewModelAsync()
        {
            await Task.Run(() => Response.Cookies.Delete("carrito"));
        }
    }
}

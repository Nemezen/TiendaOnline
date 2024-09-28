using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System.Data.Common;
using System.Diagnostics;
using System.Text.Json;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Models.ViewModels;

namespace TiendaOnline.Controllers
{
    public class BaseController : Controller
    {
        public readonly ApplicationDbContext _context;
        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public override ViewResult View(string? viewName, object? model)
        {
            ViewBag.NumeroProductos = GetCarritoCount();
            return base.View(viewName, model);
        }

        private int GetCarritoCount()
        {
            int count = 0;
            string? carritoJson = Request.Cookies["carrito"];

            if (!String.IsNullOrEmpty(carritoJson))
            {
                //La lista se carga en este modelo porque al insertar un producto al carrito, no necesitamos todos los datos del producto dentro del carrito
                //unicamente necesitamos el identificador y la cantidad, y con eso podemos obtener todas las caracteristicas del producto en la bd.
                var carrito = JsonSerializer.Deserialize<List<ProductoIdAndCantidad>>(carritoJson);
                if (carrito != null)
                {
                    count = carrito.Count;
                }
            }

            return count;
        }

        //hacer json
        public async Task<CarritoViewModel> AgregarProductoAlCarrito(int productoId, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(productoId);

            if (producto != null)
            {
                var carritoViewModel = await GetCarritoViewModelAsync();

                var carritoItem = carritoViewModel.Items.FirstOrDefault(item => item.ProductoId == productoId);
                if (carritoItem != null)
                    carritoItem.Cantidad += cantidad;
                else
                {
                    carritoViewModel.Items.Add(
                        new CarritoItemViewModel
                        {
                            ProductoId = producto.ProductoId,
                            Nombre = producto.Nombre,
                            Precio = producto.Precio,
                            Cantidad = cantidad
                        }
                    );
                }
                carritoViewModel.Total = carritoViewModel.Items.Sum(item => item.Cantidad * item.Precio);
                await UpdateCarritoViewModelAsync(carritoViewModel);
                return carritoViewModel;
            }
            return new CarritoViewModel();
        }

        //Devuelve una lista del ID del producto y sus cantidad para actualizar el carrito que esta en la cookie (json)
        private async Task UpdateCarritoViewModelAsync(CarritoViewModel carritoViewModel)
        {
            var productoIds = carritoViewModel.Items.Select(
                item=>new ProductoIdAndCantidad
                {
                    ProductoId=item.ProductoId,
                    Cantidad=item.Cantidad,
                }
                ).ToList();
            var carritoJson = await Task.Run(() => JsonSerializer.Serialize(productoIds));
            Response.Cookies.Append("carrito",carritoJson, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1)});
            //Despues de serializar el objeto, se crea la cookie "carrito" y se configura la cookie para que expire en un dia

        }

        private async Task<CarritoViewModel> GetCarritoViewModelAsync()
        {
            var carritoJson = Request.Cookies["carrito"];
            if (string.IsNullOrEmpty(carritoJson))
                return new CarritoViewModel();
            var productoIdsAndCantidades = JsonSerializer.Deserialize<List<ProductoIdAndCantidad>>(carritoJson);
            var carritoViewModel = new CarritoViewModel();
            if (productoIdsAndCantidades!=null)
            {
                foreach(var item in productoIdsAndCantidades)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if (producto != null)
                    {
                        carritoViewModel.Items.Add(
                            new CarritoItemViewModel
                            {
                                ProductoId = producto.ProductoId,
                                Nombre = producto.Nombre,
                                Precio = producto.Precio,
                                Cantidad = item.Cantidad
                            });
                    }
                }
            }
            carritoViewModel.Total = carritoViewModel.Items.Sum(item => item.Subtotal);
            return carritoViewModel;
        }

        //Maneja errores generales
        protected IActionResult HandleError(Exception error)
        {
            return View(
                "Error ",
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
                );
        }

        //Maneja errores en la base de datos
        protected IActionResult HandleDbError(DbException dbEx)
        {
            var ViewModel = new DbErrorViewModel
            {
                ErrorMessage = "Error de base de datos",
                ErrorDetail = dbEx.Message
            };
            return View("DbError", ViewModel);
        }

        //Maneja errores al actualizar la base de datos.
        protected IActionResult HandleDbError(DbUpdateException dbUpdateEx)
        {
            var ViewModel = new DbErrorViewModel
            {
                ErrorMessage = "Error de base de datos",
                ErrorDetail = dbUpdateEx.Message
            };
            return View("DbError", ViewModel);
        }
    }
}

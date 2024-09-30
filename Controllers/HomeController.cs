using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TiendaOnline.Data;
using TiendaOnline.Models;
using TiendaOnline.Services;

namespace TiendaOnline.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IProductoService productoService, ICategoriaService categoriaService) : base(context)
        {

            _logger = logger;
            _productoService = productoService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categorias = await _categoriaService.GetCategorias();
            try
            {
                List<Producto> productosDestacados = await _productoService.GetProductosDestacados();
                return View(productosDestacados);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        public IActionResult DetalleProducto(int id)
        {
            var producto = _productoService.GetProducto(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        public async Task<IActionResult> Productos(int? categoriaId, string? busqueda, int pagina=1)
        {
            try
            {
                int productosPorPagina = 6;
                var model = await _productoService.GetProductosPaginados(categoriaId, busqueda, pagina, productosPorPagina);
                ViewBag.Categorias = await _categoriaService.GetCategorias();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    //se utiliza en esta ocasion partialview para evitar sobrecargar las vistas
                    return PartialView("_ProductosPartial", model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
        //Este metodo lo que hace es agregar un producto sin actualizar la pagina, es decir, con tan solo hacer clic sin importar que estemos en la pagina 1 o 7.
        public async Task<IActionResult> AgregarProducto(int id, int cantidad, int? categoriaId, string? busqueda, int pagina = 1)
        {
            var carritoViewModel = await AgregarProductoAlCarrito(id, cantidad);
            if (carritoViewModel != null)
            {
                return RedirectToAction(
                    "Productos",
                    new
                    {
                        id,
                        categoriaId,
                        busqueda,
                        pagina
                    }
                );
            }
            else
                return NotFound();
        }
        public async Task<IActionResult> AgregarProductoIndex(int id, int cantidad, int? categoriaId, string? busqueda, int pagina = 1)
        {
            var carritoViewModel = await AgregarProductoAlCarrito(id, cantidad);
            if (carritoViewModel != null)
            {
                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }
        public async Task<IActionResult> AgregarProductoDetalle(int id, int cantidad, int? categoriaId, string? busqueda, int pagina = 1)
        {
            var carritoViewModel = await AgregarProductoAlCarrito(id, cantidad);
            if (carritoViewModel != null)
            {
                return RedirectToAction("DetalleProducto", new { id });
            }
            else
                return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
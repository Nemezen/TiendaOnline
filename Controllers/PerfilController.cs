using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class PerfilController : BaseController
    {
        public PerfilController(ApplicationDbContext context) : base(context)
        {

        }
        //Metodo para cargar informacion del usuario y sus direcciones
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Direcciones)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();
            return View(usuario);
        }

        public IActionResult AgregarDireccion(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDireccion(Direccion direccion, int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == id);
                if (usuario != null)
                    direccion.Usuario = usuario;

                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id });
            }
            catch (System.Exception)
            {
                return View(direccion);
            }
        }
        public IActionResult RecargarBalance(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RecargarBalance(int UsuarioId, decimal balance)
        {
            ViewBag.id = UsuarioId;
            try
            {
                var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == UsuarioId);
                if (existingUser == null)
                    return NotFound();

                // Sumar el balance nuevo al existente
                ViewBag.balance = existingUser.Balance;
                existingUser.Balance += balance;
                _context.Update(existingUser);
                await _context.SaveChangesAsync();

                // Redireccionar de nuevo a la vista de detalles
                return RedirectToAction("Details", new { id = UsuarioId });
            }
            catch
            {
                return RedirectToAction("Details", new { id = UsuarioId });
            }
        }
    }
}

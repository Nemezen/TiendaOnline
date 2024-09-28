using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TiendaOnline.Data;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class UsuariosController : BaseController
    {
        public UsuariosController(ApplicationDbContext context) : base(context)
        {
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Usuarios.Include(u => u.Rol);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(String Jsonusuario)
        {
            Console.WriteLine(Jsonusuario.ToString());
            if (string.IsNullOrEmpty(Jsonusuario))
            {
                ModelState.AddModelError(string.Empty, "Los datos del usuario no pueden estar vacíos.");
                return View();
            }
            var usuario = JsonConvert.DeserializeObject<Usuario>(Jsonusuario);

            if (usuario != null)
            {
                var rol = await _context.Roles.FirstOrDefaultAsync(c => c.RolId == usuario.RolId);
                if (rol == null)
                {
                    return BadRequest("Rol no encontrado.");
                }
                usuario.Direcciones = new List<Direccion> { 
                    new Direccion{
                        Address = usuario.Direccion,
                        Estado = usuario.Estado,
                        CodigoPostal = usuario.CodigoPostal,
                    }
                };
                usuario.Rol = rol;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error al procesar los datos del producto.");
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Telefono,NombreUsuario,Contrasenia,Correo,Direccion,Estado,CodigoPostal,RolId,Balance")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }
            var rol = await _context.Roles.FirstOrDefaultAsync(c => c.RolId == usuario.RolId);
            if (rol != null)//Comprueba que la variable rol no sea null, esto garantiza que no el campo por seleccionar Rol sin seleccionar y a su vez, que no quede vacio el campo.
            {
                usuario.Rol = rol;

                var existingUser = await _context.Usuarios//Se declara una variable existingUser, su funcion es asignar un ICollection de usuarios donde se guarda el ID y las direcciones del usuario
                    .Include(u => u.Direcciones)
                    .FirstOrDefaultAsync(u => u.UsuarioId == id);
                if (existingUser != null)//Mientras exista el id del usuario dentro de la base de datos
                {
                    if (existingUser.Direcciones.Count > 0)//En caso de que exista una direccion, se agrega la nueva direccion al ICollection
                    {
                        var direccion = existingUser.Direcciones.First();
                        direccion.Address = usuario.Direccion;
                        direccion.Estado = usuario.Estado;
                        direccion.CodigoPostal = usuario.CodigoPostal;
                    }
                    else//En caso de que no exista una direccion para el usuario a editar, se crea una nueva direccion
                    {
                        existingUser.Direcciones = new List<Direccion>
                        {
                            new Direccion
                            {
                                Address = usuario.Direccion,
                                Estado = usuario.Estado,
                                CodigoPostal = usuario.CodigoPostal,
                            }
                        };
                    }
                    try
                    {
                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!UsuarioExists(usuario.UsuarioId))
                        {
                            ModelState.AddModelError(string.Empty, "No se pudieron guardar los cambios " + ex);
                            throw;
                        }       
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["RolId"] = new SelectList(_context.Roles, "RolId", "Nombre", usuario.RolId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}

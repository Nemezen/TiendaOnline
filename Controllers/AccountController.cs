using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Claims;
using TiendaOnline.Data;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ApplicationDbContext context) : base(context)
        {

        }
        //Falta generar vista
        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> Register(Usuario usuario)
        //{
        //    try
        //    {
        //        if (usuario != null)
        //        {
        //            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario))
        //            {
        //                ModelState.AddModelError(nameof(usuario.NombreUsuario), "El nombre de usuario ya esta en uso.");
        //                return View(usuario);
        //            }
        //            var clienteRol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");

        //            if (clienteRol != null)
        //            {
        //                usuario.RolId = clienteRol.RolId;
        //            }
        //            usuario.Direcciones = new List<Direccion> {
        //                new Direccion{
        //                    Address = usuario.Direccion,
        //                    Estado = usuario.Estado,
        //                    CodigoPostal = usuario.CodigoPostal,
        //                }
        //            };
        //            _context.Usuarios.Add(usuario);
        //            await _context.SaveChangesAsync();

        //            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        //            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreUsuario));
        //            identity.AddClaim(new Claim(ClaimTypes.Role, "Cliente"));

        //            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        //            return RedirectToAction("Index", "Home");
        //        }
        //        return View(usuario);
        //    }

        //    catch (DbException ex)
        //    {
        //        return HandleDbError(ex);
        //    }
        //}
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Administrador") || User.IsInRole("Staff"))
                    return RedirectToAction("Index", "Dashboard");
                else
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Contrasenia == password);
                if (user != null)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, username));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UsuarioId.ToString()));

                    var rol = await _context.Roles.FirstOrDefaultAsync(r => r.RolId == user.RolId);
                    if (rol != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, rol.Nombre));
                    }

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Credenciales Invalidas.");
                return RedirectToAction("AccessDenied");
            }
            catch (Exception ex) 
            {
                return HandleError(ex);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

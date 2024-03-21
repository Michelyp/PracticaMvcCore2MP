using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMVC2.Models;
using PracticaMVC2.Repositories;
using System.Security.Claims;

namespace PracticaMVC2.Controllers
{
    public class ManagedController : Controller
    {
        RepositoryTienda repo;
        public ManagedController(RepositoryTienda repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {

            Usuario usuario = await this.repo.Login(email, pass);
            if (usuario != null)
            {

                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);
                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                Claim claimRol = new Claim(ClaimTypes.Role, "Usuario");
                Claim clainSalario = new Claim("Apellidos", usuario.Apellidos.ToString());
                Claim clainEmail = new Claim("Email", usuario.Apellidos.ToString());
                Claim clainFoto = new Claim("Foto", usuario.Apellidos.ToString());
                identity.AddClaim(claimName);
                identity.AddClaim(claimId);
                identity.AddClaim(claimRol);
                identity.AddClaim(clainSalario);
                identity.AddClaim(clainEmail);
                identity.AddClaim(clainFoto);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                return RedirectToAction(action, controller);
            }else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

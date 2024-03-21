using Microsoft.AspNetCore.Mvc;
using PracticaMVC2.Filters;
using PracticaMVC2.Models;
using PracticaMVC2.Repositories;
using System.Security.Claims;

namespace PracticaMVC2.Controllers
{
    public class UsuarioController : Controller
    {
        private RepositoryTienda repo;
        public UsuarioController(RepositoryTienda repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AuthorizeTienda]
        public async Task<IActionResult> Perfil()
        {
            string data =
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Usuario user = await this.repo.FindUsuarioAsync(int.Parse(data)); 
            return View(user);
        }
    }
}

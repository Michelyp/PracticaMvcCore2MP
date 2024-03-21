using Microsoft.AspNetCore.Mvc;
using PracticaMVC2.Extensions;
using PracticaMVC2.Filters;
using PracticaMVC2.Models;
using PracticaMVC2.Repositories;
using System.Collections.Generic;
using System.Security.Claims;

namespace PracticaMVC2.Controllers
{
    public class LibrosController : Controller
    {
        RepositoryTienda repo;
        public LibrosController(RepositoryTienda repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Libros()
        {
            List<Libro> libros = await this.repo.GetLibroAsync();
            return View(libros);
        }
        public async Task<IActionResult> FiltrarLibros(int idGenero)
        {
           List<Libro> libros = await this.repo.FindLibroGenero(idGenero);
            return View(libros);
        }
        public async Task<IActionResult> Details(int idlibro)
        {
            Libro libro = await this.repo.FindLibro(idlibro);
            return View(libro);
        }
        public IActionResult Carrito(int idLibro)
        {
            List<int> libros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            if(libros == null)
            {
                List<int> listalibros = new List<int> { idLibro };
                HttpContext.Session.SetObject("IDSLIBROS", listalibros);
            }
            else
            {
                if (!libros.Contains(idLibro)){
                    libros.Add(idLibro);
                    HttpContext.Session.SetObject("IDSLIBROS", libros);
                }
            }
            return RedirectToAction("Libros");
        }
        public async Task<IActionResult> CarritoDevolver()
        {

            List<int> libros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            List<Libro> libr = new List<Libro>();
            if (libros != null)
            {
                foreach(int ids in libros)
                {
                    Libro libro = await this.repo.FindLibro(ids);
                    libr.Add(libro);
                }
            }

            return View(libr);
        }
        [AuthorizeTienda]
        public async Task<IActionResult> Comprando()
        {
            string idusuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int iduser = int.Parse(idusuario);
            List<int> libros= HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            if (libros != null)
            {
                foreach(int idlibro in libros)
                {
                    await this.repo.CompraPedido(idlibro, iduser);
                }
                HttpContext.Session.Remove("IDSLIBROS");
            }
            List<VistaPedido> pedidos = await this.repo.GetVistaPedido(iduser);
            return View(pedidos);
        }
    }
}

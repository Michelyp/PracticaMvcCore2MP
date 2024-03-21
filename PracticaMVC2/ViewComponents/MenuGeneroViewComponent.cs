using Microsoft.AspNetCore.Mvc;
using PracticaMVC2.Models;
using PracticaMVC2.Repositories;

namespace PracticaMVC2.ViewComponents
{
    public class MenuGeneroViewComponent:ViewComponent
    {
        private RepositoryTienda repo;

        public MenuGeneroViewComponent(RepositoryTienda repo)
        {
            this.repo = repo;
        }

        //PODRIAMOS TENER TODOS LOS METODOS QUE DESEEMOS EN LA CLASE
        //ES OBLIGATORIO TENER UN METODO LLAMADO InvokeAsync QUE 
        //SERA EL QUE ADMINISTRE EL DIBUJO CON EL MODEL
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGeneros();
            return View(generos);
        }

    }
}

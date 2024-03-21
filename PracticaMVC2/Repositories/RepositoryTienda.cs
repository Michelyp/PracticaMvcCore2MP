using Microsoft.EntityFrameworkCore;
using PracticaMVC2.Data;
using PracticaMVC2.Models;

namespace PracticaMVC2.Repositories
{
    public class RepositoryTienda
    {
        public TiendaContext context;
        public RepositoryTienda(TiendaContext context)
        {
            this.context = context;
        }
        public async Task<Usuario> Login(string email, string pass)
        {
            return await this.context.Usuarios.Where(z => z.Email == email && z.Pass == pass).FirstOrDefaultAsync(); 
        }
        public async Task<Usuario> FindUsuarioAsync(int iduser)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(Z => Z.IdUsuario == iduser);
        }
        public async Task<List<Genero>> GetGeneros() {
            return await this.context.Generos.ToListAsync();
        }
        public async Task<List<Libro>> GetLibroAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<Libro> FindLibro(int idlibro)
        {
            return await this.context.Libros.FirstOrDefaultAsync(z=>z.IdLibro == idlibro);
        }
        public async Task<List<Libro>> FindLibroGenero(int idGenero)
        {
            List<Libro> libros= await this.context.Libros.Where(z => z.IdGenero == idGenero).ToListAsync();
            return libros;
        }
        public async Task<Libro> FindLibroCompra(int idlibro)
        {
            return await this.context.Libros.Where(z => z.IdLibro == idlibro).FirstOrDefaultAsync();
        }
        public async Task<List<Libro>> GetLibrosCompras(List<int> ids)
        {
            var consulta = from datos in this.context.Libros where ids.Contains(datos.IdLibro) select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            return await consulta.ToListAsync();
        }
        public async Task<List<VistaPedido>> GetVistaPedido(int iduser)
        {
            List<VistaPedido> pedidos = await this.context.VistaPedidos.Where(z => z.IdUsuario == iduser).ToListAsync();
            return pedidos;
        }


        private async Task<int> GetMaxIdPedidoAsync()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Pedidos.MaxAsync(Z => Z.IdPedido) + 1;
            }
        }
        private async Task<int> GetMaxIdFacturaAsync()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Pedidos.MaxAsync(Z => Z.IdFactura) + 1;
            }
        }
        public async Task CompraPedido(int idlibro, int iduser)
        {
            DateTime dat = DateTime.Now;
            Pedido pedido = new Pedido()
            {
                IdPedido = await this.GetMaxIdPedidoAsync(),
                IdFactura = await this.GetMaxIdFacturaAsync(),
                Fecha = dat,
                IdLibro = idlibro,
                IdUsuario = iduser,
                Cantidad = 1
            };
            this.context.Pedidos.Add(pedido);
            await this.context.SaveChangesAsync();
             
        }

        public async Task<List<VistaPedido>> GetVistaPedidosAsync(int iduser)
        {
            return await this.context.VistaPedidos.Where(z => z.IdUsuario == iduser).ToListAsync();
        }
     


    }
}

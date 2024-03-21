using Microsoft.EntityFrameworkCore;
using PracticaMVC2.Models;

namespace PracticaMVC2.Data
{
    public class TiendaContext: DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options)
            : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<VistaPedido> VistaPedidos { get; set; }
    }
}

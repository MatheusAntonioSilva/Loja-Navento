using Microsoft.EntityFrameworkCore;
using Navento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Navento.Data
{
    public class NaventoContext : DbContext
    {
        public NaventoContext()
        {
        }

        public NaventoContext(DbContextOptions<NaventoContext> options): base(options)
        { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidosItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PedidoItem>().HasKey(t => new { t.PedidoId, t.ProdutoId });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Navento.Data;
using Navento.Models;

namespace Navento.Controllers
{
   
    public class PedidoesController : Controller
    {
        private readonly NaventoContext _context;

        static List<Produto> listaDeProdutos = new List<Produto>
        {
        
        };
        public PedidoesController(NaventoContext context)
        {
            _context = context;
        }

        // GET: Pedidoes
        public async Task<IActionResult> Index()
        {
            var naventoContext = _context.Pedidos.Include(p => p.Usuario);
            return View(await naventoContext.ToListAsync());
        }

        // GET: Pedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedidoes/Create
        public IActionResult Create()
        {
            ViewBag.listaDeProdutos = listaDeProdutos;
            ViewBag.UsuarioId = new SelectList(_context.Usuarios, "Id", "Id");
            ViewBag.UsuarioName = new SelectList(_context.Usuarios, "Name", "Name");
            return View();
        }

        // POST: Pedidoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,UsuarioId")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();

                  foreach(var pi in listaDeProdutos) {
                    var pedidoItem = new PedidoItem {
                        PedidoId = pedido.Id, ProdutoId = pi.Id
                    };
                    _context.PedidosItens.Add(pedidoItem);
                    _context.SaveChanges();
                  }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", pedido.UsuarioId);
            
          
            return View(pedido);
        }
/* 
        [HttpPost("/Pedidoes/Comprar")]
        [ValidateAntiForgeryToken]
        public IActionResult Comprar([Bind("Descricao, PrecoUnitario")] Produto produto)
        {
            try
            {
                listaDeProdutos.Add(produto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        */
        [HttpPost("Pedidoes/comprar")]
        public IActionResult Comprar([Bind("Id, Nome, PrecoUnitario")] Produto produto)
        {
            try
            {
                listaDeProdutos.Add(produto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.SingleOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", pedido.UsuarioId);
            return View(pedido);
        }

        // POST: Pedidoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,UsuarioId")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", pedido.UsuarioId);
            return View(pedido);
        }

        // GET: Pedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.SingleOrDefaultAsync(m => m.Id == id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}

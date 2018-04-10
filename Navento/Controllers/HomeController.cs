using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navento.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Navento.Data;

namespace Navento.Controllers
{
    public class HomeController : Controller
    {
        private readonly NaventoContext _context;

        public HomeController(NaventoContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var pedido_item = new PedidoItem();
            var cont = 0;
            decimal soma = 0;
             foreach(var pi in _context.PedidosItens.ToList()) {
                 cont = cont +1;
                 foreach(var pr in _context.Produtos.ToList()) {
                     if(pi.ProdutoId == pr.Id) {
                         soma = soma + pr.PrecoUnitario;
                     }
                 };
            };
          
            ViewData["contador"] = cont;
            ViewData["preco_total"] = soma;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

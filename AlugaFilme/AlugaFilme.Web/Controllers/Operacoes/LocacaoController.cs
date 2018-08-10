using AlugaFilme.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlugaFilme.Web.Controllers
{
    [Authorize]
    public class LocacaoController : Controller
    {
        private const int _QtdMax = 5;
        DbContexto db;
        public LocacaoController()
        {
            db = new DbContexto();
        }
        public ActionResult Index()
        {
            var lista = db.Locacao.ToList();
            ViewBag.Filmes = db.Filmes.ToList();
            ViewBag.Filtro = new SelectList(new string[] { "CPF", "Nome" });
            ViewBag.ListaTamPag = new SelectList(new int[] { _QtdMax, 10, 15, 20 }, _QtdMax);
            ViewBag.QtdMax = _QtdMax;
            var qtdReg = lista.Count;
            var difQtdPag = (qtdReg % ViewBag.QtdMax) > 0 ? 1 : 0;
            ViewBag.QtdPag = (qtdReg / ViewBag.QtdMax) + difQtdPag;
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarClientes(string tipo, string filtro)
        {
            var filtro_str = "";
            if (tipo == "CPF")
            {
                filtro_str = formata_cpf(filtro);
            }
            else
            {
                filtro_str = filtro.ToLower();
            }
            
            var cliente = db.Clientes.Where(p=>(tipo == "CPF" ? p.NumDocumento : p.Nome).ToLower().Contains(filtro_str));
            var lista = cliente.ToList();
            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvaLocacao(string tipo, string filtro)
        {
            var filtro_str = "";
            if (tipo == "CPF")
            {
                filtro_str = formata_cpf(filtro);
            }
            else
            {
                filtro_str = filtro.ToLower();
            }

            var cliente = db.Clientes.Where(p => (tipo == "CPF" ? p.NumDocumento : p.Nome).ToLower().Contains(filtro_str));
            var lista = cliente.ToList();
            return Json(lista);
        }

        public string formata_cpf(string texto)
        {
            return Convert.ToUInt64(texto).ToString(@"000\.000\.000\-00");
        }
    }
}
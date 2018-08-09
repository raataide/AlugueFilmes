using AlugaFilme.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AlugaFilme.Web.Controllers.Cadastros
{
    [Authorize]
    public class CadClientesController : Controller
    {
        private const int _QtdMax = 5;
        DbContexto db;
        public CadClientesController()
        {
            db = new DbContexto();
        }
        
        public ActionResult Index()
        {
            var lista = db.Clientes.OrderBy(s => s.Nome).ToList();
            ViewBag.ListaTamPag = new SelectList(new int[] { _QtdMax, 10, 15, 20 }, _QtdMax);
            ViewBag.QtdMax = _QtdMax;
            var qtdReg = lista.Count;
            var difQtdPag = (qtdReg % ViewBag.QtdMax) > 0 ? 1 : 0;
            ViewBag.QtdPag = (qtdReg / ViewBag.QtdMax) + difQtdPag;
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ClientesPagina(int pagina, int tamPag, string filtro, string ordenacao)
        {
            var lista = db.Clientes.
                Where(f => f.Nome.ToLower().Contains(filtro != string.Empty ? filtro.ToLower() : f.Nome));
            switch (ordenacao)
            {
                case "nome asc":
                    lista = lista.OrderBy(s => s.Nome);
                    break;
                case "nome desc":
                    lista = lista.OrderByDescending(s => s.Nome);
                    break;
                default:
                    lista = lista.OrderBy(s => s.Nome);
                    break;
            }
            var pos = ((pagina - 1) * tamPag);
            if (tamPag > 0 && pagina > 0)
            {
                lista = lista.Skip(pos)
                    .Take(tamPag);
            }

            return Json(lista.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarClientes(int id)
        {
            return Json(db.Clientes.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarClientes(ClientesModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;
            if (!ModelState.IsValid)
            {
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                resultado = "AVISO";
            }
            else
            {
                var id = 0;
                try
                {
                    if (model.Id > 0)
                    {
                        var Clientes = db.Clientes.Find(model.Id);
                        if (Clientes == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Clientes.Nome = model.Nome;
                        Clientes.Ativo = model.Ativo;
                        Clientes.Cep = model.Cep;
                        Clientes.Cidade = model.Cidade;
                        Clientes.Complemento = model.Complemento;
                        Clientes.Estado = model.Estado;
                        Clientes.Logradouro = model.Logradouro;
                        Clientes.NumDocumento = model.NumDocumento;
                        Clientes.Pais = model.Pais;
                        Clientes.Telefone = model.Telefone;
                        Clientes.Numero = model.Numero;
                        db.SaveChanges();
                        id = Clientes.Id;
                    }
                    else
                    {
                        var NewClientes = new ClientesModel();
                        NewClientes.Nome = model.Nome;
                        NewClientes.Ativo = model.Ativo;
                        NewClientes.Cep = model.Cep;
                        NewClientes.Cidade = model.Cidade;
                        NewClientes.Complemento = model.Complemento;
                        NewClientes.Estado = model.Estado;
                        NewClientes.Logradouro = model.Logradouro;
                        NewClientes.NumDocumento = model.NumDocumento;
                        NewClientes.Pais = model.Pais;
                        NewClientes.Telefone = model.Telefone;
                        NewClientes.Numero = model.Numero;

                        db.Clientes.Add(NewClientes);
                        db.SaveChanges();

                        id = NewClientes.Id;
                        
                    }
                                        
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }


                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirClientes(int id)
        {
            ClientesModel Cli = db.Clientes.Find(id);
            db.Clientes.Remove(Cli);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirVariosClientes(int[] id)
        {
            var ret = false;
            foreach (var item in id)
            {
                ClientesModel Cli = db.Clientes.Find(item);
                db.Clientes.Remove(Cli);
                db.SaveChanges();
                ret = true;
            }

            return Json(ret);
        }
    }
}
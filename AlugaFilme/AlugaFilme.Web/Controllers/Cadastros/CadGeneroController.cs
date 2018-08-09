using AlugaFilme.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AlugaFilme.Web.Controllers.Cadastros
{
    public class CadGeneroController : Controller
    {
        private const int _QtdMax = 5;

        DbContexto db;
        public CadGeneroController()
        {
            db = new DbContexto();
        }
        public ActionResult Index()
        {
            var lista = db.Genero.OrderBy(s=>s.Nome).ToList();
            ViewBag.ListaTamPag = new SelectList(new int[] { _QtdMax, 10, 15, 20 }, _QtdMax);
            ViewBag.QtdMax = _QtdMax;
            var qtdReg = lista.Count;
            var difQtdPag = (qtdReg % ViewBag.QtdMax) > 0 ? 1 : 0;
            ViewBag.QtdPag = (qtdReg / ViewBag.QtdMax) + difQtdPag;
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GeneroPagina(int pagina, int tamPag, string filtro, string ordenacao)
        {
            var lista = db.Genero.
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
        public ActionResult RecuperarGenero(int id)
        {
            var generos = db.Genero.Find(id);            
            return Json(generos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarGenero(GeneroModel model)
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
                        var genero = db.Genero.Find(model.Id);
                        if (genero == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        genero.Nome = model.Nome;
                        genero.Ativo = model.Ativo;
                        db.SaveChanges();
                        id = genero.Id;

                    }
                    else
                    {
                        var NewGenero = new GeneroModel();
                        NewGenero.Nome = model.Nome;
                        NewGenero.Ativo = model.Ativo;
                        NewGenero.DtCriacao = DateTime.Now;

                        db.Genero.Add(NewGenero);
                        db.SaveChanges();
                        id = NewGenero.Id;
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
        public ActionResult ExcluirGenero(int id)
        {
            GeneroModel Gen = db.Genero.Find(id);
            db.Genero.Remove(Gen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirVariosGeneros(int[] id)
        {
            var ret = false;
            foreach (var item in id)
            {
                GeneroModel Gen = db.Genero.Find(item);
                db.Genero.Remove(Gen);
                db.SaveChanges();
                ret = true;
            }

            return Json(ret);
        }
    }
}
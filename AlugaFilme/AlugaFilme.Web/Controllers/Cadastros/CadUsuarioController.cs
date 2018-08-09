using AlugaFilme.Web.Helpers;
using AlugaFilme.Web.Models;
using AlugaFilme.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AlugaFilme.Web.Controllers.Cadastros
{
    [Authorize]
    public class CadUsuarioController : Controller
    {
        private const int _QtdMax = 5;
        private const string _senhaPadrao = "{$127,$188}";

        DbContexto db;
        public CadUsuarioController()
        {
            db = new DbContexto();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.SenhaPadrao = _senhaPadrao;
            var lista = db.Login.OrderBy(s=>s.Nome).ToList();
            ViewBag.ListaTamPag = new SelectList(new int[] { _QtdMax, 10, 15, 20 }, _QtdMax);
            ViewBag.QtdMax = _QtdMax;
            var qtdReg = lista.Count;
            var difQtdPag = (qtdReg % ViewBag.QtdMax) > 0 ? 1 : 0;
            ViewBag.QtdPag = (qtdReg / ViewBag.QtdMax) + difQtdPag;            

            return View(lista.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int tamPag, string filtro, string ordenacao)
        {
            var lista = db.Login.
                Where(f => f.Nome.ToLower().Contains(filtro != string.Empty ? filtro.ToLower() : f.Nome));
            switch (ordenacao)
            {
                case "nome asc":
                    lista = lista.OrderBy(s => s.Nome);
                    break;
                case "nome desc":
                    lista = lista.OrderByDescending(s => s.Nome);
                    break;
                case "login asc":
                    lista = lista.OrderBy(s => s.Login);
                    break;
                case "login desc":
                    lista = lista.OrderByDescending(s => s.Login);
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
        public ActionResult RecuperarUsuario(int id)
        {
            var Login = db.Login.Find(id);
            Login.Senha = _senhaPadrao;
            return Json(Login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel model)
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
                        var Login = db.Login.Find(model.Id);
                        if (Login == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Login.Email = model.Email;
                        Login.Nome = model.Nome;
                        Login.Login = model.Login;

                        if (model.Senha == _senhaPadrao)
                        {
                            Login.Senha = Cripto.HashMD5(model.Senha);
                        }                        
                        db.SaveChanges();

                        id = Login.Id;


                    }
                    else
                    {
                        var NewLogin = new LoginModel();
                        NewLogin.Email = model.Email;
                        NewLogin.Nome = model.Nome;
                        NewLogin.Login = model.Login;

                        NewLogin.Senha = Cripto.HashMD5(model.Senha);
                        
                        db.Login.Add(NewLogin);
                        db.SaveChanges();
                        id = NewLogin.Id;
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
        public ActionResult ExcluirUsuario(int id)
        {
            LoginModel user = db.Login.Find(id);
            db.Login.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirVariosUsuarios(int[] id)
        {
            var ret = false;
            foreach (var item in id)
            {
                LoginModel Login = db.Login.Find(item);
                db.Login.Remove(Login);
                db.SaveChanges();
                ret = true;
            }

            return Json(ret);
        }

    }
}
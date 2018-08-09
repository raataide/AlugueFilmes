using AlugaFilme.Web.Helpers;
using AlugaFilme.Web.Models;
using AlugaFilme.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AlugaFilme.Web.Controllers
{
    
    public class ContaController : Controller
    {
        private const string _senhaPadrao = "{$127,$188}";

        DbContexto db;
        public ContaController()
        {
            db = new DbContexto();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            using (DbContexto _contexto = new DbContexto())
            {
                if (!_contexto.Database.Exists())
                {
                    try
                    {
                        var senha = Cripto.HashMD5("123456");
                        var login = new LoginModel { Login = "Administrador", Nome= "Administrador", Senha = senha, Email = "teste@hotmail.com" };
                        _contexto.Login.Add(login);
                        
                        _contexto.SaveChanges();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

            }            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var usuario = UsuarioModel.ValidarUsuario(login.Usuario, login.Senha);

            if (usuario != null)
            {
                var ticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, usuario.Nome, DateTime.Now, DateTime.Now.AddHours(12), login.LembrarMe, usuario.Id + "|" + "Gerente"));
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);
                Response.Cookies.Add(cookie);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login inválido.");
            }

            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult NovoCadastro(UsuarioModel model)
        {
            return View(model);
        }

        [AllowAnonymous]
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

    }
}
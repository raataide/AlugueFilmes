using AlugaFilme.Web.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AlugaFilme.Web.Controllers.Cadastros
{
    [Authorize]
    public class CadFilmesController : Controller
    {
        private const int _QtdMax = 5;
        
        DbContexto db;
        public CadFilmesController()
        {
            db = new DbContexto();
        }
        public ActionResult Index()
        {
            ViewBag.ListaGeneros = db.Genero.ToList();
            var lista = db.Filmes.OrderBy(s => s.Nome).ToList();
            ViewBag.ListaTamPag = new SelectList(new int[] { _QtdMax, 10, 15, 20 }, _QtdMax);
            ViewBag.QtdMax = _QtdMax;
            var qtdReg = lista.Count;
            var difQtdPag = (qtdReg % ViewBag.QtdMax) > 0 ? 1 : 0;
            ViewBag.QtdPag = (qtdReg / ViewBag.QtdMax) + difQtdPag;
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarFilmes(FilmesModel model, List<int> idGeneros)
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
                        var Filmes = db.Filmes.Find(model.Id);
                        if (Filmes == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Filmes.Nome = model.Nome;
                        Filmes.Ativo = model.Ativo;
                        Filmes.DtCriacao = DateTime.Now;

                        db.SaveChanges();

                        id = Filmes.Id;
                    }
                    else
                    {
                        var NewFilmes = new FilmesModel();
                        NewFilmes.Nome = model.Nome;
                        NewFilmes.Ativo = model.Ativo;
                        NewFilmes.DtCriacao = DateTime.Now;

                        db.Filmes.Add(NewFilmes);
                        db.SaveChanges();

                        id = NewFilmes.Id;
                    }

                    using (var conexao = new SqlConnection())
                    {
                        conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                        conexao.Open();

                        var Filmes_Id = id;

                        using (var transacao = conexao.BeginTransaction())
                        {
                            if (idGeneros.Count > 0 && idGeneros != null)
                            {
                                var sql_delete = "delete from FilmesGeneros where Filmes_Id = @Filmes_Id";
                                var parametros_delete = new { Filmes_Id };
                                conexao.ExecuteScalar(sql_delete, parametros_delete, transacao);

                                foreach (var generoId in idGeneros)
                                {

                                    var sql_insert = "insert into FilmesGeneros (Filmes_Id, Generos_Id) values (@Filmes_Id, @generoId)";
                                    var parametros_insert = new { Filmes_Id, generoId };
                                    conexao.ExecuteScalar(sql_insert, parametros_insert, transacao);
                                }

                            }
                            transacao.Commit();
                        }

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
        public JsonResult FilmesPagina(int pagina, int tamPag, string filtro, string ordenacao)
        {
            var lista = db.Filmes.
                Where(f => f.Nome.ToLower().Contains(filtro != string.Empty ? filtro.ToLower() : f.Nome));
            switch (ordenacao)
            {
                case "nome asc":
                    lista = lista.OrderBy(s => s.Nome);
                    break;
                case "nome desc":
                    lista = lista.OrderByDescending(s => s.Nome);
                    break;
                case "dtcriacao asc":
                    lista = lista.OrderBy(s => s.DtCriacao);
                    break;
                case "dtcriacao desc":
                    lista = lista.OrderByDescending(s => s.DtCriacao);
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
            var ret = lista.ToList();

            return Json(ret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarFilmes(int id)
        {
            var Filmes = db.Filmes.Find(id);
            Filmes.Genero.Clear();

            

            using (var conexao = new SqlConnection())
            {

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                var sql = "select g.* from FilmesGeneros fg, genero g where fg.filmes_id = @id and g.Id = fg.Generos_Id";
                var parametros = new { id };
                var generos = conexao.ExecuteReader(sql, parametros);                
                while(generos.Read())
                {
                    Filmes.Genero.Add(new GeneroModel
                    {
                        Id = (int)generos["Id"],
                        Nome = (string)generos["nome"]                        
                    });
                }
                


                /*
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select u.* from perfil_usuario pu, usuario u where pu.id_perfil = @id_perfil and pu.id_usuario = u.id";
                    comando.Parameters.Add("@id_perfil", SqlDbType.Int).Value = Id;
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuarios.Add(new UsuarioModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Login = (string)reader["login"]
                        });
                    }
                }*/

            }

            return Json(Filmes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirFilmes(int id)
        {
            FilmesModel Fil = db.Filmes.Find(id);
            db.Filmes.Remove(Fil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirVariosFilmes(int[] id)
        {
            var ret = false;
            foreach (var item in id)
            {
                FilmesModel Fil = db.Filmes.Find(item);
                db.Filmes.Remove(Fil);
                db.SaveChanges();
                ret = true;
            }

            return Json(ret);
        }
    }
}
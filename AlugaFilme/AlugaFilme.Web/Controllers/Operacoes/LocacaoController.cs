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

namespace AlugaFilme.Web.Controllers
{
    public class listaRetorno
    {
        public int Id { get; set; }
        public string Cli_Nome { get; set; }
        public string Dt_Locacao { get; set; }
        public int Qtd_Filmes { get; set; }
        public string Devolvido { get; set; }
    }


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
            List<string> cli = new List<string>();
            foreach (var item in lista)
            {

                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    var sql = "select * from locacao_filmes where locacao_id = @locacao_id";
                    var locacao_id = item.Id;
                    var parametros = new { locacao_id };
                    var filmes = conexao.ExecuteReader(sql, parametros);
                    while (filmes.Read())
                    {
                        item.Filmes.Add(db.Filmes.Find((int)filmes["filmes_id"]));
                    }
                }
                var Cliente = db.Clientes.Find(item.Id_Cliente);
                cli.Add(Cliente.Nome);


            }
            ViewBag.Cliente_Nome = cli;

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

            var cliente = db.Clientes.Where(p => (tipo == "CPF" ? p.NumDocumento : p.Nome).ToLower().Contains(filtro_str));
            var lista = cliente.ToList();
            return Json(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvaLocacao(string ClienteId, string[] FilmesId)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            var locacao = new LocacaoModel();
            var cliente = db.Clientes.Find(Convert.ToInt32(ClienteId));
            locacao.Cpf_Cliente = cliente.NumDocumento;
            locacao.Id_Cliente = cliente.Id;
            locacao.Devolvido = false;
            foreach (var FI in FilmesId)
            {
                FilmesModel Fil = db.Filmes.Find(Convert.ToInt32(FI));
                locacao.Filmes.Add(Fil);
            }
            locacao.DtLocacao = DateTime.Now;
            db.Locacao.Add(locacao);
            db.SaveChanges();
            idSalvo = locacao.Id.ToString();


            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        public string formata_cpf(string texto)
        {
            return Convert.ToUInt64(texto).ToString(@"000\.000\.000\-00");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DevolverLocacao(int id)
        {
            LocacaoModel Loc = db.Locacao.Find(id);
            Loc.Devolvido = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarLocacao(int id)
        {
            var Locacao = db.Locacao.Find(id);

            return Json(Locacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperaFilme(string filtro)
        {

            var Filmes = db.Filmes.Where(p => p.Nome.ToLower().Contains((filtro != string.Empty) ? filtro.ToLower() : p.Nome));

            return Json(Filmes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LocacaoPagina(int pagina, int tamPag, string filtro, string ordenacao)
        {            
            var ret = new List<listaRetorno>();


            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                var sql = "select l.*, c.nome, (select count(*) from locacao_filmes fl where l.Id = fl.Locacao_Id) as qtd from locacao l left join clientes c on c.id=l.id_cliente where c.nome like '%" + filtro.ToLower() + "%'";
                var pos = ((pagina - 1) * tamPag);
                if (pagina > 0 && tamPag > 0)
                {
                    sql += string.Format(" offset {0} rows fetch next {1} rows only", pos, tamPag);
                }
                var loca = conexao.ExecuteReader(sql);
                while (loca.Read())
                {                    
                    var dt = (DateTime)loca["DtLocacao"];                
                    var dev = ((bool)loca["Devolvido"] == true) ? "SIM" : "NÃO";                    
                    ret.Add(new listaRetorno
                    {
                        Id = (int)loca["Id"],
                        Cli_Nome = (string)loca["nome"],
                        Devolvido = dev,
                        Dt_Locacao = dt.ToString(),
                        Qtd_Filmes = (int)loca["qtd"]
                    });
                }
            }            
            return Json(ret.ToList());
        }

    }
}
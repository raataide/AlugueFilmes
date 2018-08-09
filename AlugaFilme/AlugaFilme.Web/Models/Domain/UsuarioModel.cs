using AlugaFilme.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Web;
using AlugaFilme.Web.Models;

//namespace AlugaFilme.Web.Views.Domain
namespace AlugaFilme.Web.Models.Domain
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o login.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe o nome do usuário.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe o e-mail.")]
        public string Email { get; set; }


        public static UsuarioModel ValidarUsuario(string Usuario, string Senha)
        {
            UsuarioModel ret = null;
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                var sql = "select * from usuario where login = @usuario and senha=@senhaHash";
                var senhaHash = Cripto.HashMD5(Senha);
                var parametros = new { Usuario, senhaHash };

                ret = conexao.QuerySingleOrDefault<UsuarioModel>(sql, parametros);                

            }
            return ret;
        }
        public static UsuarioModel RecuperarPeloId(int id)
        {

            UsuarioModel ret = null; 
            
            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                var sql = "select * from usuario where id = @id";
                var parametros = new { id };
                ret = conexao.QuerySingleOrDefault<UsuarioModel>(sql, parametros);                

            }
            return ret;
        }


    }
}
using AlugaFilme.Web.Models;
using AlugaFilme.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace AlugaFilme.Web
{
    public class AppPrinc : GenericPrincipal
    {
        public UsuarioModel Dados { get; set; }

        public AppPrinc(IIdentity identity, string[] roles, int id) : base(identity, roles)
        {
            Dados = UsuarioModel.RecuperarPeloId(id);
        }
    }
}
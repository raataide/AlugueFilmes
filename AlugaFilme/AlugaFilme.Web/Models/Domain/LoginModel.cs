using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlugaFilme.Web.Models.Domain
{
    [Table("Usuario")]
    public class LoginModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O Usuário é obrigatório.")]
        public string Login { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Nome obrigatório.")]
        public string Nome { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage ="O e-mail é obrigatório.")]
        public string Email { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlugaFilme.Web.Models
{
    [Table("Locacao")]
    public class LocacaoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DtLocacao { get; set; }               

        [Required]
        public string Cpf_Cliente { get; set; }
        [Required]
        public int Id_Cliente { get; set; }

        public List<FilmesModel> Filmes { get; set; }
        public LocacaoModel()
        {
            Filmes = new List<FilmesModel>();
        }

    }
}
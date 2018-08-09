using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlugaFilme.Web.Models
{
    [Table("Filmes")]
    public class FilmesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de criação é obrigatória.")]
        public DateTime DtCriacao { get; set; }        
        
        public List<GeneroModel> Genero { get; set; }
        public FilmesModel()
        {
            Genero = new List<GeneroModel>();
        }
        public ICollection<LocacaoModel> Locacao { get; set; }


        public bool Ativo { get; set; }
    }
}
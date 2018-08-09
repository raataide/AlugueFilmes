using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlugaFilme.Web.Models
{
    [Table("Genero")]
    public class GeneroModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="Data é obrigatória.")]
        public DateTime DtCriacao { get; set; }

        [Required]
        public bool Ativo { get; set; }

        public ICollection<FilmesModel> Filmes { get; set; }


    }
}
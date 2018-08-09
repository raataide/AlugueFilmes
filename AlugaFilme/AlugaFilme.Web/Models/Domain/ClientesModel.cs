using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlugaFilme.Web.Models
{
    [Table("Clientes")]
    public class ClientesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string NumDocumento { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage ="Informe o número da casa ou S/N para sem número.")]
        public string Numero { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [StringLength(100)]
        public string Logradouro { get; set; }

        [StringLength(50)]
        public string Complemento { get; set; }

        [StringLength(15)]
        public string Cep { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "O país é obrigatório.")]
        public string Pais { get; set; }

        [StringLength(30)]
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }

        public bool Ativo { get; set; }
    }
}
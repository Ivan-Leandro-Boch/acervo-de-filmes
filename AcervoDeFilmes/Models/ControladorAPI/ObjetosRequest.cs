
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static AcervoDeFilmes.Uteis;

namespace AcervoDeFilmes.Models.ControladorAPI
{
    public class RequestCriarFilme
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Titulo { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Genero { get; set; }
        [Required]
        public int? Ano_Lancamento { get; set; }
        [Required]
        public int? Mes_Lancamento { get; set; }
        [MinimumElementsAttribute(1)]
        public List<Streamings> Streamings { get; set; }
    }


    public class RequestAlteraFilme
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Titulo { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Genero { get; set; }
        [Required]
        public int? Ano_Lancamento { get; set; }
        [Required]
        public int? Mes_Lancamento { get; set; }
        [MinimumElementsAttribute(1)]
        public List<Streamings> Streamings { get; set; }
    }

    public class RequestExcluiFilme
    {
        public string Titulo { get; set; }
    }

    public class Streamings
    {
        [StringLength(100)]
        public string streaming { get; set; }
    }

    public class RequestSalvarAvaliacao
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string Titulo { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Usuario { get; set; }
        [Required]
        public int? Classificacao { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Comentario { get; set; }
    }
}
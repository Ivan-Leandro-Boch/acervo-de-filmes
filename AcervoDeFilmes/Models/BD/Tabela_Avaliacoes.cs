using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcervoDeFilmes.Models.BD
{
    public class Tabela_Avaliacoes
    {
        public long id_Filme { get; set; }
        public string Usuario { get; set; }
        public int Classificacao { get; set; }
        public string Comentario { get; set; }
    }
}
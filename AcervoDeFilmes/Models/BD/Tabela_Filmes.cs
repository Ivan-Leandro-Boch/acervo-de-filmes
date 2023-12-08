using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcervoDeFilmes.Models.BD
{
    
    public class Tabela_Filmes
    {
        public Tabela_Filmes()
        {
            this.Streamings = new List<Tabela_Streamings>();
            this.Avaliacoes = new List<Tabela_Avaliacoes>();
        }

        public long ID { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public int Ano_Lancamento { get; set; }
        public int Mes_Lancamento { get; set; }
        public List<Tabela_Streamings> Streamings { get; set; }
        public List<Tabela_Avaliacoes> Avaliacoes { get; set; }
    }
}
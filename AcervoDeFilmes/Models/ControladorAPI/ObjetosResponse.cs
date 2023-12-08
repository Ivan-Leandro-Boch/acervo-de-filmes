using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcervoDeFilmes.Models.ControladorAPI
{
    public class ResponseAvaliacaoMediaPorFilme
    {
        public long id_Filme { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public double classificacao_media { get; set; }
    }

    public class ResponseLancamentosPorAno
    {
        public int ano_lancamento { get; set; }
        public int qtdLancada { get; set; }
        public List<filme> filmes { get; set; }
    }

    public class filme
    {
        public string Titulo { get; set; }
    }

    public class ResponseLocalizarFilmePorClassificacao
    {
        public long id_Filme { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public double classificacao_media { get; set; }
        public List<Comentario> comentarios { get; set; }
    }

    public class Comentario
    {
        public string comentario { get; set; }
    }


    public class ResponseAvaliacaoMediaPorGenero
    {
        public ResponseAvaliacaoMediaPorGenero()
        {
            avaliacoes = new List<Avaliacoes>();
        }

        public int ano_lancamento { get; set; }
        public List<Avaliacoes> avaliacoes { get; set; }
    }

    public class Avaliacoes
    {
        public string genero { get; set; }
        public double classificacao_media { get; set; }
    }

}
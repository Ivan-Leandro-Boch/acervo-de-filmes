using AcervoDeFilmes.Models.BD;
using AcervoDeFilmes.Models.ControladorAPI;
using AcervoDeFilmes.Repositorios;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AcervoDeFilmes.Controllers
{
    //
    [RoutePrefix("api/Acervo")]
    public class AcervoController : ApiController
    {
        [AcceptVerbs("POST")]
        [Route("CriarFilme")]
        public IHttpActionResult CriarFilme([FromBody]RequestCriarFilme request)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    string messages = string.Join(" - ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));

                    var erro = messages;
                    
                    throw new ApplicationException(erro);
                }

                var repBuscaFilmes = new Tabela_Filmes_Repositorio();

                var lst = repBuscaFilmes.RetornaFilmes();
                var filme = lst.Where(x => x.Titulo == request.Titulo).FirstOrDefault();

                if (filme != null)
                    throw new ApplicationException("Filme já se encontra no acervo!");

                using (var conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            long id_Filme = 0;
                            var repFilmes = new Tabela_Filmes_Repositorio(conn, transaction);
                            var repStreamings = new Tabela_Streamings_Repositorio(conn, transaction);

                            id_Filme= repFilmes.Insere(new Tabela_Filmes()
                            {
                                Titulo = request.Titulo,
                                Ano_Lancamento = request.Ano_Lancamento.GetValueOrDefault(),
                                Mes_Lancamento = request.Mes_Lancamento.GetValueOrDefault(),
                                Genero = request.Genero
                            });

                            foreach(var streaming in request.Streamings)
                            {
                                repStreamings.Insere(new Tabela_Streamings()
                                {
                                    id_Filme = id_Filme,
                                    Streaming = streaming.streaming
                                });
                            }                            

                            transaction.Commit();
                        }
                        catch(Exception tr)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(tr.Message.ToString());
                        }
                    }
                }

                return Ok();
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("PUT")]
        [Route("AlteraFilme")]
        public IHttpActionResult AlteraFilme([FromBody]RequestAlteraFilme request)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    string messages = string.Join(" - ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));

                    var erro = messages; 

                    throw new ApplicationException(erro);
                }

                var repBuscaFilmes = new Tabela_Filmes_Repositorio();               

                var lst = repBuscaFilmes.RetornaFilmes();
                var filme = lst.Where(x => x.Titulo == request.Titulo).FirstOrDefault();

                if (filme == null)
                    throw new ApplicationException("Filme não encontrado no acervo!");

                using (var conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            
                            var repFilmes = new Tabela_Filmes_Repositorio(conn, transaction);
                            var repStreamings = new Tabela_Streamings_Repositorio(conn, transaction);

                            filme.Ano_Lancamento = request.Ano_Lancamento.GetValueOrDefault();
                            filme.Mes_Lancamento = request.Mes_Lancamento.GetValueOrDefault();
                            filme.Genero = request.Genero;
                            repFilmes.Altera(filme);

                            repStreamings.ExcluiPorFilme(filme.ID);
                            foreach (var streaming in request.Streamings)
                            {
                                repStreamings.Insere(new Tabela_Streamings()
                                {
                                    id_Filme = filme.ID,
                                    Streaming = streaming.streaming
                                });
                            }

                            transaction.Commit();
                        }
                        catch (Exception tr)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(tr.Message.ToString());
                        }
                    }
                }

                return Ok();
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }


        [AcceptVerbs("PUT")]
        [Route("ExcluiFilme")]
        public IHttpActionResult ExcluiFilme([FromBody]RequestExcluiFilme request)
        {
            try
            {
                var repBuscaFilmes = new Tabela_Filmes_Repositorio();

                var lst = repBuscaFilmes.RetornaFilmes();
                var filme = lst.Where(x => x.Titulo == request.Titulo).FirstOrDefault();

                if (filme == null)
                    throw new ApplicationException("Filme não encontrado no acervo!");

                using (var conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {

                            var repFilmes = new Tabela_Filmes_Repositorio(conn, transaction);                            
                            repFilmes.Exclui(filme.ID);

                            transaction.Commit();
                        }
                        catch (Exception tr)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(tr.Message.ToString());
                        }
                    }
                }

                return Ok();
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("POST")]
        [Route("SalvarAvaliacao")]
        public IHttpActionResult SalvarAvaliacao([FromBody]RequestSalvarAvaliacao request)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    string messages = string.Join(" - ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));

                    var erro = messages;

                    throw new ApplicationException(erro);
                }

                var repBuscaFilmes = new Tabela_Filmes_Repositorio();
                var repBuscaAvaliacoes = new Tabela_Avaliacoes_Repositorio();

                var lstAva = repBuscaAvaliacoes.RetornaAvaliacoes();

                var lst = repBuscaFilmes.RetornaFilmes();
                var filme = lst.Where(x => x.Titulo == request.Titulo).FirstOrDefault();

                if (filme == null)
                    throw new ApplicationException("Filme não se encontra no acervo!");

                var avaliacao = lstAva.Where(x => x.id_Filme == filme.ID && x.Usuario == request.Usuario).FirstOrDefault();
                if (avaliacao != null)
                    throw new ApplicationException("Usuário já avaliou o filme!");

                using (var conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            var repAvaliacao = new Tabela_Avaliacoes_Repositorio(conn, transaction);

                            repAvaliacao.Insere(new Tabela_Avaliacoes()
                            {
                                id_Filme = filme.ID,
                                Classificacao = request.Classificacao.GetValueOrDefault(),
                                Comentario = request.Comentario,
                                Usuario = request.Usuario

                            });

                            transaction.Commit();
                        }
                        catch (Exception tr)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(tr.Message.ToString());
                        }
                    }
                }

                return Ok();
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("Get")]
        [Route("ListarFilmes")]
        public IHttpActionResult ListarFilmes(int? paginaTamanho,  int? paginaNumero)
        {
            try
            {
                var repBuscaFilmes = new Tabela_Filmes_Repositorio();
                var repBuscaStreamings = new Tabela_Streamings_Repositorio();
                var repBuscaAvaliacoes = new Tabela_Avaliacoes_Repositorio();

                var lst = repBuscaFilmes.RetornaFilmes();
                var lstAva = repBuscaAvaliacoes.RetornaAvaliacoes();
                var lstStream = repBuscaStreamings.RetornaStreamings();

                foreach (var item in lst)
                {
                    item.Streamings.AddRange(lstStream.Where(x => x.id_Filme == item.ID));
                    item.Avaliacoes.AddRange(lstAva.Where(x => x.id_Filme == item.ID));
                }

                if (lst.Count > 0)
                {
                    if (paginaTamanho.GetValueOrDefault() == 0)
                        paginaTamanho = lst.Count();
                }
                else
                    paginaTamanho = 1;

                if (paginaTamanho == lst.Count())
                    paginaNumero = 1;
                else
                {
                    if (paginaNumero.GetValueOrDefault() <= 0)
                        paginaNumero = 1;
                }

                return Ok(lst.ToPagedList(paginaNumero.GetValueOrDefault(), paginaTamanho.GetValueOrDefault()));
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("Get")]
        [Route("StreamingsPorFilme")]
        public IHttpActionResult StreamingsPorFilme(string titulo)
        {
            try
            {
                if (string.IsNullOrEmpty(titulo))
                    throw new ApplicationException("Informe um título!");

                var repBuscaFilmes = new Tabela_Filmes_Repositorio();
                var repBuscaStreamings = new Tabela_Streamings_Repositorio();
                var repBuscaAvaliacoes = new Tabela_Avaliacoes_Repositorio();
                
                var lstTemp = repBuscaFilmes.RetornaFilmes();

                var lst = lstTemp.Where(x => x.Titulo == titulo).ToList();
                var lstStream = repBuscaStreamings.RetornaStreamings();

                foreach (var item in lst)
                {
                    item.Streamings.AddRange(lstStream.Where(x => x.id_Filme == item.ID));
                }

                if (lst.Count() == 0)
                    throw new ApplicationException("Título não foi encontrado!");

                

                return Ok(lst);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("Get")]
        [Route("AvaliacaoMediaPorFilme")]
        public IHttpActionResult AvaliacaoMediaPorFilme(string titulo)
        {
            try
            {
                var lst = new List<Tabela_Filmes>();
                
                var repBuscaFilmes = new Tabela_Filmes_Repositorio();
                var repBuscaAvaliacoes = new Tabela_Avaliacoes_Repositorio();

                var lstTemp = repBuscaFilmes.RetornaFilmes();

                if (!string.IsNullOrEmpty(titulo))
                    lst = lstTemp.Where(x => x.Titulo == titulo).ToList();
                else
                    lst = lstTemp.ToList();

                var lstTempAva = repBuscaAvaliacoes.RetornaAvaliacoes();                

                var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Tabela_Filmes, ResponseAvaliacaoMediaPorFilme>()
                 .ForMember(destino => destino.id_Filme, vm => vm.MapFrom(origin => origin.ID))
                );
                var mapperx = config.CreateMapper();
                var objRetorno = mapperx.Map<List<ResponseAvaliacaoMediaPorFilme>>(lst);

                foreach (var item in objRetorno)
                {
                    var lstAva = lstTempAva.Where(x => x.id_Filme == item.id_Filme).ToList();
                    if (lstAva.Count > 0)
                        item.classificacao_media = double.Parse(Math.Round(lstAva.Average(b => b.Classificacao), 1).ToString());
                }

                

                if (objRetorno.Count() == 0)
                    throw new ApplicationException("Título não foi encontrado!");



                return Ok(objRetorno);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("Get")]
        [Route("LancamentosPorAno")]
        public IHttpActionResult LancamentosPorAno(string ano)
        {
            try
            {
                var lst = new List<Tabela_Filmes>();
                var objRetorno = new List<ResponseLancamentosPorAno>();

                var repBuscaFilmes = new Tabela_Filmes_Repositorio();

                var lstTemp = repBuscaFilmes.RetornaFilmes();

                if (!string.IsNullOrEmpty(ano))
                    lst = lstTemp.Where(x => x.Ano_Lancamento == int.Parse(ano)).ToList();
                else
                    lst = lstTemp.ToList();
                

                foreach (var item in lst.Select(x => x.Ano_Lancamento).Distinct())
                {

                    var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Tabela_Filmes, filme>());
                    var mapperx = config.CreateMapper();
                    var lstFilmes = mapperx.Map<List<filme>>(lst.Where(x => x.Ano_Lancamento == item));

                    objRetorno.Add(new ResponseLancamentosPorAno()
                    {
                        ano_lancamento = item,
                        qtdLancada = lst.Where(x => x.Ano_Lancamento == item).Count(),
                        filmes = lstFilmes
                    });
                }



                if (objRetorno.Count() == 0)
                    throw new ApplicationException("Título não foi encontrado!");



                return Ok(objRetorno);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("Get")]
        [Route("LocalizarFilmePorClassificacao")]
        public IHttpActionResult LocalizarFilmePorClassificacao(int? classificacao)
        {
            try
            {
                var repBuscaFilmes = new Tabela_Filmes_Repositorio();
                var repBuscaAvaliacoes = new Tabela_Avaliacoes_Repositorio();

                var lst = repBuscaFilmes.RetornaFilmes();                

                var lstTempAva = repBuscaAvaliacoes.RetornaAvaliacoes();

                var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Tabela_Filmes, ResponseLocalizarFilmePorClassificacao>()
                 .ForMember(destino => destino.id_Filme, vm => vm.MapFrom(origin => origin.ID))
                );
                var mapperx = config.CreateMapper();
                var objRetorno = mapperx.Map<List<ResponseLocalizarFilmePorClassificacao>>(lst);

                foreach (var item in objRetorno)
                {
                    var lstAva = lstTempAva.Where(x => x.id_Filme == item.id_Filme).ToList();

                    var config1 = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Tabela_Avaliacoes, Comentario>());
                    var mapperx1 = config1.CreateMapper();
                    var lstComentarios = mapperx1.Map<List<Comentario>>(lstAva);

                    if (lstAva.Count > 0)
                        item.classificacao_media = double.Parse(Math.Round(lstAva.Average(b => b.Classificacao), 1).ToString());

                    item.comentarios = lstComentarios;
                }



                if (objRetorno.Count() == 0)
                    throw new ApplicationException("Título não foi encontrado!");

                if (classificacao.HasValue)
                    objRetorno = objRetorno.Where(x => x.classificacao_media == classificacao.GetValueOrDefault()).ToList();

                return Ok(objRetorno);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }

        [AcceptVerbs("Get")]
        [Route("AvaliacaoMediaPorGenero")]
        public IHttpActionResult AvaliacaoMediaPorGenero()
        {
            try
            {                
                var objRetorno = new List<ResponseAvaliacaoMediaPorGenero>();

                var repBuscaFilmes = new Tabela_Filmes_Repositorio();
                var repBuscaAvaliacoes = new Tabela_Avaliacoes_Repositorio();
                
                var lst = repBuscaFilmes.RetornaFilmes();

                var lstAva = repBuscaAvaliacoes.RetornaAvaliacoes();
                

                foreach (var item in lst.Select(x => x.Ano_Lancamento).Distinct())
                {
                    var Avaliacoes = new List<Avaliacoes>();
                    var lstTempFilmes = lst.Where(x => x.Ano_Lancamento == item).ToList();
                    
                    foreach (var subitem in lstTempFilmes.Select(x => x.Genero).Distinct())
                    {
                        var lstTempFilmes1 = lst.Where(x => x.Ano_Lancamento == item && x.Genero == subitem).ToList();
                        var lstTempAva = lstAva.Where(i => lstTempFilmes1.Any(a => i.id_Filme == a.ID)).ToList();


                        Avaliacoes.Add(new Avaliacoes()
                        {
                            classificacao_media = lstTempAva.Count > 0 ? double.Parse(Math.Round(lstTempAva.Average(b => b.Classificacao), 1).ToString()) : 0,
                            genero = subitem
                        });
                        
                    }

                    objRetorno.Add(new ResponseAvaliacaoMediaPorGenero()
                    {
                        ano_lancamento = item,
                        avaliacoes = Avaliacoes
                    });
                }



                if (objRetorno.Count() == 0)
                    throw new ApplicationException("Título não foi encontrado!");



                return Ok(objRetorno);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message.ToString());
            }

        }



    }
}

using AcervoDeFilmes.Models.BD;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AcervoDeFilmes.Repositorios
{
    public class Tabela_Avaliacoes_Repositorio
    {
        SqlConnection sqlConn = null;
        SqlTransaction sqlTrans = null;

        #region Construtor

        public Tabela_Avaliacoes_Repositorio()
        {

        }

        public Tabela_Avaliacoes_Repositorio(SqlConnection sqlConn1, SqlTransaction sqlTrans1)
        {
            sqlConn = sqlConn1;
            sqlTrans = sqlTrans1;
        }

        #endregion

        #region Metodos de Retorno

        public List<Tabela_Avaliacoes> RetornaAvaliacoes()
        {
            string sql = string.Format(@"SELECT * FROM Avaliacoes;");

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
            {
                var result = conn.Query<Tabela_Avaliacoes>(sql);
                return result.ToList();
            }
        }



        #endregion

        #region "Métodos de Manutenção"

        public void Insere(Tabela_Avaliacoes objAvaliacao)
        {
            string campos = "id_Filme,usuario,classificacao,comentario";
            string valores = "@id_Filme,@usuario,@classificacao,@comentario";

            string sql = string.Format(@"INSERT INTO Avaliacoes
                                  ({0})
                                 VALUES
                                  ({1});", campos, valores);

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, objAvaliacao);
                }
            }
            else
            {
                sqlConn.Execute(sql, objAvaliacao, sqlTrans);
            }

        }

        public void Altera(Tabela_Avaliacoes objAvaliacao)
        {
            string campos = "classificacao=@classificacao, comentario=@comentario";

            string sql = string.Format(@"UPDATE Avaliacoes
                                        SET {0}
                                        WHERE id_Filme = @id_Filme AND usuario = @usuario;", campos);

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, objAvaliacao);
                }
            }
            else
            {
                sqlConn.Execute(sql, objAvaliacao, sqlTrans);
            }

        }

        public void Exclui(long Id_Filme, string Usuario)
        {
            string sql = @"DELETE FROM Avaliacoes WHERE id_Filme = @id_Filme AND usuario = @usuario;";

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, new { id_Filme = Id_Filme, usuario = Usuario });
                }
            }
            else
            {
                sqlConn.Execute(sql, new { id_Filme = Id_Filme, usuario = Usuario }, sqlTrans);
            }
        }

        #endregion
    }
}
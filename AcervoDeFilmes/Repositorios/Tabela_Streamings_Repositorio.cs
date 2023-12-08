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
    public class Tabela_Streamings_Repositorio
    {
        SqlConnection sqlConn = null;
        SqlTransaction sqlTrans = null;

        #region Construtor

        public Tabela_Streamings_Repositorio()
        {

        }

        public Tabela_Streamings_Repositorio(SqlConnection sqlConn1, SqlTransaction sqlTrans1)
        {
            sqlConn = sqlConn1;
            sqlTrans = sqlTrans1;
        }

        #endregion

        #region Metodos de Retorno

        public List<Tabela_Streamings> RetornaStreamings()
        {
            string sql = string.Format(@"SELECT * FROM Streamings;");

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
            {
                var result = conn.Query<Tabela_Streamings>(sql);
                return result.ToList();
            }
        }



        #endregion

        #region "Métodos de Manutenção"

        public void Insere(Tabela_Streamings objStreaming)
        {
            string campos = "id_Filme,streaming";
            string valores = "@id_Filme,@streaming";

            string sql = string.Format(@"INSERT INTO Streamings
                                  ({0})
                                 VALUES
                                  ({1});", campos, valores);

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, objStreaming);
                }
            }
            else
            {
                sqlConn.Execute(sql, objStreaming, sqlTrans);
            }

        }
        

        public void Exclui(long Id_Filme, string Streaming)
        {
            string sql = @"DELETE FROM Streamings WHERE id_Filme = @id_Filme AND streaming = @streaming;";

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, new { id_Filme = Id_Filme, streaming = Streaming });
                }
            }
            else
            {
                sqlConn.Execute(sql, new { id_Filme = Id_Filme, streaming = Streaming }, sqlTrans);
            }
        }

        public void ExcluiPorFilme(long Id_Filme)
        {
            string sql = @"DELETE FROM Streamings WHERE id_Filme = @id_Filme;";

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, new { id_Filme = Id_Filme });
                }
            }
            else
            {
                sqlConn.Execute(sql, new { id_Filme = Id_Filme }, sqlTrans);
            }
        }

        #endregion
    }
}
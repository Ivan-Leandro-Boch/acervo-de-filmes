using AcervoDeFilmes.Models.BD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;

namespace AcervoDeFilmes.Repositorios
{
    public class Tabela_Filmes_Repositorio
    {
        SqlConnection sqlConn = null;
        SqlTransaction sqlTrans = null;

        #region Construtor

        public Tabela_Filmes_Repositorio()
        {

        }

        public Tabela_Filmes_Repositorio(SqlConnection sqlConn1, SqlTransaction sqlTrans1)
        {
            sqlConn = sqlConn1;
            sqlTrans = sqlTrans1;
        }

        #endregion

        #region Metodos de Retorno

        public List<Tabela_Filmes> RetornaFilmes()
        {
            string sql = string.Format(@"SELECT * FROM Filmes;");

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
            {
                var result = conn.Query<Tabela_Filmes>(sql);
                return result.ToList();
            }
        }



        #endregion

        #region "Métodos de Manutenção"

        public long Insere(Tabela_Filmes objFilme)
        {
            string campos = "titulo,genero,ano_lancamento,mes_lancamento";
            string valores = "@titulo,@genero,@ano_lancamento,@mes_lancamento";

            string sql = string.Format(@"INSERT INTO Filmes
                                  ({0})
                                 VALUES
                                  ({1}); SELECT SCOPE_IDENTITY();", campos, valores);

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    return conn.ExecuteScalar<long>(sql, objFilme);
                }
            }
            else
            {
                return sqlConn.ExecuteScalar<long>(sql, objFilme, sqlTrans);
            }

        }

        public void Altera(Tabela_Filmes objFilme)
        {
            string campos = "titulo=@titulo, genero=@genero, ano_lancamento=@ano_lancamento, mes_lancamento=@mes_lancamento";

            string sql = string.Format(@"UPDATE Filmes
                                        SET {0}
                                        WHERE Id = @Id;", campos);

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, objFilme);
                }
            }
            else
            {
                sqlConn.Execute(sql, objFilme, sqlTrans);
            }

        }

        public void Exclui(long Id)
        {
            string sql = @"DELETE FROM Filmes WHERE Id = @Id;";

            if (sqlConn == null)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
                {
                    conn.Execute(sql, new { Id = Id });
                }
            }
            else
            {
                sqlConn.Execute(sql, new { Id = Id }, sqlTrans);
            }
        }

        #endregion
    }
}
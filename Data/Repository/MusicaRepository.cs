using Data.BancoDeDados;
using Data.Models;
using Microsoft.Data.Sqlite;
using Data.ViewModel;

namespace Data.Repository
{
    public class MusicaRepository
    {

        // Caminho do banco de dados
        private readonly string _caminhoBanco;

        public MusicaRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }

        // Metodo para listar musicas
        public List<MusicaModel> ListarMusicas()
        {
            try
            {
                var musicas = new List<MusicaModel>();
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string selectSql = "SELECT * FROM Musica";
                using var cmd = new SqliteCommand(selectSql, conexao);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    musicas.Add(new MusicaModel
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        Titulo = reader.IsDBNull(reader.GetOrdinal("Titulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Titulo")),
                        NomeArtista = reader.IsDBNull(reader.GetOrdinal("NomeArtista")) ? string.Empty : reader.GetString(reader.GetOrdinal("NomeArtista")),
                        Genero = reader.IsDBNull(reader.GetOrdinal("Genero")) ? string.Empty : reader.GetString(reader.GetOrdinal("Genero")),
                        Ano = reader.IsDBNull(reader.GetOrdinal("Ano")) ? 0 : reader.GetInt32(reader.GetOrdinal("Ano")),
                        NomeArquivo = reader.IsDBNull(reader.GetOrdinal("NomeArquivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("NomeArquivo")),
                        Arquivo = reader.IsDBNull(reader.GetOrdinal("Arquivo")) ? Array.Empty<byte>() : (byte[])reader["Arquivo"],
                        Usuario_Id = reader.IsDBNull(reader.GetOrdinal("Usuario_Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Usuario_Id"))
                    });
                }

                return musicas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar músicas: {ex.Message}");
                return new List<MusicaModel>();
            }
        }

        // Metodo para adicionar musicas
        public bool AdicionarMusica(string titulo, string nomeArtista, string genero, int ano, string nomeArquivo, byte[] arquivo, int Usuario_Id)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string insertSql = "INSERT INTO Musica (Titulo, NomeArtista, Genero, Ano, NomeArquivo, Arquivo, Usuario_Id) " +
                                   "VALUES (@Titulo, @NomeArtista, @Genero, @Ano, @NomeArquivo, @Arquivo, @Usuario_Id)";
                using var cmd = new SqliteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@NomeArtista", nomeArtista);
                cmd.Parameters.AddWithValue("@Genero", genero);
                cmd.Parameters.AddWithValue("@Ano", ano);
                cmd.Parameters.AddWithValue("@NomeArquivo", nomeArquivo);
                cmd.Parameters.AddWithValue("@Arquivo", arquivo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Usuario_Id", Usuario_Id);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro SQLite: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return false;
            }
        }

        //Metodo para excluir musica
        public bool ExcluirMusicaPorId(int id)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string deleteSql = "DELETE FROM Musica WHERE Id = @Id";
                using var cmd = new SqliteCommand(deleteSql, conexao);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro SQLite: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return false;
            }
        }
        public void IncrementarReproducao(int id)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();
            using var cmd = new SqliteCommand(
                "UPDATE Musica SET Reproducao = Reproducao + 1 WHERE Id = @id", conexao);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<EstatisticaViewArtistaModel> ObterEstatisticasArtistas()
        {
            var lista = new List<EstatisticaViewArtistaModel>();
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();
            string sql = @"
                SELECT NomeArtista, COUNT(*) AS TotalMusicas, SUM(Reproducao) AS TotalReproducoes
                  FROM Musica
              GROUP BY NomeArtista
              ORDER BY TotalReproducoes DESC";
            using var cmd = new SqliteCommand(sql, conexao);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new EstatisticaViewArtistaModel
                {
                    NomeArtista = reader.GetString(0),
                    TotalMusicas = reader.GetInt32(1),
                    TotalReproducoes = reader.GetInt32(2)
                });
            }
            return lista;
        }

        public List<EstatisticaMusicaViewModel> ObterEstatisticasMusicas()
        {
            var lista = new List<EstatisticaMusicaViewModel>();
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();
            string sql = @"
                SELECT Id, Titulo, NomeArtista, Reproducao AS TotalReproducoes
                  FROM Musica
              ORDER BY TotalReproducoes DESC
                 LIMIT 10";
            using var cmd = new SqliteCommand(sql, conexao);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new EstatisticaMusicaViewModel
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    NomeArtista = reader.GetString(2),
                    TotalReproducoes = reader.GetInt32(3)
                });
            }
            return lista;
        }
    }
}
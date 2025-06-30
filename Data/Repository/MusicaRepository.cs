using Data.Config;
using Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class MusicaRepository
    {
        // Caminho do banco de dados
        private readonly string caminhoBanco;

        // Construtor que carrega configurações do banco de dados
        public MusicaRepository()
        {
            IConfigurationRoot config = ConfigHelper.LoadConfiguration();
            string caminhoArquivo = config.GetSection("DataSettings:ConexaoBanco").Value
                                    ?? throw new InvalidOperationException("ConexaoBanco não encontrada nas configurações.");

            caminhoBanco = $"Data Source={caminhoArquivo}";
        }

        // Metodo para listar musicas
        public List<MusicaModel> ListarMusicas()
        {
            try
            {
                using var conexao = new SqliteConnection(caminhoBanco);
                conexao.Open();

                string selectSql = "SELECT * FROM Musica";
                using var cmd = new SqliteCommand(selectSql, conexao);
                using var reader = cmd.ExecuteReader();

                var musicas = new List<MusicaModel>();
                while (reader.Read())
                {
                    var musica = new MusicaModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                        NomeArtista = reader.GetString(reader.GetOrdinal("NomeArtista")),
                        Genero = reader.GetString(reader.GetOrdinal("Genero")),
                        Ano = reader.GetInt32(reader.GetOrdinal("Ano")),
                        NomeArquivo = reader.GetString(reader.GetOrdinal("NomeArquivo")),
                        Usuario_Id = reader.GetInt32(reader.GetOrdinal("Usuario_Id"))
                    };
                    musicas.Add(musica);
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
                using var conexao = new SqliteConnection(caminhoBanco);
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
   
        //Metodo para excluir musicas
        public bool ExcluirMusicaPorId(int id)
        {
            try
            {
                using var conexao = new SqliteConnection(caminhoBanco);
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
    }
}
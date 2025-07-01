using Business.Utilities;
using Data.Config;
using Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class ArtistaRepository
    {       
        // Caminho do banco de dados
        private readonly string caminhoBanco;

        // Construtor que carrega configurações do banco de dados
        public ArtistaRepository()
        {
            IConfigurationRoot config = ConfigHelper.LoadConfiguration();
            string caminhoArquivo = config.GetSection("DataSettings:ConexaoBanco").Value
                                    ?? throw new InvalidOperationException("ConexaoBanco não encontrada nas configurações.");

            caminhoBanco = $"Data Source={caminhoArquivo}";
        }  
             
        // Método para listar músicas por usuário
        public List<MusicaModel> ListarMusicasPorUsuario(int usuarioId)
        {
            var musicas = new List<MusicaModel>();
            using var conexao = new SqliteConnection(caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Id, Titulo, NomeArtista, Genero, Ano, NomeArquivo FROM Musica WHERE Usuario_Id = @Usuario_Id";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Usuario_Id", usuarioId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                musicas.Add(new MusicaModel
                {
                    Id = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    NomeArtista = reader.GetString(2),
                    Genero = reader.GetString(3),
                    Ano = reader.GetInt32(4),
                    NomeArquivo = reader.GetString(5)
                });
            }
            return musicas;
        }
    }
}
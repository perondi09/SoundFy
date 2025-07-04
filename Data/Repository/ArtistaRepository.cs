<<<<<<< HEAD
using Data.BancoDeDados;
=======
using Data.Config;
>>>>>>> 8ead8e9e4cd55d59a14497d41aa31da90884a682
using Data.Models;
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class ArtistaRepository
    {       
        // Caminho do banco de dados
        private readonly string _caminhoBanco;

        public ArtistaRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }
             
        // Método para listar músicas por usuário
        public List<MusicaModel> ListarMusicasPorUsuario(int usuarioId)
        {
            var musicas = new List<MusicaModel>();
            using var conexao = new SqliteConnection(_caminhoBanco);
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
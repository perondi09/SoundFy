<<<<<<< HEAD
<<<<<<< HEAD
using Data.BancoDeDados;
=======
using Data.Config;
>>>>>>> 8ead8e9e4cd55d59a14497d41aa31da90884a682
=======
using Data.BancoDeDados;
>>>>>>> extracao-modulo-adm
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class OuvinteRepository
    {       
        // Caminho do banco de dados
        private readonly string _caminhoBanco;

        public OuvinteRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }

        // Método para obter os bytes de uma música pelo ID
        public byte[]? ObterBytesMusicaPorId(int id)
        {
<<<<<<< HEAD
            using var conexao = new SqliteConnection(_caminhoBanco);
=======
            using var conexao = new SqliteConnection( _caminhoBanco);
>>>>>>> extracao-modulo-adm
            conexao.Open();

            string selectSql = "SELECT Arquivo FROM Musica WHERE Id = @Id";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader["Arquivo"] as byte[];
            }
            return null;
        }
    }
}
<<<<<<< HEAD
using Data.BancoDeDados;
=======
using Data.Config;
>>>>>>> 8ead8e9e4cd55d59a14497d41aa31da90884a682
using Data.Models;
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class AdministradorRepository
    {   
        // Caminho do banco de dados
        private readonly string _caminhoBanco;

        public AdministradorRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }

        //Metodo para validar administrador
        public bool ValidarAdministrador(string email, string senha)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT * FROM Administrador WHERE Email = @Email AND Senha = @Senha";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        //Metodo para excluir usuarios
        public bool ExcluirUsuario(int id)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string deleteSql = "DELETE FROM Usuario WHERE Id = @Id";
                using var cmd = new SqliteCommand(deleteSql, conexao);
                cmd.Parameters.AddWithValue("@Id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao excluir usuário.", ex);
            }
        }

        //Metodo para listar usuários
        public List<UsuarioModel> ListarUsuarios()
        {
            var usuarios = new List<UsuarioModel>();
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Id, Email, Senha, Tipo FROM Usuario";
            using var cmd = new SqliteCommand(selectSql, conexao);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                usuarios.Add(new UsuarioModel
                {
                    Id = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    Senha = reader.GetString(2),
                    Tipo = reader.GetString(3)
                });
            }

            return usuarios;
        }
    }
}


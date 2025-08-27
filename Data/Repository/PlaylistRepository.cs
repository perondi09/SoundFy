using System;
using System.Collections.Generic;
using Data.BancoDeDados;
using Data.Models;
using Microsoft.Data.Sqlite;

namespace Data.Repository
{
    public class PlaylistRepository
    {
        private readonly string _caminhoBanco;

        public PlaylistRepository()
        {
            _caminhoBanco = ConexaoBanco.ObterStringConexao();
        }

        public bool AdicionarPlaylist(string nome, int usuarioId)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string insertSql = "INSERT INTO Playlists (Nome_Playlist, Usuario_Id) VALUES (@Nome_Playlist, @Usuario_Id)";
                using var cmd = new SqliteCommand(insertSql, conexao);
                cmd.Parameters.AddWithValue("@Nome_Playlist", nome);
                cmd.Parameters.AddWithValue("@Usuario_Id", usuarioId);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ExcluirPlaylistPorId(int id)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string deleteSql = "DELETE FROM Playlists WHERE Id = @Id";
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

        public List<PlaylistModel> ListarPlaylists()
        {
            try
            {
                var playlists = new List<PlaylistModel>();
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string selectSql = "SELECT Id, Nome_Playlist, Usuario_Id FROM Playlists";
                using var cmd = new SqliteCommand(selectSql, conexao);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    playlists.Add(new PlaylistModel
                    {
                        Id = reader.GetInt32(0),
                        Nome_Playlist = reader.GetString(1),
                        Usuario_Id = reader.GetInt32(2)
                    });
                }

                return playlists;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro SQLite: {ex.Message}");
                return new List<PlaylistModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return new List<PlaylistModel>();
            }
        }   

        public PlaylistModel ObterPlaylistPorId(int id)
        {
            try
            {
                using var conexao = new SqliteConnection(_caminhoBanco);
                conexao.Open();

                string selectSql = "SELECT Id, Nome_Playlist, Usuario_Id FROM Playlists WHERE Id = @Id";
                using var cmd = new SqliteCommand(selectSql, conexao);
                cmd.Parameters.AddWithValue("@Id", id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new PlaylistModel
                    {
                        Id = reader.GetInt32(0),
                        Nome_Playlist = reader.GetString(1),
                        Usuario_Id = reader.GetInt32(2)
                    };
                }

                return null;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro SQLite: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                return null;
            }
        }
    }
}
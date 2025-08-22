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

        public int CriarPlaylist(string nome, int ouvinteId)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string insertSql = "INSERT INTO Playlists (Nome, OuvinteId) VALUES (@Nome, @OuvinteId); SELECT last_insert_rowid();";
            using var cmd = new SqliteCommand(insertSql, conexao);
            cmd.Parameters.AddWithValue("@Nome", nome);
            cmd.Parameters.AddWithValue("@OuvinteId", ouvinteId);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<PlaylistModel> ListarPlaylistsPorOuvinte(int ouvinteId)
        {
            var playlists = new List<PlaylistModel>();
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = "SELECT Id, Nome, OuvinteId FROM Playlists WHERE OuvinteId = @OuvinteId";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@OuvinteId", ouvinteId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                playlists.Add(new PlaylistModel
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Usuario_Id = reader.GetInt32(2) // ou OuvinteId, ajuste no seu model se quiser
                });
            }
            return playlists;
        }

        public bool ExcluirPlaylist(int playlistId)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string deleteMusicas = "DELETE FROM PlaylistMusicas WHERE PlaylistId = @PlaylistId";
            using var cmdMusicas = new SqliteCommand(deleteMusicas, conexao);
            cmdMusicas.Parameters.AddWithValue("@PlaylistId", playlistId);
            cmdMusicas.ExecuteNonQuery();

            string deletePlaylist = "DELETE FROM Playlists WHERE Id = @Id";
            using var cmdPlaylist = new SqliteCommand(deletePlaylist, conexao);
            cmdPlaylist.Parameters.AddWithValue("@Id", playlistId);
            return cmdPlaylist.ExecuteNonQuery() > 0;
        }

        public bool AdicionarMusicaNaPlaylist(int playlistId, int musicaId)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string insertSql = "INSERT INTO PlaylistMusicas (PlaylistId, MusicaId) VALUES (@PlaylistId, @MusicaId)";
            using var cmd = new SqliteCommand(insertSql, conexao);
            cmd.Parameters.AddWithValue("@PlaylistId", playlistId);
            cmd.Parameters.AddWithValue("@MusicaId", musicaId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool RemoverMusicaDaPlaylist(int playlistId, int musicaId)
        {
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string deleteSql = "DELETE FROM PlaylistMusicas WHERE PlaylistId = @PlaylistId AND MusicaId = @MusicaId";
            using var cmd = new SqliteCommand(deleteSql, conexao);
            cmd.Parameters.AddWithValue("@PlaylistId", playlistId);
            cmd.Parameters.AddWithValue("@MusicaId", musicaId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public List<MusicaModel> ListarMusicasDaPlaylist(int playlistId)
        {
            var musicas = new List<MusicaModel>();
            using var conexao = new SqliteConnection(_caminhoBanco);
            conexao.Open();

            string selectSql = @"
                SELECT m.Id, m.Titulo, m.NomeArtista, m.Genero, m.Ano, m.NomeArquivo
                FROM Musica m
                INNER JOIN PlaylistMusicas pm ON m.Id = pm.MusicaId
                WHERE pm.PlaylistId = @PlaylistId";
            using var cmd = new SqliteCommand(selectSql, conexao);
            cmd.Parameters.AddWithValue("@PlaylistId", playlistId);

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;

namespace Business
{
    public class PlaylistBusiness
    {
        PlaylistRepository playlistRepo = new PlaylistRepository();

        public int CriarPlaylist(string nome, int usuarioId)
        {           
            if (string.IsNullOrWhiteSpace(nome))
                throw new BusinessException("Nome da playlist não pode ser vazio.");

            return playlistRepo.CriarPlaylist(nome, usuarioId);
        }

        public List<PlaylistModel> ListarPlaylistsPorUsuario(int usuarioId)
        {
            return playlistRepo.ListarPlaylistsPorOuvinte(usuarioId);
        }

        public bool ExcluirPlaylist(int playlistId)
        {
            return playlistRepo.ExcluirPlaylist(playlistId);
        }

        public bool AdicionarMusicaNaPlaylist(int playlistId, int musicaId)
        {
            return playlistRepo.AdicionarMusicaNaPlaylist(playlistId, musicaId);
        }

        public bool RemoverMusicaDaPlaylist(int playlistId, int musicaId)
        {
            return playlistRepo.RemoverMusicaDaPlaylist(playlistId, musicaId);
        }       

        public List<MusicaModel> ListarMusicasDaPlaylist(int playlistId)
        {
            return playlistRepo.ListarMusicasDaPlaylist(playlistId);
        }
    }
}
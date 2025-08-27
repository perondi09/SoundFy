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
        MusicaRepository musicaRepo = new MusicaRepository();

        public bool AdicionarPlaylist(string nome, int usuarioId)
        {
            return playlistRepo.AdicionarPlaylist(nome, usuarioId);
        }

        public bool ExcluirPlaylistPorId(int id)
        {
            return playlistRepo.ExcluirPlaylistPorId(id);
        }

        public List<PlaylistModel> ListarPlaylist()
        {
            return playlistRepo.ListarPlaylists();
        }

        public PlaylistModel ObterPlaylistPorId(int id)
        {
            return playlistRepo.ObterPlaylistPorId(id);
        }        
    }
}
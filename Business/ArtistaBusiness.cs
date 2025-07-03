using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;

namespace Business
{
    public class ArtistaBusiness
    {
        ArtistaRepository artistaRepo = new ArtistaRepository();
        MusicaRepository musicaRepo = new MusicaRepository();

        public List<MusicaModel> ListarMusicasPorArtista(int usuarioId)
        {
            return artistaRepo.ListarMusicasPorUsuario(usuarioId);
        }

        
        public bool AdicionarMusica(string titulo, string nomeArtista, string genero, int ano, string nomeArquivo, byte[] arquivo, int Usuario_Id)
        {
            return musicaRepo.AdicionarMusica(titulo, nomeArtista, genero, ano, nomeArquivo, arquivo, Usuario_Id);
        }

        public bool ExcluirMusicaPorId(int id)
        {
            return musicaRepo.ExcluirMusicaPorId(id);
        }
    }
}
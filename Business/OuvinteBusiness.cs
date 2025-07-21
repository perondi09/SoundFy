using Data.Models;
using Data.Repository;

namespace Business.Properties
{
    public class OuvinteBusiness
    {
        MusicaRepository musicaRepo = new MusicaRepository();
        OuvinteRepository ouvinteRepo = new OuvinteRepository();

        public List<MusicaModel> ListarMusicas()
        {
            return musicaRepo.ListarMusicas();
        }

        public byte[]? ObterBytesMusicaPorId(int id)
        {
            return ouvinteRepo.ObterBytesMusicaPorId(id);
        }     

         public bool ContaReproducao(int Id, int Reproducao)
        {
            return musicaRepo.Reproducoes(Id, Reproducao);
        }          
    }
}
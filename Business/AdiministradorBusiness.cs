using Data.Repository;

namespace Business
{
    public class AdiministradorBusiness
    {
        AdministradorRepository AdmRepo = new AdministradorRepository();
        MusicaRepository MusicaRepo = new MusicaRepository();

        public bool ExcluirUsuarioPeloId(int id)
        {
            return AdmRepo.ExcluirUsuario(id);
        }

        public List<Data.Models.UsuarioModel> ListarUsuariosPeloId()
        {
            return AdmRepo.ListarUsuarios();
        }

        public bool ValidarSeUsuarioExiste(string email, string senha)
        {
            return AdmRepo.ValidarAdministrador(email, senha);
        }         
    }
}
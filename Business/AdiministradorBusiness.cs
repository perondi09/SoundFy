using Data.Repository;

namespace Business
{
    public class AdiministradorBusiness
    {
        AdministradorRepository AdmRepo = new AdministradorRepository();

        public bool ExcluirUsuarioPeloId(int id)
        {
            return AdmRepo.ExcluirUsuario(id);
        }

        public List<Data.Models.UsuarioModel> ListarUsuariosPeloId()
        {
            return AdmRepo.ListarUsuarios();
        }      
    }
}
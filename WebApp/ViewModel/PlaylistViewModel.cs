namespace WebApp.ViewModel
{
    public class PlaylistViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UsuarioId { get; set; }
        public List<MusicaViewModel> Musicas { get; set; } = new List<MusicaViewModel>();
    }
}
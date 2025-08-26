namespace WebApp.ViewModel
{
    public class PlaylistViewModel
    {
        public int Id { get; set; }
        public required string Nome_Playlist { get; set; }
        public int Usuario_Id { get; set; }
        public List<MusicaViewModel> Musicas { get; set; } = new List<MusicaViewModel>();
    }
}
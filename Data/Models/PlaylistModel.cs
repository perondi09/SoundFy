namespace Data.Models
{
    public class PlaylistModel
    {
        public int Id { get; set; }
        public required string Nome_Playlist { get; set; }
        public int Usuario_Id { get; set; }
    }
}
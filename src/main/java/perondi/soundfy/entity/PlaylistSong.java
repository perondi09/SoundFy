package perondi.soundfy.entity;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

@Entity
@Table(name = "playlist_song")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class PlaylistSong {

    @EmbeddedId
    private PlaylistSongId id;

    @ManyToOne(fetch = FetchType.LAZY)
    @MapsId("playlistId")
    @JoinColumn(name = "playlist_id")
    private Playlist playlist;

    @ManyToOne(fetch = FetchType.LAZY)
    @MapsId("songId")
    @JoinColumn(name = "song_id")
    private Song song;

    @Column(nullable = false)
    private Integer position;
}


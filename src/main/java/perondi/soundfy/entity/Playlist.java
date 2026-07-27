package perondi.soundfy.entity;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;
import perondi.soundfy.entity.Song;

import java.time.Instant;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "playlist")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Playlist {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    @Column(nullable = false)
    private String name;

    private String description;

    @Column(name = "created_at")
    private Instant createdAt = Instant.now();

    @OneToMany(mappedBy = "playlist", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<PlaylistSong> playlistSongs = new ArrayList<>();

    public void addSong(Song song) {
        int nextPosition = playlistSongs.size();
        PlaylistSong ps = new PlaylistSong(new PlaylistSongId(this.id, song.getId()), this, song, nextPosition);
        playlistSongs.add(ps);
    }
}
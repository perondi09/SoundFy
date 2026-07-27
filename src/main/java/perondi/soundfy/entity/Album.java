package perondi.soundfy.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "album")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Album {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    @Column(nullable = false)
    private String title;

    @Column(name = "artist_name")
    private String artistName;

    @Column(name = "release_year")
    private Integer releaseYear;

    @OneToMany(mappedBy = "album", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<Song> songs = new ArrayList<>();

    public void addSong(Song song) {
        songs.add(song);
        song.setAlbum(this);
    }
}
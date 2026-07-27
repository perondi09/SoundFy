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
@Table(name = "song")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Song {

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private UUID id;

    @Column(nullable = false)
    private String title;

    @Column(name = "duration_seconds")
    private Integer durationSeconds;

    @Column(name = "track_number")
    private Integer trackNumber;

    @Column(name = "file_path", nullable = false)
    private String filePath;

    @Column(name = "file_size_bytes")
    private Long fileSizeBytes;

    @Column(name = "content_type")
    private String contentType = "audio/mpeg";

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "album_id", nullable = false)
    private Album album;

    @OneToMany(mappedBy = "song", cascade = CascadeType.ALL, orphanRemoval = true)
    private List<PlaylistSong> playlistSongs = new ArrayList<>();
}


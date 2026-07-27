package perondi.soundfy.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import perondi.soundfy.entity.Song;

import java.util.List;
import java.util.UUID;

public interface SongRepository extends JpaRepository<Song, UUID> {

    List<Song> findByAlbumId(UUID albumId);
}


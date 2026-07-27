package perondi.soundfy.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import perondi.soundfy.entity.PlaylistSong;
import perondi.soundfy.entity.PlaylistSongId;

import java.util.List;
import java.util.UUID;

public interface PlaylistSongRepository extends JpaRepository<PlaylistSong, PlaylistSongId> {

    List<PlaylistSong> findByPlaylistIdOrderByPositionAsc(UUID playlistId);
}

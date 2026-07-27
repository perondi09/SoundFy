package perondi.soundfy.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import perondi.soundfy.entity.Playlist;

import java.util.UUID;

public interface PlaylistRepository extends JpaRepository<Playlist, UUID> {
}

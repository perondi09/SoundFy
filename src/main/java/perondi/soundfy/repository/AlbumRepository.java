package perondi.soundfy.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import perondi.soundfy.entity.Album;

import java.util.UUID;

public interface AlbumRepository extends JpaRepository<Album, UUID> {
}
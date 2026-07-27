package perondi.soundfy.dto.playlist;

import java.time.Instant;
import java.util.List;
import java.util.UUID;

public record PlaylistDTO(
        UUID id,
        String name,
        String description,
        Instant createdAt,
        List<PlaylistSongDTO> songs
) {
}
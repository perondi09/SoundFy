package perondi.soundfy.dto.playlist;

import java.util.UUID;

public record PlaylistSongDTO(
        UUID songId,
        String title,
        Integer durationSeconds,
        int position
) {
}

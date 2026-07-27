package perondi.soundfy.dto.album;

import java.util.UUID;

public record AlbumDTO(
        UUID id,
        String title,
        String artistName,
        Integer releaseYear,
        int songCount
) {
}


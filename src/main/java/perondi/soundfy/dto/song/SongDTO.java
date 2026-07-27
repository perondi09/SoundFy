package perondi.soundfy.dto.song;

import java.util.UUID;

public record SongDTO(
        UUID id,
        String title,
        Integer durationSeconds,
        Integer trackNumber,
        String filePath,
        Long fileSizeBytes,
        String contentType,
        UUID albumId,
        String albumTitle
) {
}
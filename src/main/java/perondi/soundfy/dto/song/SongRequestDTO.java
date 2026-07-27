package perondi.soundfy.dto.song;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.util.UUID;

public record SongRequestDTO(
        @NotBlank(message = "O título da música é obrigatório")
        String title,

        Integer durationSeconds,

        Integer trackNumber,

        @NotBlank(message = "O caminho/URL do arquivo mp3 é obrigatório")
        String filePath,

        Long fileSizeBytes,

        String contentType,

        @NotNull(message = "A música precisa estar associada a um álbum")
        UUID albumId
) {
}

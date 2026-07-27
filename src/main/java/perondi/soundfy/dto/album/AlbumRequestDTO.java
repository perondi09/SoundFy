package perondi.soundfy.dto.album;

import jakarta.validation.constraints.NotBlank;

public record AlbumRequestDTO(
        @NotBlank(message = "O título do álbum é obrigatório")
        String title,

        String artistName,

        Integer releaseYear
) {
}

package perondi.soundfy.dto.song;

import jakarta.validation.constraints.NotNull;

import java.util.UUID;

public record AddSongToPlaylistRequestDTO(
        @NotNull(message = "O id da música é obrigatório")
        UUID songId
) {
}

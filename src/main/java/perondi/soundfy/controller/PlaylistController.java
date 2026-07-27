package perondi.soundfy.controller;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import perondi.soundfy.dto.playlist.PlaylistDTO;
import perondi.soundfy.dto.playlist.PlaylistRequestDTO;
import perondi.soundfy.dto.song.AddSongToPlaylistRequestDTO;
import perondi.soundfy.service.PlaylistService;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/playlists")
@RequiredArgsConstructor
public class PlaylistController {

    private final PlaylistService playlistService;

    @GetMapping
    public List<PlaylistDTO> findAll() {
        return playlistService.findAll();
    }

    @GetMapping("/{id}")
    public PlaylistDTO findById(@PathVariable UUID id) {
        return playlistService.findById(id);
    }

    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public PlaylistDTO create(@Valid @RequestBody PlaylistRequestDTO request) {
        return playlistService.create(request);
    }

    @PutMapping("/{id}")
    public PlaylistDTO update(@PathVariable UUID id, @Valid @RequestBody PlaylistRequestDTO request) {
        return playlistService.update(id, request);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable UUID id) {
        playlistService.delete(id);
        return ResponseEntity.noContent().build();
    }

    @PostMapping("/{id}/songs")
    public PlaylistDTO addSong(@PathVariable UUID id, @Valid @RequestBody AddSongToPlaylistRequestDTO request) {
        return playlistService.addSong(id, request.songId());
    }

    @DeleteMapping("/{id}/songs/{songId}")
    public PlaylistDTO removeSong(@PathVariable UUID id, @PathVariable UUID songId) {
        return playlistService.removeSong(id, songId);
    }
}
